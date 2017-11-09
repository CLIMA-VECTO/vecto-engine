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
Imports System.IO
Imports System.Xml
Imports System.Xml.Schema
Imports TUGraz.VectoHashing

Public Class cMAP0
	Public FilePath As String

	Private ReadOnly MapPoints As New List(Of cMapPoint)
	Private iMapDim As Integer

	Private ReadOnly RPMlists As New SortedDictionary(Of Double, cRPMlist)
	Public FLC_Parent As cFLD0
	Public FLC As cFLD0
	Public Motoring As cFLD0

	Public Map_n_idle As Double
	Public Map_n_lo As Double
	Public Map_n_pref As Double
	Public Map_n_95h As Double
	Public Map_n_hi As Double

	Public NCV_CorrectionFactor As Double
	Public temp_Map_FuelType As String

	Private TqStep As Double
	Private nUStepTol As Double

	Private ReadOnly TqList As New List(Of Double)

	Private FuelMap As cDelaunayMap

	Private ReadOnly LnU_out As New List(Of Double)
	Private ReadOnly LTq_out As New List(Of Double)
	Private ReadOnly LTqMot_out As New List(Of Double)
	Private ReadOnly LnU_outTemp As New List(Of Double)
	Private ReadOnly LTq_outTemp As New List(Of Double)


	Public Function ReadFile() As Boolean
		Dim file As cFile_V3
		Dim line As String()
		Dim nU As Double
		Dim Tq As Double
		Dim FC As Double
		Dim StringForSplit As String
		Dim StringsAfterSplit As String()

		'Stop if there's no file
		If FilePath = "" OrElse Not IO.File.Exists(FilePath) Then
			WorkerMsg(tMsgID.Err, "Map file not found! (" & FilePath & ")")
			Return False
		End If

		'Open file
		file = New cFile_V3
		If Not file.OpenRead(FilePath) Then
			file = Nothing
			WorkerMsg(tMsgID.Err, "Failed to open file (" & FilePath & ") !")
			Return False
		End If

		MapPoints.Clear()
		iMapDim = -1
		FuelMap = New cDelaunayMap

		'Skip Header
		file.ReadLine()


		Try

			Do While Not file.EndOfFile

				'Line read
				line = file.ReadLine

				iMapDim += 1

				nU = line(0)
				Tq = line(1)
				FC = line(2)

				If nU < 0 Or Tq < (0 - TqStepTol) Or FC <= 0 Then
					WorkerMsg(tMsgID.Err, "Invalid map point! Line number " & iMapDim + 2 & " (" & FilePath & ")")
					If nU < 0 Then
						WorkerMsg(tMsgID.Err, "  >>> Negative engine speed value (" & nU & ")!")
					ElseIf Tq < (0 - TqStepTol) Then
						WorkerMsg(tMsgID.Err, "  >>> Engine torque value lower than specified tolerance (" & Tq & ")!")
					Else
						WorkerMsg(tMsgID.Err, "  >>> Zero or negative fuel consumption value (" & FC & ")!")
					End If
					GoTo lbEr

				Else

					StringForSplit = line(0)
					If StringForSplit.Contains(".") Then
						StringsAfterSplit = StringForSplit.Split(New String() {"."c}, StringSplitOptions.RemoveEmptyEntries)
						If StringsAfterSplit(1).Length <> 2 Then
							WorkerMsg(tMsgID.Err,
									  "Value needs to have exactly 2 digits after the decimal point: " & StringForSplit &
									  " [1/min]! Line number: " & iMapDim + 2 & " (" & FilePath & ")")
							GoTo lbEr
						End If
					Else
						WorkerMsg(tMsgID.Err,
								  "Value does not contain a valid decimal separator: " & StringForSplit & " [1/min]! Line number: " &
								  iMapDim + 2 & " (" & FilePath & ")")
						GoTo lbEr
					End If

					StringForSplit = line(1)
					If StringForSplit.Contains(".") Then
						StringsAfterSplit = StringForSplit.Split(New String() {"."c}, StringSplitOptions.RemoveEmptyEntries)
						If StringsAfterSplit(1).Length <> 2 Then
							WorkerMsg(tMsgID.Err,
									  "Value needs to have exactly 2 digits after the decimal point: " & StringForSplit &
									  " [Nm]! Line number: " & iMapDim + 2 & " (" & FilePath & ")")
							GoTo lbEr
						End If
					Else
						WorkerMsg(tMsgID.Err,
								  "Value does not contain a valid decimal separator: " & StringForSplit & " [Nm]! Line number: " &
								  iMapDim + 2 & " (" & FilePath & ")")
						GoTo lbEr
					End If

					StringForSplit = line(2)
					If StringForSplit.Contains(".") Then
						StringsAfterSplit = StringForSplit.Split(New String() {"."c}, StringSplitOptions.RemoveEmptyEntries)
						If StringsAfterSplit(1).Length <> 2 Then
							WorkerMsg(tMsgID.Err,
									  "Value needs to have exactly 2 digits after the decimal point: " & StringForSplit &
									  " [g/h]! Line number: " & iMapDim + 2 & " (" & FilePath & ")")
							GoTo lbEr
						End If
					Else
						WorkerMsg(tMsgID.Err,
								  "Value does not contain a valid decimal separator: " & StringForSplit & " [g/h]! Line number: " &
								  iMapDim + 2 & " (" & FilePath & ")")
						GoTo lbEr
					End If


				End If

				MapPoints.Add(New cMapPoint(nU, Tq, FC))

			Loop

		Catch ex As Exception

			WorkerMsg(tMsgID.Err, "Error during file read! Line number " & iMapDim + 2 & " (" & FilePath & ")")
			GoTo lbEr

		End Try


		'Close file
		file.Close()

		Return True


		'ERROR-label for clean Abort
lbEr:
		file.Close()

		Return False
	End Function


	Public Function Init() As Boolean
		Dim i As Integer
		Dim nU_Double As Double
		Dim nUStep As Double
		Dim TqStepMax As Double
		Dim IntervalCount As Integer
		Dim mp0 As cMapPoint
		Dim Matched As Boolean
		Dim TargetRPMs As New List(Of Double)
		Dim rl As cRPMlist
		' now in Declarations of Class
		'Dim TqList As New List(Of Double)
		Dim Tq As Double
		Dim TqTarget As Double
		Dim MapSpeed_nA As Double
		Dim MapSpeed_nB As Double
		Dim MapSpeed_n57 As Double
		Dim dn_idle_A_44 As Double
		Dim dn_B_95h_44 As Double
		Dim dn_idle_A_35 As Double
		Dim dn_B_95h_35 As Double
		Dim dn_idle_A_53 As Double
		Dim dn_B_95h_53 As Double
		Dim dn_44 As Double
		Dim dn_35 As Double
		Dim dn_53 As Double
		Dim IntervalTarget(1) As Integer
		Dim NumPointsBelowFL As Integer


		' ADDITIONAL CHECKS for input data
		WorkerMsg(tMsgID.Normal, "Analysing validity of input data acc. to CO2-family definition")
		'**************************************
		' Checks for family concept
		'**************************************

		' 1. idle speed parent <= idle speed of engine to be certified
		If FLC.n_idle < FLC_Parent.n_idle Then
			WorkerMsg(tMsgID.Err,
					  "Idle speed of engine to be certified lower than idle speed of CO2-parent engine (not valid acc. to CO2-family definition).")
			Return False
		End If
		' 2. n_95h speed of engine to be certified within +/-3% of n_95h speed of parent
		If Math.Abs(FLC.n_95h - FLC_Parent.n_95h) / FLC_Parent.n_95h > 0.03 Then
			WorkerMsg(tMsgID.Err,
					  "n_95h speed of engine to be certified deviates more than +/-3% from n_95h speed of CO2-parent engine (not valid acc. to CO2-family definition).")
			Return False
		End If
		' 3. n_57 speed of engine to be certified within +/-3% of n_57 speed of parent
		If Math.Abs(FLC.n_57 - FLC_Parent.n_57) / FLC_Parent.n_57 > 0.03 Then
			WorkerMsg(tMsgID.Err,
					  "n_57 speed of engine to be certified deviates more than +/-3% from n_57 speed of CO2-parent engine (not valid acc. to CO2-family definition).")
			Return False
		End If
		' 4. Tq full-load parent >= tq full-load engine to be certified
		For i = 0 To FLC_Parent.iDim_orig
			If _
				FLC_Parent.Tq_orig(FLC_Parent.LnU_orig(i)) < FLC.Tq_orig(FLC_Parent.LnU_orig(i)) AndAlso
				FLC_Parent.Tq_orig(FLC_Parent.LnU_orig(i)) > 0 Then
				WorkerMsg(tMsgID.Err,
						  "Full-load torque of CO2-parent engine lower than full-load torque of engine to be certified at " &
						  FLC_Parent.LnU_orig(i) & "[1/min] (not valid acc. to CO2-family definition).")
				Return False
			End If
		Next


		'**************************************
		' Checks for family concept - END
		'**************************************


		WorkerMsg(tMsgID.Normal, "Analysing Map")

		'Calculate tolerance for engine speed value of fuel map points based on highest target engine speed (paragraph 4.3.5.5 of technical annex)
		nUStepTol = 0.01 * Map_n_95h

		'Define target rpms for FC Map
		TargetRPMs.Clear()

		MapSpeed_n57 = 0.565 * (0.45 * Map_n_lo + 0.45 * Map_n_pref + 0.1 * Map_n_hi - Map_n_idle) * 2.0327 + Map_n_idle
		MapSpeed_nA = MapSpeed_n57 - 0.05 * (Map_n_95h - Map_n_idle)
		MapSpeed_nB = MapSpeed_n57 + 0.08 * (Map_n_95h - Map_n_idle)

		TargetRPMs.Add(Math.Round(Map_n_idle, 2))
		TargetRPMs.Add(Math.Round(MapSpeed_nA, 2))
		TargetRPMs.Add(Math.Round(MapSpeed_nB, 2))
		TargetRPMs.Add(Math.Round(Map_n_95h, 2))


		'Calculate speed intervals for definition of 4/4, 3/5 or 5/3 distribution
		dn_idle_A_44 = (MapSpeed_nA - Map_n_idle) / 4
		dn_B_95h_44 = (Map_n_95h - MapSpeed_nB) / 4
		dn_idle_A_35 = (MapSpeed_nA - Map_n_idle) / 3
		dn_B_95h_35 = (Map_n_95h - MapSpeed_nB) / 5
		dn_idle_A_53 = (MapSpeed_nA - Map_n_idle) / 5
		dn_B_95h_53 = (Map_n_95h - MapSpeed_nB) / 3

		dn_44 = Math.Abs(dn_idle_A_44 - dn_B_95h_44)
		dn_35 = Math.Abs(dn_idle_A_35 - dn_B_95h_35)
		dn_53 = Math.Abs(dn_idle_A_53 - dn_B_95h_53)

		If (dn_44 < dn_35) And (dn_44 < dn_53) Then
			IntervalTarget(0) = 4
			IntervalTarget(1) = 4
		ElseIf (dn_35 < dn_44) And (dn_35 < dn_53) Then
			IntervalTarget(0) = 3
			IntervalTarget(1) = 5
		ElseIf (dn_53 < dn_44) And (dn_53 < dn_35) Then
			IntervalTarget(0) = 5
			IntervalTarget(1) = 3
		End If


		'Check if values at zero torque are present
		If (From mp As cMapPoint In MapPoints Where Math.Abs(mp.Tq) <= TqStepTol).Count = 0 Then
			WorkerMsg(tMsgID.Err, "No values in FC map at zero torque!")
			WorkerMsg(tMsgID.Err, "  >>> Check also tolerances for torque points at zero torque!")
			Return False
		End If

		'Count points between n_idle to MapSpeed_nA at zero torque
		IntervalCount = (From mp As cMapPoint In MapPoints Where Math.Abs(mp.Tq) <= TqStepTol _
																 AndAlso mp.nU > Map_n_idle + nUStepTol _
																 AndAlso mp.nU < MapSpeed_nA - nUStepTol).Count + 1

		If IntervalCount <> IntervalTarget(0) Then
			WorkerMsg(tMsgID.Err,
					  "Incorrect rpm intervals between n_idle and n_A (" & IntervalCount & " intervals, but should be " &
					  IntervalTarget(0) & ")!")
			WorkerMsg(tMsgID.Err, "  >>> Check also tolerances for torque points at zero torque!")
			Return False
		End If

		'Fill in list part1
		nUStep = (MapSpeed_nA - Map_n_idle) / IntervalCount
		For i = 1 To IntervalCount - 1
			TargetRPMs.Add(Math.Round(Map_n_idle + nUStep * i, 1))
		Next

		'Count points between MapSpeed_nB and n_95h at zero torque
		IntervalCount = (From mp As cMapPoint In MapPoints Where Math.Abs(mp.Tq) <= TqStepTol _
																 AndAlso mp.nU > MapSpeed_nB + nUStepTol _
																 AndAlso mp.nU < Map_n_95h - nUStepTol).Count + 1

		If IntervalCount <> IntervalTarget(1) Then
			WorkerMsg(tMsgID.Err,
					  "Incorrect rpm intervals between n_B and n_95h (" & IntervalCount & " intervals, but should be " &
					  IntervalTarget(1) & ")!")
			WorkerMsg(tMsgID.Err, "  >>> Check also tolerances for torque points at zero torque!")
			Return False
		End If

		'Fill in list part2
		nUStep = (Map_n_95h - MapSpeed_nB) / IntervalCount
		For i = 1 To IntervalCount - 1
			TargetRPMs.Add(Math.Round(MapSpeed_nB + nUStep * i, 1))
		Next

		'Sort list
		TargetRPMs.Sort()

		'Feed RPM lists and check rpm tolerances
		RPMlists.Clear()

		For Each nU_Double In TargetRPMs
			RPMlists.Add(nU_Double, New cRPMlist(nU_Double))
		Next

		For Each mp0 In MapPoints

			Matched = False

			For Each nU_Double In TargetRPMs
				If Math.Abs(nU_Double - mp0.nU) <= nUStepTol Then
					RPMlists(nU_Double).MapPoints.Add(mp0)
					Matched = True
					Exit For
				End If
			Next

			If Not Matched Then
				WorkerMsg(tMsgID.Err, "Invalid rpm value in FC map! (" & mp0.nU & " [1/min], " & mp0.Tq & " [Nm])")
				WorkerMsg(tMsgID.Err,
						  "  >>> Calculated target rpm values are: " & String.Join(", ", TargetRPMs.ToArray) & " [1/min].")
				Return False
			End If

		Next

		'Sort rpm lists by torque
		For Each rl In RPMlists.Values
			rl.MapPoints = (From mp As cMapPoint In rl.MapPoints Order By mp.Tq).ToList
		Next


		'Exact Torque step size for map (based on Parent full-load)
		TqStepMax = FLC_Parent.TqMax / 10


		'CHANGE IN v1.2: not needed any more since torque grid is fixed
		'***
		'Check of maximum torque interval allowed (over each map point, also for highest)
		'For Each rl In RPMlists.Values
		'    If rl.MapPoints.Count > 2 Then

		'        For i = 1 To rl.MapPoints.Count - 1 'NOT skip highest torque (index from [0] to [count-1])  -->  [count-1] is highest point

		'            TqStep = rl.MapPoints(i).Tq - rl.MapPoints(i - 1).Tq

		'            'Check step size
		'            ' 2*Tolerance value (since first can be lower and second can be higher)
		'            If TqStep > TqStepMax + 2 * TqStepTol Then
		'                WorkerMsg(tMsgID.Err,
		'                          "Invalid torque interval in FC map at " & rl.TargetRPM & " [1/min] between " & rl.MapPoints(i - 1).Tq &
		'                          "[Nm] and " & rl.MapPoints(i).Tq & "[Nm]!")
		'                WorkerMsg(tMsgID.Err, "	 Calculated torque interval = " & TqStep & " [Nm].")
		'                WorkerMsg(tMsgID.Err,
		'                          "	 Maximum allowed torque interval = " & TqStepMax & " [Nm] + " & 2 * TqStepTol & "[Nm] tolerance.")
		'                Return False
		'            End If
		'        Next

		'    End If

		'Next


		'Variable only needed for function "AddFld()"
		TqStep = TqStepMax

		'Create list with target torque points
		Tq = 0
		Do
			TqList.Add(Tq)
			Tq += TqStepMax
		Loop Until Tq > FLC_Parent.TqMax + TqStepTol


		'Check correct location of every torque value in map
		For Each rl In RPMlists.Values

			i = -1
			For Each mp0 In rl.MapPoints
				i += 1

				'Different handling of full-load point
				' Index start from 0, so (i-1) is the last value

				If i < (rl.MapPoints.Count - 1) Then

					'Regular points

					'Different handling of points close to full-load (5% rule; combining last point below full-load acc. to par. 4.3.5.2.2 of technical annex)
					'Change of target torque value necessary
					'If (TqList(i) > (FLC_Parent.Tq(mp0.nU) - 0.05 * FLC_Parent.TqMax)) Then
					If (TqList(i) > (FLC_Parent.Tq_orig(rl.TargetRPM) - 0.05 * FLC_Parent.TqMax)) Then

						'TqTarget = FLC_Parent.Tq(mp0.nU)

						'If Math.Abs(mp0.Tq - TqTarget) > TqStepTol Then
						WorkerMsg(tMsgID.Err, "Invalid torque value in FC map! (" & mp0.nU & " [1/min], " & mp0.Tq & " [Nm])")
						WorkerMsg(tMsgID.Err, "	 Expected torque = full load.")
						Return False
						'End If

					Else
						TqTarget = TqList(i)

						If Math.Abs(mp0.Tq - TqTarget) > TqStepTol Then
							WorkerMsg(tMsgID.Err, "Invalid torque value in FC map! (" & mp0.nU & " [1/min], " & mp0.Tq & " [Nm])")
							WorkerMsg(tMsgID.Err, "	 Expected torque = " & TqTarget & " [Nm].")
							WorkerMsg(tMsgID.Err, "	 Allowed tolerance is +/- " & TqStepTol & " [Nm].")
							Return False
						End If

					End If

				Else

					'Full load point has to be located at least above previously measured point
					'TqTarget = (FLC_Parent.Tq(mp0.nU) - TqStepTol)
					TqTarget = rl.MapPoints(rl.MapPoints.Count - 2).Tq

					If mp0.Tq <= TqTarget Then
						WorkerMsg(tMsgID.Err, "Invalid torque value in FC map! (" & mp0.nU & " [1/min], " & mp0.Tq & " [Nm])")
						WorkerMsg(tMsgID.Err, "	 Expected torque = full load.")
						WorkerMsg(tMsgID.Err, "	 Full load torque should be >= " & TqTarget & " [Nm].")


						Return False
					End If

				End If

			Next

		Next


		'Check validity of map for family concept
		'   >>> 54 target points of the parent map must be located below the full-load curve of the engine to be certified
		NumPointsBelowFL = 0
		For Each rl In RPMlists.Values
			For Each mp0 In rl.MapPoints
				If mp0.Tq < FLC.Tq_orig(mp0.nU) Then
					NumPointsBelowFL += 1
				End If
			Next
		Next
		If NumPointsBelowFL < 54 Then
			WorkerMsg(tMsgID.Err,
					  "FC map not valid for engine to be certified, less than 54 map points below full-load curve (not valid acc. to CO2-family definition).")
			Return False
		End If


		'Copy first rpmlist to idle -100
		rl = New cRPMlist(Map_n_idle - 100)
		For Each mp0 In RPMlists(Map_n_idle).MapPoints
			rl.MapPoints.Add(New cMapPoint(Map_n_idle - 100, mp0.Tq, mp0.FC))
		Next

		RPMlists.Add(Map_n_idle - 100, rl)

		'Copy last rpmlist to last rpm +500
		rl = New cRPMlist(TargetRPMs.Last + 500)
		For Each mp0 In RPMlists(TargetRPMs.Last).MapPoints
			rl.MapPoints.Add(New cMapPoint(TargetRPMs.Last + 500, mp0.Tq, mp0.FC))
		Next

		RPMlists.Add(TargetRPMs.Last + 500, rl)


		'Not used any more
		MapPoints.Clear()


		Return True
	End Function

	Public Function Triangulate() As Boolean
		Dim rl As cRPMlist
		Dim mp0 As cMapPoint

		'FC Map Triangulate
		For Each rl In RPMlists.Values
			For Each mp0 In rl.MapPoints
				FuelMap.AddPoints(mp0.nU, mp0.Tq, mp0.FC)
			Next
		Next


		If Not FuelMap.Triangulate() Then
			WorkerMsg(tMsgID.Err, "Failed to triangulate FC map!")
			Return False
		End If

		Return True
	End Function

	Public Function ExtrapolateMap() As Boolean
		Dim LinReg As cRegression
		Dim LinRegResult As cRegression.RegressionProcessInfo
		Dim lX As New List(Of Double)
		Dim lY As New List(Of Double)
		Dim Tq As Double
		Dim Tq_FullLoad As Double
		Dim i As Integer
		Dim TqDistMax As Double


		For Each rl In RPMlists.Values

			'Maximum torque distance for extrapolation defined as exactly one torque stepwidth for FC map
			TqDistMax = FLC_Parent.TqMax / 10

			'Tq = rl.MapPoints.Last.Tq + TqDistMax
			'Extrapolate for each speed step to overall max Torque + 1 grid
			'Tq = FLC_Parent.TqMax + TqDistMax

			'Abort if not enough points below CO2-Parent full load
			If rl.MapPoints.Count < 3 Then
				WorkerMsg(tMsgID.Err, "Extrapolation not possible: Less than 3 points in fuel map at " & rl.TargetRPM & " [1/min] !")
				Return False
			End If

			'Linear regression of last three map points
			lX.Clear()
			lY.Clear()
			LinReg = New cRegression

			For i = Math.Max(0, rl.MapPoints.Count - 3) To rl.MapPoints.Count - 1
				lX.Add(rl.MapPoints(i).Tq)
				lY.Add(rl.MapPoints(i).FC)
			Next

			LinRegResult = LinReg.Regress(lX.ToArray, lY.ToArray)

			'Tq_FullLoad = rl.MapPoints.Last.Tq

			For i = 0 To (TqList.Count - 1)
				If TqList(i) > rl.MapPoints.Last.Tq Then
					'Add extrapolated points above full load with standard stepsize up to (T_max_overall + one grid step)
					'rl.MapPoints.Add(New cMapPoint(rl.TargetRPM, Tq, LinRegResult.a + LinRegResult.b * Tq))
					rl.MapPoints.Add(New cMapPoint(rl.TargetRPM, TqList(i), LinRegResult.a + LinRegResult.b * TqList(i)))
					'Tq += TqDistMax
				End If
			Next
			'Loop Until Tq >= FLC_Parent.TqMax + TqDistMax

			'Add last point exactly at (T_max_overall + one grid step)
			Tq = FLC_Parent.TqMax + TqDistMax
			rl.MapPoints.Add(New cMapPoint(rl.TargetRPM, Tq, LinRegResult.a + LinRegResult.b * Tq))


		Next

		Return True
	End Function

	Public Function LimitFlcParentToMap() As Boolean
		Dim i As Integer
		Dim nU As Double
		Dim Tq As Double
		Dim TqMaxMap As Double

		For i = 0 To FLC_Parent.iDim
			nU = FLC_Parent.LnU(i)
			Tq = FLC_Parent.LTq(i)
			TqMaxMap = TqMax(nU)
			If Tq > TqMaxMap Then
				FLC_Parent.LTq(i) = TqMaxMap
				WorkerMsg(tMsgID.Warn,
						  "   CO2-Parent Full Load torque exceeds Fuel Map max torque at " & Math.Round(nU, 2) & " [1/min] by " &
						  Math.Round(Tq - TqMaxMap, 2) & "[Nm] !")
				WorkerMsg(tMsgID.Warn,
						  "     >>> Maximum torque at " & Math.Round(nU, 2) & " [1/min] will be limited to " &
						  Math.Round(TqMaxMap, 2) & "[Nm] for WHTC simulation!")
			End If
		Next

		Return True
	End Function

	Public Function LimitDragtoMap() As Boolean
		Dim i As Integer
		Dim nU As Double
		Dim Tq As Double
		Dim TqMinMap As Double

		For i = 0 To Motoring.iDim
			nU = Motoring.LnU(i)
			Tq = Motoring.LTq(i)
			TqMinMap = TqMin(nU)
			'If Tq < TqMinMap Then
			'    Motoring.LTq(i) = TqMinMap
			'End If

			' set all motoring torque values within fuel map speed range to interpolated torque values from map
			If nU > (Map_n_idle - 100) And nU < (Map_n_95h + 500) Then
				Motoring.LTq(i) = TqMinMap
			End If
		Next

		Return True
	End Function

	Public Function AddFld() As Boolean
		Dim mp0 As cMapPoint
		Dim mp1 As cMapPoint
		Dim FC As Double
		Dim LinReg As cRegression
		Dim LinRegResult As cRegression.RegressionProcessInfo
		Dim lX As New List(Of Double)
		Dim lY As New List(Of Double)
		Dim Tq As Double
		Dim TqR85 As Double
		Dim TqMaxMap As Double
		Dim PointFoundAbove As Boolean
		Dim i As Integer
		Dim nUInt As Integer
		Dim nU As Double
		Dim rl0 As cRPMlist
		Dim Cancel As Boolean
		Dim TqDistMax As Double
		Dim rl1 As cRPMlist


		'Add map points at R85 rpm steps if over Map torque
		For i = 0 To FLC.iDim

			nU = FLC.LnU(i)
			nUInt = CInt(nU)
			TqR85 = FLC.LTq(i)
			TqMaxMap = TqMax(nU)
			rl1 = RPMlists.Last.Value

			'Only add points if (R85 full load torque > Map full load torque) AND (R85 speed <= maximum Map speed)
			If TqR85 > TqMaxMap And nU <= rl1.TargetRPM Then

				Cancel = False

				For Each rl In RPMlists.Values
					If Math.Abs(nU - rl.TargetRPM) < nUStepTol Then
						Cancel = True
						Exit For
					End If
				Next

				If Cancel Then Continue For

				rl0 = New cRPMlist(nUInt)

				Tq = 0
				Do While Tq < (TqMaxMap - TqStepTol)
					rl0.MapPoints.Add(New cMapPoint(nU, Tq, FuelMap.Intpol(nU, Tq)))
					Tq += TqStep
				Loop

				rl0.MapPoints.Add(New cMapPoint(nU, TqMaxMap, FuelMap.Intpol(nU, TqMaxMap)))

				'Mark points to be removed close to R85 max torque
				'Check tolerances for distance from R85 still to be implemented, if needed at all
				'rl0.MapPoints(rl0.MapPoints.Count - 1).MarkedForDeathAfterR85Cut = True

				'Add motoring curve
				rl0.MapPoints.Insert(0, New cMapPoint(nU, Motoring.Tq(nU), 0))

				RPMlists.Add(nUInt, rl0)

			End If
		Next


		'Extrapolate/Interpolate FC values at R85 full load curve and cut above R85 full load
		For Each rl In RPMlists.Values

			'R85 full load at this rpm
			Tq = FLC.Tq(rl.TargetRPM)
			i = -1
			PointFoundAbove = False

			For Each mp1 In rl.MapPoints
				i += 1

				If PointFoundAbove Then

					'Cut above full load
					mp1.MarkedForDeath = True

				Else

					If mp1.Tq - TqStepTol > Tq Or Math.Abs(mp1.Tq - Tq) < TqStepTol Then
						'Point is above (or at) full load => interpolate & cut

						If i < 1 Then
							WorkerMsg(tMsgID.Err,
									  "Extrapolation not possible: Only one map point below R85 full load at " & rl.TargetRPM & " [1/min] !")
							Return False
						End If

						'Interpolate FC
						mp0 = rl.MapPoints(i - 1)
						FC = (Tq - mp0.Tq) * (mp1.FC - mp0.FC) / (mp1.Tq - mp0.Tq) + mp0.FC

						mp1.MarkedForDeath = True

						PointFoundAbove = True

					End If

				End If

			Next

			'Remove marked points
			'GS 23.09.2015: do not cut map
			'rl.MapPoints.RemoveAll(Function(mp) mp.MarkedForDeath)


			If PointFoundAbove Then

				'Add interpolated point at full load
				'GS 23.09.2015: do not cut map
				'rl.MapPoints.Add(New cMapPoint(rl.TargetRPM, Tq, FC))

			Else

				'No points above full load => extrapolate
				'Maximum torque deviation for extrapolation defined as half of maximum torque stepwidth for FC map
				TqDistMax = FLC_Parent.TqMax / 20
				'If Tq > 1.05 * rl.MapPoints.Last.Tq Then
				If Math.Abs(Tq - rl.MapPoints.Last.Tq) > TqDistMax Then
					WorkerMsg(tMsgID.Err,
							  "Extrapolation not allowed: R85 torque exceeds Fuel Map max torque at " & rl.TargetRPM &
							  " [1/min] by more than defined limit!")
					Return False
				End If

				'Abort if not enough points below R85
				If rl.MapPoints.Count < 2 Then
					WorkerMsg(tMsgID.Err, "Extrapolation not possible: Not enough map points at " & rl.TargetRPM & " [1/min] !")
					Return False
				End If

				'Linear regression of two or three points
				lX.Clear()
				lY.Clear()
				LinReg = New cRegression

				For i = Math.Max(0, rl.MapPoints.Count - 3) To rl.MapPoints.Count - 1
					lX.Add(rl.MapPoints(i).Tq)
					lY.Add(rl.MapPoints(i).FC)
				Next

				LinRegResult = LinReg.Regress(lX.ToArray, lY.ToArray)

				'Add extrapolated point at full load
				rl.MapPoints.Add(New cMapPoint(rl.TargetRPM, Tq, LinRegResult.a + LinRegResult.b * Tq))

			End If


			'Remove marked points with MarkedForDeathAfterR85Cut
			'rl.MapPoints.RemoveAll(Function(mp) mp.MarkedForDeathAfterR85Cut)


		Next

		Return True
	End Function


	Public Function AddDrag() As Boolean
		Dim rl As cRPMlist
		'Dim lowestRpm As cRPMlist = RPMlists(RPMlists.Keys.First)
		'Dim highestRpm As cRPMlist = RPMlists(RPMlists.Keys.Last)
		Dim lTargetRpms As List(Of Double) = RPMlists.Keys.ToList()
		Dim i As Integer
		Dim CountNumVal As Integer
		Dim Tq_forLoop As Double
		Dim dnUleft_forLoop As Double
		Dim dnUright_forLoop As Double
		Dim IndexTargetRpms As Integer


		'First run: only fill regular map points (except first and last)
		'start with wirst entry is necessary (is anyway ignored by if statements below) in order to match "rl" with "IndexTargetRpms"
		IndexTargetRpms = 0

		For Each rl In RPMlists.Values

			If IndexTargetRpms > 1 AndAlso IndexTargetRpms < (lTargetRpms.Count - 2) Then

				dnUleft_forLoop = lTargetRpms(IndexTargetRpms) - lTargetRpms(IndexTargetRpms - 1)
				dnUright_forLoop = lTargetRpms(IndexTargetRpms + 1) - lTargetRpms(IndexTargetRpms)
				Tq_forLoop = 0
				CountNumVal = 0

				For i = 0 To Motoring.iDim
					If _
						Motoring.LnU(i) >= (lTargetRpms(IndexTargetRpms) - dnUleft_forLoop / 2) AndAlso
						Motoring.LnU(i) < (lTargetRpms(IndexTargetRpms) + dnUright_forLoop / 2) Then
						Tq_forLoop += Motoring.LTq(i)
						CountNumVal += 1
					ElseIf Motoring.LnU(i) > (lTargetRpms(IndexTargetRpms) + dnUright_forLoop / 2) Then
						Exit For
					End If
				Next

				'Add motoring curve with zero FC
				rl.MapPoints.Insert(0, New cMapPoint(rl.TargetRPM, Tq_forLoop / CountNumVal, 0))
				'add additional point at (motoring torque -100Nm) with zero FC
				'rl.MapPoints.Insert(0, New cMapPoint(rl.TargetRPM, Motoring.Tq(rl.TargetRPM) - 100, 0))
				rl.MapPoints.Insert(0, New cMapPoint(rl.TargetRPM, (Tq_forLoop / CountNumVal) - 100, 0))

			ElseIf IndexTargetRpms = 1 Then
				'idle speed point (only consider values >= idle speed)

				dnUleft_forLoop = 0
				dnUright_forLoop = lTargetRpms(IndexTargetRpms + 1) - lTargetRpms(IndexTargetRpms)
				Tq_forLoop = 0
				CountNumVal = 0

				For i = 0 To Motoring.iDim
					If _
						Motoring.LnU(i) >= (lTargetRpms(IndexTargetRpms) - dnUleft_forLoop / 2) AndAlso
						Motoring.LnU(i) < (lTargetRpms(IndexTargetRpms) + dnUright_forLoop / 2) Then
						Tq_forLoop += Motoring.LTq(i)
						CountNumVal += 1
					ElseIf Motoring.LnU(i) > (lTargetRpms(IndexTargetRpms) + dnUright_forLoop / 2) Then
						Exit For
					End If
				Next

				'Add motoring curve with zero FC
				rl.MapPoints.Insert(0, New cMapPoint(rl.TargetRPM, Tq_forLoop / CountNumVal, 0))
				'add additional point at (motoring torque -100Nm) with zero FC
				rl.MapPoints.Insert(0, New cMapPoint(rl.TargetRPM, (Tq_forLoop / CountNumVal) - 100, 0))


			ElseIf IndexTargetRpms = lTargetRpms.Count - 2 Then
				'n_95h speed point (only consider values <= n_95h speed)

				dnUleft_forLoop = lTargetRpms(IndexTargetRpms) - lTargetRpms(IndexTargetRpms - 1)
				dnUright_forLoop = 0
				Tq_forLoop = 0
				CountNumVal = 0

				For i = 0 To Motoring.iDim
					If _
						Motoring.LnU(i) >= (lTargetRpms(IndexTargetRpms) - dnUleft_forLoop / 2) AndAlso
						Motoring.LnU(i) <= (lTargetRpms(IndexTargetRpms) + dnUright_forLoop / 2) Then
						Tq_forLoop += Motoring.LTq(i)
						CountNumVal += 1
					ElseIf Motoring.LnU(i) > (lTargetRpms(IndexTargetRpms) + dnUright_forLoop / 2) Then
						Exit For
					End If
				Next

				'Add motoring curve with zero FC
				rl.MapPoints.Insert(0, New cMapPoint(rl.TargetRPM, Tq_forLoop / CountNumVal, 0))
				'add additional point at (motoring torque -100Nm) with zero FC
				rl.MapPoints.Insert(0, New cMapPoint(rl.TargetRPM, (Tq_forLoop / CountNumVal) - 100, 0))

			End If

			IndexTargetRpms += 1
		Next


		'Second run to add first (100rpm below idle point) and last (500rpm above n_95h point) value by copying of nearest neighbours

		'Special for first entry (100rpm below idle point)
		'use same motoring torque as for idle speed entry
		'Add motoring curve with zero FC
		rl = RPMlists(lTargetRpms(0))
		rl.MapPoints.Insert(0, New cMapPoint(rl.TargetRPM, RPMlists(lTargetRpms(1)).MapPoints(1).Tq, 0))
		rl.MapPoints.Insert(0, New cMapPoint(rl.TargetRPM, RPMlists(lTargetRpms(1)).MapPoints.First.Tq, 0))


		'Special for last entry (500rpm above n_95h point)
		'use same motoring torque as for n_95h speed entry
		'Add motoring curve with zero FC
		rl = RPMlists(lTargetRpms(lTargetRpms.Count - 1))
		rl.MapPoints.Insert(0, New cMapPoint(rl.TargetRPM, RPMlists(lTargetRpms(lTargetRpms.Count - 2)).MapPoints(1).Tq, 0))
		rl.MapPoints.Insert(0, New cMapPoint(rl.TargetRPM, RPMlists(lTargetRpms(lTargetRpms.Count - 2)).MapPoints.First.Tq, 0))


		Return True
	End Function


	Public Function WriteMap(path As String) As Boolean
		Dim file As New cFile_V3
		Dim mp As cMapPoint
		Dim rl As cRPMlist
		Dim FC_corr As Double

		If Not file.OpenWrite(path) Then
			WorkerMsg(tMsgID.Err, "Failed to write file (" & path & ") !")
			Return False
		End If

		file.WriteLine("engine speed [rpm], torque [Nm], fuel consumption [g/h]")

		For Each rl In RPMlists.Values
			For Each mp In rl.MapPoints

				'correction of measured FC to standard NCV (not performed for Diesel reference fuel B7 coded "Diesel / CI")
				If temp_Map_FuelType = "Diesel / CI" Then
					FC_corr = mp.FC
				Else
					FC_corr = mp.FC * NCV_CorrectionFactor
				End If

				'writing of output
				file.WriteLine(Math.Round(mp.nU, 2), Math.Round(mp.Tq, 2), Math.Round(FC_corr, 2))
			Next
		Next


		file.Close()

		Return True
	End Function

	Public Function WriteFLD(path As String, AveragingOver8rpm As Boolean) As Boolean
		Dim file As New cFile_V3
		Dim nU As Double
		Dim Tq As Double
		Dim DragTq As Double
		Dim i As Integer

		' Write values in file with 8 rpm steps

		If AveragingOver8rpm Then
			'Calc 8 rpm steps
			CalcFlcOut()


			If Not file.OpenWrite(path) Then
				WorkerMsg(tMsgID.Err, "Failed to write file (" & path & ") !")
				Return False
			End If

			'Header
			file.WriteLine("engine speed [1/min], full load torque [Nm], motoring torque [Nm]")

			'Values
			For i = 0 To (LnU_out.Count - 1)

				nU = LnU_out(i)
				Tq = LTq_out(i)
				DragTq = LTqMot_out(i)

				file.WriteLine(Math.Round(nU, 2), Math.Round(Tq, 2), Math.Round(DragTq, 2))
			Next

			file.Close()

			Return True

		Else

			If Not file.OpenWrite(path) Then
				WorkerMsg(tMsgID.Err, "Failed to write file (" & path & ") !")
				Return False
			End If

			'Header
			file.WriteLine("engine speed [1/min], full load torque [Nm], motoring torque [Nm]")

			'Values
			For i = 0 To (FLC_Parent.LnU.Count - 1)

				nU = FLC_Parent.LnU(i)
				Tq = FLC_Parent.LTq(i)
				DragTq = Motoring.Tq(nU)

				file.WriteLine(Math.Round(nU, 2), Math.Round(Tq, 2), Math.Round(DragTq, 2))
			Next

			file.Close()

			Return True

		End If
	End Function

	Public Function WriteXmlComponentFile(filename As String, model As String, job As cJob) As Boolean

		Dim SchemaLocationBaseUrl = "https://webgate.ec.europa.eu/CITnet/svn/VECTO/trunk/Share/XML/XSD/"
		Dim SchemaVersion = "1.0"
		Dim tns As XNamespace = "urn:tugraz:ivt:VectoAPI:DeclarationDefinitions:v1.0"
		Dim rootNamespace As XNamespace = "urn:tugraz:ivt:VectoAPI:DeclarationComponent:v1.0"

		Dim xsi As XNamespace = XNamespace.Get("http://www.w3.org/2001/XMLSchema-instance")
		Dim xsd As XNamespace = XNamespace.Get("http://www.w3.org/2001/XMLSchema")

		Dim report = New XDocument(New XDeclaration("1.0", "utf-8", "yes"))
		'report.Add()

		Dim fcMap = New XElement(tns + "FuelConsumptionMap")

		Dim FC_corr As Double

		'FC map
		For Each rl In RPMlists.Values
			For Each mp In rl.MapPoints
				'file.WriteLine(mp.nU, mp.Tq, mp.FC)
				If job.FuelType = "Diesel / CI" Then
					FC_corr = mp.FC
				Else
					FC_corr = mp.FC * NCV_CorrectionFactor
				End If

				fcMap.Add(New XElement(tns + "Entry",
									   New XAttribute("engineSpeed", mp.nU.ToString("f2")),
									   New XAttribute("torque", mp.Tq.ToString("f2")),
									   New XAttribute("fuelConsumption", FC_corr.ToString("f2"))))
			Next
		Next


		'Full load and motoring curve
		Dim fldCurve = New XElement(tns + "FullLoadAndDragCurve")


		Dim nU As Double
		Dim Tq As Double
		Dim DragTq As Double
		Dim i As Integer

		' Write values in file with 8 rpm steps
		'Calc 8 rpm steps
		CalcFlcOut()
		'Values
		For i = 0 To (LnU_out.Count - 1)
			nU = LnU_out(i)
			Tq = LTq_out(i)
			DragTq = LTqMot_out(i)

			fldCurve.Add(New XElement(tns + "Entry",
									  New XAttribute("engineSpeed", nU.ToString("f2")),
									  New XAttribute("maxTorque", Tq.ToString("f2")),
									  New XAttribute("dragTorque", DragTq.ToString("f2"))))
		Next

		report.Add(New XElement(rootNamespace + "VectoInputDeclaration",
								New XAttribute("schemaVersion", SchemaVersion),
								New XAttribute(XNamespace.Xmlns + "xsi", xsi.NamespaceName),
								New XAttribute("xmlns", tns),
								New XAttribute(XNamespace.Xmlns + "tns", rootNamespace),
								New XAttribute(xsi + "schemaLocation",
											   String.Format("{0} {1}VectoComponent.xsd", rootNamespace, SchemaLocationBaseUrl)),
								New XElement(rootNamespace + "Engine",
											 New XElement(tns + "Data",
														  New XElement(tns + "Manufacturer", job.Manufacturer),
														  New XElement(tns + "Model", job.Model),
														  New XElement(tns + "CertificationNumber", job.CertNumber),
														  New XElement(tns + "Date",
																	   XmlConvert.ToString(DateTime.Now,
																						   XmlDateTimeSerializationMode.
																							  Utc)),
														  New XElement(tns + "AppVersion", VectoEngineVersion.FullVersion),
														  New XElement(tns + "Displacement", job.Displacement),
														  New XElement(tns + "IdlingSpeed", job.Idle),
														  New XElement(tns + "RatedSpeed", job.RatedSpeed),
														  New XElement(tns + "RatedPower", job.RatedPower * 1000),
														  New XElement(tns + "MaxEngineTorque", FLC.TqMax.ToString("F0")),
														  New XElement(tns + "WHTCUrban", job.WHTCurbanFactor.ToString("f4")),
														  New XElement(tns + "WHTCRural", job.WHTCruralFactor.ToString("f4")),
														  New XElement(tns + "WHTCMotorway",
																	   job.WHTCmotorwayFactor.ToString("f4")),
														  New XElement(tns + "BFColdHot",
																	   job.ColdHotBalancingFactor.ToString("f4")),
														  New XElement(tns + "CFRegPer", job.CF_RegPer.ToString("f4")),
														  New XElement(tns + "CFNCV", NCV_CorrectionFactor.ToString("F4")),
														  New XElement(tns + "FuelType", GetXMLFuelTypeString(job.FuelType)),
														  fcMap,
														  fldCurve
														  )
											 )
								)
				   )

		Try
			Dim stream = New MemoryStream()
			Dim writer = New StreamWriter(stream)
			writer.Write(report)
			writer.Flush()
			stream.Seek(0, SeekOrigin.Begin)
			Dim h As VectoHash = VectoHash.Load(stream)
			Dim finalReport = h.AddHash()
			finalReport.Validate(GetXMLSchema(), New ValidationEventHandler(AddressOf ValidationCallBack))

			finalReport.Save(filename)
		Catch e As Exception
			WorkerMsg(tMsgID.Err, "Generated XML is not valid - no output!")
			'MsgBox("Failed to generate XML: " + e.Message)
			Return False
		End Try
		Return True
	End Function


	Private Shared Sub ValidationCallBack(sender As Object, args As ValidationEventArgs)

		If (args.Severity = XmlSeverityType.Error) Then
			Throw New Exception(String.Format("Validation error: {0}" + Environment.NewLine +
											  "Line: {1}", args.Message, args.Exception.LineNumber))
		End If
	End Sub

	Private Shared Function GetXMLSchema() As XmlSchemaSet
		Dim resource As Stream = LoadResourceAsStream(ResourceType.XMLSchema,
													  "VectoComponent.xsd")
		Dim xset = New XmlSchemaSet()
		xset.XmlResolver = New XmlResourceResolver()
		Dim reader As XmlReader = XmlReader.Create(resource, New XmlReaderSettings(), "schema://")
		xset.Add(XmlSchema.Read(reader, Nothing))
		xset.Compile()
		Return xset
	End Function

	Private Function GetXMLFuelTypeString(fuelType As String) As String
		' input: "Diesel / CI", "Ethanol / CI", "Petrol / PI", "Ethanol / PI", "LPG / PI", "Natural Gas / PI"
		' output:  "Diesel CI", "Ethanol CI", "Petrol PI", "Ethanol PI", "LPG", "NG"
		Select Case (fuelType)
			Case "Diesel / CI", "Ethanol / CI", "Petrol / PI", "Ethanol / PI"
				Return fuelType.Replace("/ ", "")
			Case "LPG / PI"
				Return "LPG"
			Case "Natural Gas / PI"
				Return "NG"
		End Select
		Throw New Exception("Unknonw Fuel Type: " + fuelType)
	End Function

	Public Function fFCdelaunay_Intp(nU As Double, Tq As Double) As Double
		Dim val As Double

		val = FuelMap.Intpol(nU, Tq)

		If FuelMap.ExtrapolError Then
			WorkerMsg(tMsgID.Err,
					  "Cannot extrapolate FC map! n= " & nU.ToString("0.00") & " [1/min], Tq= " & Tq.ToString("0.00") & " [Nm]")
			Return -10000
		Else
			Return val
		End If
	End Function

	Public Function TqMax(nU As Double) As Double
		Dim rpm As Double
		Dim rpm0 As Double

		rpm = RPMlists.Keys(1)
		rpm0 = RPMlists.Keys(0)

		'Extrapolation for x < x(1)
		If RPMlists.First.Value.TargetRPM >= nU Then
			GoTo lbInt
		End If

		For Each rl As KeyValuePair(Of Double, cRPMlist) In RPMlists
			rpm0 = rpm
			rpm = rl.Key
			If rpm >= nU Then Exit For
		Next


lbInt:
		'Interpolation
		Return _
			(nU - rpm0) * (RPMlists(rpm).MapPoints.Last.Tq - RPMlists(rpm0).MapPoints.Last.Tq) / (rpm - rpm0) +
			RPMlists(rpm0).MapPoints.Last.Tq
	End Function

	Public Function TqMin(nU As Double) As Double
		Dim rpm As Double
		Dim rpm0 As Double

		rpm = RPMlists.Keys(1)
		rpm0 = RPMlists.Keys(0)

		'Extrapolation for x < x(1)
		If RPMlists.First.Value.TargetRPM >= nU Then
			GoTo lbInt
		End If

		For Each rl As KeyValuePair(Of Double, cRPMlist) In RPMlists
			rpm0 = rpm
			rpm = rl.Key
			If rpm >= nU Then Exit For
		Next


lbInt:
		'Interpolation
		'  !!!!!  ATTENTION: use second point from motoring curve, since first point is located 100 Nm below real motoring curve
		Return _
			(nU - rpm0) * (RPMlists(rpm).MapPoints(1).Tq - RPMlists(rpm0).MapPoints(1).Tq) / (rpm - rpm0) +
			RPMlists(rpm0).MapPoints(1).Tq

		' OLD VERSION, using first point which is wrong
		'(nU - rpm0) * (RPMlists(rpm).MapPoints.First.Tq - RPMlists(rpm0).MapPoints.First.Tq) / (rpm - rpm0) +
		'RPMlists(rpm0).MapPoints.First.Tq()
	End Function

	Private Sub CalcFlcOut()
		Dim nU As Double
		Dim Tq As Double
		Dim i As Integer
		Dim TqMaxMap As Double
		Dim nEng_forLoop As Double
		Dim CountNumVal As Integer
		Dim nURange_forLoop As Double


		LnU_outTemp.Clear()
		LTq_outTemp.Clear()


		If FLC.delta_nU < 6 Then

			' ########################################################################
			' Start: Transform full load curve (averaging over +/-4rpm in 8 rpm steps)
			' ########################################################################


			'keep first entry (might be exactly idle speed)
			LnU_outTemp.Add(FLC.LnU(0) - 0.01)
			LTq_outTemp.Add(FLC.LTq(0))


			nURange_forLoop = 0.5 * 8
			nEng_forLoop = FLC.LnU(0)

			'loop starting from (lowest speed + nURange_forLoop) in dataset
			Do While (nEng_forLoop + nURange_forLoop) < FLC.LnU(FLC.iDim)

				nURange_forLoop = 0.5 * 8

				Do
					CountNumVal = 0
					nU = 0
					Tq = 0

					For i = 0 To FLC.iDim
						If (FLC.LnU(i) > (nEng_forLoop - nURange_forLoop)) And (FLC.LnU(i) <= (nEng_forLoop + nURange_forLoop)) Then
							nU += FLC.LnU(i)
							Tq += FLC.LTq(i)
							CountNumVal += 1
						End If
					Next

					nURange_forLoop = nURange_forLoop * 1.05
				Loop Until CountNumVal > 0

				LnU_outTemp.Add(nU / CountNumVal)
				LTq_outTemp.Add(Tq / CountNumVal)

				nEng_forLoop += 8
			Loop


			'last entry (might be shorter interval)
			nURange_forLoop = 0.5 * 8

			Do
				CountNumVal = 0
				nU = 0
				Tq = 0

				For i = 0 To FLC.iDim
					If FLC.LnU(i) >= (nEng_forLoop - nURange_forLoop) Then
						nU += FLC.LnU(i)
						Tq += FLC.LTq(i)
						CountNumVal += 1
					End If
				Next

				nURange_forLoop = nURange_forLoop * 1.05
			Loop Until CountNumVal > 0

			LnU_outTemp.Add(nU / CountNumVal)
			LTq_outTemp.Add(Tq / CountNumVal)
			'END last entry (might be shorter interval)

			'keep last entry (might be exactly n_95h speed)
			LnU_outTemp.Add(FLC.LnU(FLC.iDim) + 0.01)
			LTq_outTemp.Add(FLC.LTq(FLC.iDim))

			' ######################################################################
			' End: Transform full load curve (averaging over +/-4rpm in 8 rpm steps)
			' ######################################################################


			LnU_out.Clear()
			LTq_out.Clear()
			LTqMot_out.Clear()

			'nEng_forLoop = LnU_outTemp(0)
			nEng_forLoop = FLC.n_idle - 8

			Do Until nEng_forLoop > LnU_outTemp(LnU_outTemp.Count - 1)
				'torque interpolated
				Tq = TqMax_FLC_cMAP(nEng_forLoop)
				TqMaxMap = TqMax(nEng_forLoop)

				'Check if full load torque is above max torque from map
				If Tq > TqMaxMap Then
					WorkerMsg(tMsgID.Warn,
							  "   Actual engine Full Load torque exceeds Fuel Map max torque at " & Math.Round(nU, 2) &
							  " [1/min] by " & Math.Round(Tq - TqMaxMap, 2) & " [Nm] !")
					WorkerMsg(tMsgID.Warn,
							  "     >>> Maximum torque at " & Math.Round(nU, 2) & " [1/min] will be limited to " &
							  Math.Round(TqMaxMap, 2) & " [Nm] in full load output file!")
					Tq = TqMaxMap
				End If

				LnU_out.Add(nEng_forLoop)
				LTq_out.Add(Tq)
				LTqMot_out.Add(Motoring.Tq(nEng_forLoop))

				nEng_forLoop += 8
			Loop

			If LnU_out(LnU_out.Count - 1) < FLC.n_hi Then
				'add last value (exact value might be necessary for calculation of n_hi in VECTO)
				LnU_out.Add(FLC.n_hi)
				LTq_out.Add(FLC.Tq(FLC.n_hi))
				LTqMot_out.Add(Motoring.Tq(FLC.n_hi))
			End If

		Else

			'delta_nU >= 6 rpm

			LnU_out.Clear()
			LTq_out.Clear()
			LTqMot_out.Clear()

			nEng_forLoop = FLC.n_idle - 8

			Do Until nEng_forLoop > FLC.LnU(FLC.LnU.Count - 1)
				'torque interpolated
				Tq = FLC.Tq(nEng_forLoop)
				TqMaxMap = TqMax(nEng_forLoop)

				'Check if full load torque is above max torque from map
				If Tq > TqMaxMap Then
					WorkerMsg(tMsgID.Warn,
							  "   Actual engine Full Load torque exceeds Fuel Map max torque at " & Math.Round(nU, 2) &
							  " [1/min] by " & Math.Round(Tq - TqMaxMap, 2) & " [Nm] !")
					WorkerMsg(tMsgID.Warn,
							  "     >>> Maximum torque at " & Math.Round(nU, 2) & " [1/min] will be limited to " &
							  Math.Round(TqMaxMap, 2) & " [Nm] in full load output file!")
					Tq = TqMaxMap
				End If

				LnU_out.Add(nEng_forLoop)
				LTq_out.Add(Tq)
				LTqMot_out.Add(Motoring.Tq(nEng_forLoop))

				nEng_forLoop += 8
			Loop

			If LnU_out(LnU_out.Count - 1) < FLC.n_hi Then
				'add last value (exact value might be necessary for calculation of n_hi in VECTO)
				LnU_out.Add(FLC.n_hi)
				LTq_out.Add(FLC.Tq(FLC.n_hi))
				LTqMot_out.Add(Motoring.Tq(FLC.n_hi))
			End If


		End If
	End Sub

	Private Function TqMax_FLC_cMAP(nU As Double) As Double
		Dim i As Int32

		'Extrapolation for x < x(1)
		If LnU_outTemp(0) >= nU Then
			i = 1
			GoTo lbInt
		End If

		i = 0
		Do While LnU_outTemp(i) < nU And i < (LnU_outTemp.Count - 1)
			i += 1
		Loop


lbInt:
		'Interpolation
		Return _
			(nU - LnU_outTemp(i - 1)) * (LTq_outTemp(i) - LTq_outTemp(i - 1)) / (LnU_outTemp(i) - LnU_outTemp(i - 1)) +
			LTq_outTemp(i - 1)
	End Function

	Private Class cRPMlist
		Public ReadOnly TargetRPM As Double
		Public MapPoints As New List(Of cMapPoint)

		Public Sub New(rpm As Integer)
			TargetRPM = rpm
		End Sub
	End Class

	Private Class cMapPoint
		Public ReadOnly nU As Double
		Public ReadOnly Tq As Double
		Public ReadOnly FC As Double
		Public MarkedForDeath As Boolean
		Public MarkedForDeathAfterR85Cut As Boolean


		Public Sub New(nU0 As Double, Tq0 As Double, FC0 As Double)
			nU = nU0
			Tq = Tq0
			FC = FC0
			MarkedForDeath = False
			MarkedForDeathAfterR85Cut = False
		End Sub
	End Class
End Class