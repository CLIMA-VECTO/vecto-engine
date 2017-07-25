'
' This file is part of VECTO-Engine.
'
' Copyright © 2012-2017 European Union
'
' Developed by Graz University of Technology,
'              Institute of Internal Combustion Engines and Thermodynamics,
'              Institute of Technical Informatics
'
' VECTO is licensed under the EUPL, Version 1.1 or - as soon they will be approved
' by the European Commission - subsequent versions of the EUPL (the "Licence");
' You may not use VECTO except in compliance with the Licence.
' You may obtain a copy of the Licence at:
'
' https://joinup.ec.europa.eu/community/eupl/og_page/eupl
'
' Unless required by applicable law or agreed to in writing, VECTO
' distributed under the Licence is distributed on an "AS IS" basis,
' WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
' See the Licence for the specific language governing permissions and
' limitations under the Licence.
'
' Authors:
'   Stefan Hausberger, hausberger@ivt.tugraz.at, IVT, Graz University of Technology
'   Raphael Luz, luz@ivt.tugraz.at, IVT, Graz University of Technology
'   Markus Quaritsch, markus.quaritsch@tugraz.at, IVT, Graz University of Technology
'   Martin Rexeis, rexeis@ivt.tugraz.at, IVT, Graz University of Technology
'   Gérard Silberholz, silberholz@ivt.tugraz.at, IVT, Graz University of Technology
'

Public Class cJob
	Public MapFile As String
	Public FlcParentFile As String
	Public DragFile As String
	Public FlcFile As String

	Public Manufacturer As String
	Public Model As String
	Public CertNumber As String
	Public Idle_Parent As Double
	Public Idle As Double
	Public Displacement As Double
	Public RatedPower As Integer
	Public RatedSpeed As Integer
	Public FuelType As String
	Public NCVfuel As Double

	Public FCspecMeas_ColdTot As Double
	Public FCspecMeas_HotTot As Double
	Public FCspecMeas_HotUrb As Double
	Public FCspecMeas_HotRur As Double
	Public FCspecMeas_HotMw As Double

	Public CF_RegPer As Double

	Public OutPath As String

	Public WHTCsim As cWHTC
	'Public WHSCsim As cWHSC

	Private MAP As cMAP0
	Private FlcParent As cFLD0
	Private Drag As cFLD0
	Private Flc As cFLD0

	Public WHTCurbanFactor As Double
	Public WHTCruralFactor As Double
	Public WHTCmotorwayFactor As Double
	Public ColdHotBalancingFactor As Double


	Public PT1 As cPT1

	Public Function Run() As Boolean

		'Initialize Warning counter
		NumWarnings = 0

		WorkerMsg(tMsgID.Normal, "Analyzing input files")

		PT1 = New cPT1
		'PT1.Filepath = MyConfPath & "PT1.csv"
		If Not PT1.Init() Then Return False

		FlcParent = New cFLD0
		FlcParent.FilePath = FlcParentFile
		FlcParent.IdleSpeedValueForCheck = Idle_Parent
		If Not FlcParent.ReadFile(True, True) Then Return False

		Drag = New cFLD0
		Drag.FilePath = DragFile
		Drag.IdleSpeedValueForCheck = Idle_Parent
		If Not Drag.ReadFile(True, False) Then Return False

		Flc = New cFLD0
		Flc.FilePath = FlcFile
		Flc.IdleSpeedValueForCheck = Idle
		If Not Flc.ReadFile(True, True) Then Return False

		' Calculate tolerances for fuel map torque values based on Tmax_overall (2% rule, paragraph 4.3.5.5 of technical annex)
		TqStepTol = Math.Max(TqStepTol_abs, 0.02 * FlcParent.TqMax)

		MAP = New cMAP0
		MAP.FilePath = MapFile
		MAP.FLC_Parent = FlcParent
		MAP.FLC = Flc
		MAP.Motoring = Drag

		If Not MAP.ReadFile Then Return False


		If Not FlcParent.RpmCalc("CO2-parent full load") Then Return False
		If Not Flc.RpmCalc("actual engine full load") Then Return False

		' Plausibility check of input value in GUI for P_rated and n_rated
		If Math.Abs(Flc.Pmax - RatedPower) / Flc.Pmax > 0.05 Then
			WorkerMsg(tMsgID.Warn,
					  "Rated power input in GUI deviates by more than +/- 5% from rated power calculated from engine full-load curve (" &
					  RatedPower & "[kW] vs. " &
					  Math.Round(Flc.Pmax, 2) & "[kW]).")
		End If


		'Assign char. speeds from full-load curve to Map
		MAP.Map_n_idle = FlcParent.n_idle
		MAP.Map_n_lo = FlcParent.n_lo
		MAP.Map_n_pref = FlcParent.n_pref
		MAP.Map_n_95h = FlcParent.n_95h
		MAP.Map_n_hi = FlcParent.n_hi

		'DEBUG ONLY
#If DEBUG Then
		If Not MAP.WriteFLD(OutPath & "DEBUG_" & Manufacturer & "_" & Model & "_" & "_FLC_forMapCheck.vfld", False) Then _
			Return False
#End If


		'Analyse FC Map
		'WorkerMsg(tMsgID.Normal, "Analysing Map")
		'  >>> now inside cMAP0
		If Not MAP.Init() Then Return False

		'Extrapolate fuel consumption map to cover knees in full load curve
		WorkerMsg(tMsgID.Normal, "Extrapolating Map")
		If Not MAP.ExtrapolateMap() Then Return False

		'Add motoring curve to Map (for FC calc around 0 Nm)
		If Not MAP.AddDrag() Then Return False

		If Not MAP.Triangulate Then Return False

		'Limit CO2-Parent full load to FC Map maximum torque
		WorkerMsg(tMsgID.Normal, "Comparing CO2-parent maximum torque to extrapolated Map maximum torque")
		If Not MAP.LimitFlcParentToMap() Then Return False
		'Limit motoring curve to interpolated values from FC Map
		If Not MAP.LimitDragtoMap() Then Return False


		WorkerMsg(tMsgID.Normal, "WHTC Initialisation")
		'Allocation of data used for calculation and reading of input files for WHTC
		WHTCsim = New cWHTC
		WHTCsim.Drag = Drag
		'NOT VALID ANYMORE: Use original full load (not limited from Map), since WHTC target torque values shall be calculated based on original full load curve
		'simulate WHTC with actual engine not parent
		WHTCsim.FullLoad = Flc
		WHTCsim.Map = MAP
		WHTCsim.PT1 = PT1
		WHTCsim.WHTC_n_idle = Flc.n_idle
		WHTCsim.WHTC_n_lo = Flc.n_lo
		WHTCsim.WHTC_n_pref = Flc.n_pref
		WHTCsim.WHTC_n_hi = Flc.n_hi
		If Not WHTCsim.InitCycle(False) Then Return False

		WorkerMsg(tMsgID.Normal, "WHTC Simulation")
		'NOT VALID ANYMORE: Use CO2-parent full load from Map, since this is limited by map maximum torque
		'NOT VALID ANYMORE: Different full load torque used in function "CalcFC of cWHTC"
		'simulate WHTC with actual engine not parent
		WHTCsim.FullLoad = Flc
		If Not WHTCsim.CalcFC() Then Return False
		If Not WHTCsim.CalcResults(False) Then Return False

		'Calculation of WHTC-Correction and Cold-Hot-Balancing-Factor
		WHTCurbanFactor = FCspecMeas_HotUrb / Math.Round(WHTCsim.Urban, 2)
		If WHTCurbanFactor < 1 Then WHTCurbanFactor = 1
		WHTCruralFactor = FCspecMeas_HotRur / Math.Round(WHTCsim.Rural, 2)
		If WHTCruralFactor < 1 Then WHTCruralFactor = 1
		WHTCmotorwayFactor = FCspecMeas_HotMw / Math.Round(WHTCsim.Motorway, 2)
		If WHTCmotorwayFactor < 1 Then WHTCmotorwayFactor = 1

		WorkerMsg(tMsgID.Normal, "WHTC Simulation Results:")
		WorkerMsg(tMsgID.Normal, "   Urban: " & (WHTCsim.Urban).ToString("f2") & " [g/kWh].")
		WorkerMsg(tMsgID.Normal, "   Rural: " & (WHTCsim.Rural).ToString("f2") & " [g/kWh].")
		WorkerMsg(tMsgID.Normal, "   Motorway: " & (WHTCsim.Motorway).ToString("f2") & " [g/kWh].")
		WorkerMsg(tMsgID.Normal, "   Total: " & (WHTCsim.TotFCspec).ToString("f2") & " [g/kWh].")


		ColdHotBalancingFactor = 1 + 0.1 * (FCspecMeas_ColdTot - FCspecMeas_HotTot) / FCspecMeas_HotTot
		If ColdHotBalancingFactor < 1 Then ColdHotBalancingFactor = 1
		''Cut Map to R85	full load (extrapolation or interpolation, interpolation only for lower power ratings)
		'WorkerMsg(tMsgID.Normal, "Extrapolating Map to fit Parent R49 full load")
		'If Not MAP.AddFld() Then Return False


		'' *** WHSC SIMULATION START
		'WorkerMsg(tMsgID.Normal, "WHSC Initialisation")
		''Allocation of data used for calculation and reading of input files for WHSC
		'WHSCsim = New cWHSC
		'WHSCsim.Drag = Drag
		''Use original CO2-parent full load (not limited from Map), since WHTC target torque values shall be calculated based on original full load curve
		'WHSCsim.FullLoad = FlcParent
		'WHSCsim.Map = MAP
		'WHSCsim.PT1 = PT1
		'WHSCsim.WHSC_n_idle = FlcParent.n_idle
		'WHSCsim.WHSC_n_lo = FlcParent.n_lo
		'WHSCsim.WHSC_n_pref = FlcParent.n_pref
		'WHSCsim.WHSC_n_hi = FlcParent.n_hi
		'If Not WHSCsim.InitCycle(MyConfPath & "WHSC.csv") Then Return False


		'WorkerMsg(tMsgID.Normal, "WHSC Simulation")
		''Use CO2-parent full load from Map, since this is limited by map maximum torque
		''Different full load torque used in function "CalcFC of cWHSC"
		'WHSCsim.FullLoad = MAP.FLC_Parent
		'If Not WHSCsim.CalcFC() Then Return False
		'If Not WHSCsim.CalcResults(False) Then Return False

		'WorkerMsg(tMsgID.Normal, "WHSC Simulation Results:")
		'WorkerMsg(tMsgID.Normal, "   Total: " & (WHSCsim.TotFCspec).ToString("0.0000") & " [g/kWh].")
		'' *** WHSC SIMULATION END


		'calculate NCV correction factor depending on fuel type
		MAP.NCV_CorrectionFactor = Math.Round(NCVfuel / NCV_std.Item(FuelType), 4)
		MAP.temp_Map_FuelType = FuelType
		'MsgBox(MAP.NCV_CorrectionFactor)

		'Write output files
		WorkerMsg(tMsgID.Normal, "Writing XML output file")
		'If Not MAP.WriteMap(OutPath & fFILE(MapFile, False) & "_mod.vmap") Then Return False
		'If Not MAP.WriteMap(OutPath & "UNOFFICIAL_OUTPUT_" & Manufacturer & "_" & Model & "_" & CertNumber & "_FCmap.vmap") Then Return False
		'If Not MAP.WriteFLD(OutPath & fFILE(MapFile, False) & "_" & fFILE(FlcFile, False) & ".vfld") Then Return False
		'If Not MAP.WriteFLD(OutPath & "UNOFFICIAL_OUTPUT_" & Manufacturer & "_" & Model & "_" & CertNumber & "_FLC.vfld", True) Then Return False
		'If Not WriteTransFile(OutPath & "WHTC-Correction-Factors.xml") Then Return False
		MAP.WriteXmlComponentFile(OutPath & Manufacturer & "_" & Model & ".xml",
								  fFILE(FlcFile, False), Me)

		'RpmWarnings()
		'RpmWarningsGearshifting()

		WorkerMsg(tMsgID.Normal, "Completed.")


		If NumWarnings > 0 Then
			WorkerMsg(tMsgID.Warn,
					  "ATTENTION:  " & NumWarnings &
					  " Warning(s) occured: Please check detailled descriptions in 'Message Window'!")
		End If


		'Messagebox: VALID DESPITE OF WARNINGS
		MsgBox(
			"DATA EVALUATION IS COMPLETED." & Chr(13) & Chr(13) &
			"The results produced are valid for certification despite any warnings displayed in the message window!" & Chr(13) &
			Chr(13) &
			"Nevertheless, causes for warnings shall be analyzed together with the Technical Service or Type Approval Authority.",
			MsgBoxStyle.Information, MsgBoxStyle.OkOnly)


		Return True
	End Function


	Private Sub RpmWarnings()

		'If Math.Abs(n_idle - FlcParent.n_idle) > 5 Then WorkerMsg(tMsgID.Warn, "Calculated (from CO2-Parent Full Load) n_idle = " & FlcParent.n_idle & " [1/min]!")
		'WorkerMsg(tMsgID.Warn, "Calculated (from CO2-Parent Full Load) n_idle = " & FlcParent.n_idle & " [1/min]!")
		'If Math.Abs(n_lo - FlcParent.n_lo) > 5 Then WorkerMsg(tMsgID.Warn, "Calculated (from CO2-Parent Full Load) n_lo = " & FlcParent.n_lo & " [1/min]!")

		'If Math.Abs(n_pref - FlcParent.n_pref) > 5 Then WorkerMsg(tMsgID.Warn, "Calculated (from CO2-Parent Full Load) n_pref = " & FlcParent.n_pref & " [1/min]!")
		'WorkerMsg(tMsgID.Warn, "Calculated (from CO2-Parent Full Load) n_lo = " & FlcParent.n_lo & " [1/min]!")

		'If Math.Abs(n_95h - FlcParent.n_95h) > 5 Then WorkerMsg(tMsgID.Warn, "Calculated (from CO2-Parent Full Load) n_95h = " & FlcParent.n_95h & " [1/min]!")
		'WorkerMsg(tMsgID.Warn, "Calculated (from CO2-Parent Full Load) n_pref = " & FlcParent.n_pref & " [1/min]!")

		'If Math.Abs(n_hi - FlcParent.n_hi) > 5 Then WorkerMsg(tMsgID.Warn, "Calculated (from CO2-Parent Full Load) n_hi = " & FlcParent.n_hi & " [1/min]!")
		'WorkerMsg(tMsgID.Warn, "Calculated (from CO2-Parent Full Load) n_95h = " & FlcParent.n_95h & " [1/min]!")
	End Sub

	Private Sub RpmWarningsGearshifting()

		'If (n_idle - Flc.n_idle) >= 5 Then WorkerMsg(tMsgID.Warn, "n_idle for actual engine is lower than n_idle for Parent engine (will cause error in VECTO vehicle simulation)!")

		'If (n_95h - Flc.n_95h) <= -5 Then WorkerMsg(tMsgID.Warn, "n_95h for actual engine is higher than n_idle for Parent engine (will cause error in VECTO vehicle simulation)!")
	End Sub

	Private Function WriteTransFile(path As String) As Boolean
		Dim xml As New XDocument
		Dim xe As XElement
		Dim xe0 As XElement

		Try
			xe = New XElement("VECTO-Engine-TransferFile", New XAttribute("FileVersion", 1))

			xe0 = New XElement("Info")
			xe0.Add(New XElement("DateCreated", Now.ToString))
			xe0.Add(New XElement("MapFile", fFILE(MapFile, True)))
			xe0.Add(New XElement("FullloadFile", fFILE(FlcFile, True)))
			xe.Add(xe0)


			xe0 = New XElement("WHTCCorrectionFactors")
			xe0.Add(New XElement("Urban", WHTCurbanFactor))
			xe0.Add(New XElement("Rural", WHTCruralFactor))
			xe0.Add(New XElement("Motorway", WHTCmotorwayFactor))
			xe.Add(xe0)

			xml.Add(xe)
			xml.Save(path)

		Catch ex As Exception
			WorkerMsg(tMsgID.Err, "Failed to write VECTO transfer file! " & ex.Message)
			Return False
		End Try

		Return True
	End Function
End Class


