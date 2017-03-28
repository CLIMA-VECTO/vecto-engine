Imports System.Xml.Linq

Public Class cJob
	Public MapFile As String
	Public R49TqFile As String
	Public R49DragFile As String
	Public R85TqFile As String

    Public Manufacturer As String
    Public Make As String
    Public TypeID As String
    Public Idle_Parent As Single
    Public Idle As Single
    Public Displacement As Single
    Public FuelType As String
    Public NCVfuel As Single

    Public FCspecMeas_ColdTot As Single
    Public FCspecMeas_HotTot As Single
    Public FCspecMeas_HotUrb As Single
    Public FCspecMeas_HotRur As Single
    Public FCspecMeas_HotMw As Single

    Public CF_RegPer As Single

	Public OutPath As String

    Public WHTCsim As cWHTC
    Public WHSCsim As cWHSC

	Private MAP As cMAP0
	Private R49Tq As cFLD0
	Private Drag As cFLD0
	Private R85Tq As cFLD0

	Public WHTCurbanFactor As Single
	Public WHTCruralFactor As Single
    Public WHTCmotorwayFactor As Single
    Public ColdHotBalancingFactor As Single

	Public PT1 As cPT1

	Public Function Run() As Boolean

		'Initialize Warning counter
		NumWarnings = 0

		WorkerMsg(tMsgID.Normal, "Reading files")

		PT1 = New cPT1
		PT1.Filepath = MyConfPath & "PT1.csv"
		If Not PT1.Init() Then Return False

		R49Tq = New cFLD0
		R49Tq.FilePath = R49TqFile
		If Not R49Tq.ReadFile(False) Then Return False

		Drag = New cFLD0
		Drag.FilePath = R49DragFile
		If Not Drag.ReadFile(False) Then Return False

		R85Tq = New cFLD0
		R85Tq.FilePath = R85TqFile
		If Not R85Tq.ReadFile(False) Then Return False

		MAP = New cMAP0
		MAP.FilePath = MapFile
        MAP.FLC_Parent = R49Tq
        MAP.FLC_highest = R85Tq
        MAP.Motoring = Drag

		If Not MAP.ReadFile Then Return False


        If Not R49Tq.RpmCalc("Analysing Parent R49 full load") Then Return False
        If Not R85Tq.RpmCalc("Analysing Actual engine R49 full load") Then Return False

        'Assign char. speeds from full-load curve to Map
        MAP.Map_n_idle = R49Tq.n_idle
        MAP.Map_n_lo = R49Tq.n_lo
        MAP.Map_n_pref = R49Tq.n_pref
        MAP.Map_n_95h = R49Tq.n_95h
        MAP.Map_n_hi = R49Tq.n_hi

		'Analyse FC Map
		WorkerMsg(tMsgID.Normal, "Analysing Map")
		If Not MAP.Init() Then Return False

		'Extrapolate fuel consumption map to cover knees in full load curve
		WorkerMsg(tMsgID.Normal, "Extrapolating Map")
		If Not MAP.ExtrapolateMap() Then Return False

		'Add drag curve to Map (for FC calc around 0 Nm)
		If Not MAP.AddDrag() Then Return False

		If Not MAP.Triangulate Then Return False

		'Limit R49 Parent full load to FC Map maximum torque
		WorkerMsg(tMsgID.Normal, "Comparing Parent R49 maximum torque to extrapolated Map maximum torque")
		If Not MAP.LimitR49toMap() Then Return False


		'Limit Drag curve to interpolated values from FC Map
		If Not MAP.LimitDragtoMap() Then Return False


		WorkerMsg(tMsgID.Normal, "WHTC Initialisation")
		'Allocation of data used for calculation and reading of input files for WHTC
		WHTCsim = New cWHTC
		WHTCsim.Drag = Drag
		'Use original R49 Parent full load (not limited from Map), since WHTC target torque values shall be calculated based on original full load curve
		WHTCsim.FullLoad = R49Tq
		WHTCsim.Map = MAP
		WHTCsim.PT1 = PT1
        WHTCsim.WHTC_n_idle = R49Tq.n_idle
        WHTCsim.WHTC_n_lo = R49Tq.n_lo
        WHTCsim.WHTC_n_pref = R49Tq.n_pref
        WHTCsim.WHTC_n_hi = R49Tq.n_hi
		If Not WHTCsim.InitCycle(False, MyConfPath & "WHTC.csv") Then Return False

		WorkerMsg(tMsgID.Normal, "WHTC Simulation")
		'Use R49 Parent full load from Map, since this is limited by map maximum torque
        'Different full load torque used in function "CalcFC of cWHTC"
        WHTCsim.FullLoad = MAP.FLC_Parent
		If Not WHTCsim.CalcFC() Then Return False
		If Not WHTCsim.CalcResults(False) Then Return False

				  "   WHTC Measurement total specific fuel consumption: " & (WHTCmes.TotFCspec).ToString("0.00") & " [g/kWh].")

        'Calculation of WHTC-Correction and Cold-Hot-Balancing-Factor
        WHTCurbanFactor = FCspecMeas_HotUrb / WHTCsim.Urban
        If WHTCurbanFactor < 1 Then WHTCurbanFactor = 1
        WHTCruralFactor = FCspecMeas_HotRur / WHTCsim.Rural
        If WHTCruralFactor < 1 Then WHTCruralFactor = 1
        WHTCmotorwayFactor = FCspecMeas_HotMw / WHTCsim.Motorway
        If WHTCmotorwayFactor < 1 Then WHTCmotorwayFactor = 1

		WorkerMsg(tMsgID.Normal, "WHTC Simulation Results:")
		WorkerMsg(tMsgID.Normal, "   Urban: " & (WHTCsim.Urban).ToString("0.00") & " [g/kWh].")
		WorkerMsg(tMsgID.Normal, "   Rural: " & (WHTCsim.Rural).ToString("0.00") & " [g/kWh].")
		WorkerMsg(tMsgID.Normal, "   Motorway: " & (WHTCsim.Motorway).ToString("0.00") & " [g/kWh].")
        WorkerMsg(tMsgID.Normal, "   Total: " & (WHTCsim.TotFCspec).ToString("0.00") & " [g/kWh].")


        ColdHotBalancingFactor = 1 + 0.1 * (FCspecMeas_ColdTot - FCspecMeas_HotTot) / FCspecMeas_HotTot
        If ColdHotBalancingFactor < 1 Then ColdHotBalancingFactor = 1
		''Cut Map to R85	full load (extrapolation or interpolation, interpolation only for lower power ratings)
		'WorkerMsg(tMsgID.Normal, "Extrapolating Map to fit Parent R49 full load")
		'If Not MAP.AddFld() Then Return False





        ' *** WHSC SIMULATION START
        WorkerMsg(tMsgID.Normal, "WHSC Initialisation")
        'Allocation of data used for calculation and reading of input files for WHSC
        WHSCsim = New cWHSC
        WHSCsim.Drag = Drag
        'Use original R49 Parent full load (not limited from Map), since WHTC target torque values shall be calculated based on original full load curve
        WHSCsim.FullLoad = R49Tq
        WHSCsim.Map = MAP
        WHSCsim.PT1 = PT1
        WHSCsim.WHSC_n_idle = R49Tq.n_idle
        WHSCsim.WHSC_n_lo = R49Tq.n_lo
        WHSCsim.WHSC_n_pref = R49Tq.n_pref
        WHSCsim.WHSC_n_hi = R49Tq.n_hi
        If Not WHSCsim.InitCycle(MyConfPath & "WHSC.csv") Then Return False


        WorkerMsg(tMsgID.Normal, "WHSC Simulation")
        'Use R49 Parent full load from Map, since this is limited by map maximum torque
        'Different full load torque used in function "CalcFC of cWHSC"
        WHSCsim.FullLoad = MAP.FLC_Parent
        If Not WHSCsim.CalcFC() Then Return False
        If Not WHSCsim.CalcResults(False) Then Return False

        WorkerMsg(tMsgID.Normal, "WHSC Simulation Results:")
        WorkerMsg(tMsgID.Normal, "   Total: " & (WHSCsim.TotFCspec).ToString("0.0000") & " [g/kWh].")
        ' *** WHSC SIMULATION END




		'Write output files
		WorkerMsg(tMsgID.Normal, "Writing output files")
		If Not MAP.WriteMap(OutPath & fFILE(MapFile, False) & "_mod.vmap") Then Return False
		If Not MAP.WriteFLD(OutPath & fFILE(MapFile, False) & "_" & fFILE(R85TqFile, False) & ".vfld") Then Return False
		If Not WriteTransFile(OutPath & "WHTC-Correction-Factors.xml") Then Return False
		MAP.WriteXmlComponentFile(OutPath & fFILE(MapFile, False) & "_" & fFILE(R85TqFile, False) & ".xml",
								  fFILE(R85TqFile, False), Me)

        'RpmWarnings()
        'RpmWarningsGearshifting()

		WorkerMsg(tMsgID.Normal, "Completed.")


		If NumWarnings > 0 Then
			WorkerMsg(tMsgID.Warn,
					  "ATTENTION:  " & NumWarnings &
					  " Warnings occured: Please check detailled descriptions in 'Message Window'!")
		End If


		Return True
	End Function


	Private Sub RpmWarnings()

        'If Math.Abs(n_idle - R49Tq.n_idle) > 5 Then WorkerMsg(tMsgID.Warn, "Calculated (from Parent R49 Full Load) n_idle = " & R49Tq.n_idle & " [1/min]!")
			WorkerMsg(tMsgID.Warn, "Calculated (from Parent R49 Full Load) n_idle = " & R49Tq.n_idle & " [1/min]!")
        'If Math.Abs(n_lo - R49Tq.n_lo) > 5 Then WorkerMsg(tMsgID.Warn, "Calculated (from Parent R49 Full Load) n_lo = " & R49Tq.n_lo & " [1/min]!")

        'If Math.Abs(n_pref - R49Tq.n_pref) > 5 Then WorkerMsg(tMsgID.Warn, "Calculated (from Parent R49 Full Load) n_pref = " & R49Tq.n_pref & " [1/min]!")
			WorkerMsg(tMsgID.Warn, "Calculated (from Parent R49 Full Load) n_lo = " & R49Tq.n_lo & " [1/min]!")

        'If Math.Abs(n_95h - R49Tq.n_95h) > 5 Then WorkerMsg(tMsgID.Warn, "Calculated (from Parent R49 Full Load) n_95h = " & R49Tq.n_95h & " [1/min]!")
			WorkerMsg(tMsgID.Warn, "Calculated (from Parent R49 Full Load) n_pref = " & R49Tq.n_pref & " [1/min]!")

        'If Math.Abs(n_hi - R49Tq.n_hi) > 5 Then WorkerMsg(tMsgID.Warn, "Calculated (from Parent R49 Full Load) n_hi = " & R49Tq.n_hi & " [1/min]!")
			WorkerMsg(tMsgID.Warn, "Calculated (from Parent R49 Full Load) n_95h = " & R49Tq.n_95h & " [1/min]!")

		If Math.Abs(n_hi - R49Tq.n_hi) > 5 Then _
			WorkerMsg(tMsgID.Warn, "Calculated (from Parent R49 Full Load) n_hi = " & R49Tq.n_hi & " [1/min]!")
	End Sub

	Private Sub RpmWarningsGearshifting()

        'If (n_idle - R85Tq.n_idle) >= 5 Then WorkerMsg(tMsgID.Warn, "n_idle for actual engine is lower than n_idle for Parent engine (will cause error in VECTO vehicle simulation)!")

        'If (n_95h - R85Tq.n_95h) <= -5 Then WorkerMsg(tMsgID.Warn, "n_95h for actual engine is higher than n_idle for Parent engine (will cause error in VECTO vehicle simulation)!")

		If (n_95h - R85Tq.n_95h) <= -5 Then _
			WorkerMsg(tMsgID.Warn,
					  "n_95h for actual engine is higher than n_idle for Parent engine (will cause error in VECTO vehicle simulation)!")
	End Sub

	Private Function WriteTransFile(ByVal path As String) As Boolean
		Dim xml As New XDocument
		Dim xe As XElement
		Dim xe0 As XElement

		Try
			xe = New XElement("VECTO-Engine-TransferFile", New XAttribute("FileVersion", 1))

			xe0 = New XElement("Info")
			xe0.Add(New XElement("DateCreated", Now.ToString))
			xe0.Add(New XElement("MapFile", fFILE(MapFile, True)))
			xe0.Add(New XElement("R85File", fFILE(R85TqFile, True)))
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


