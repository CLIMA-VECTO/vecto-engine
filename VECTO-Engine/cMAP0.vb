Imports System.Security.Cryptography
Imports System.Text
Imports System.Xml

Public Class cMAP0
	Public FilePath As String

	Private ReadOnly MapPoints As New List(Of cMapPoint)
	Private iMapDim As Integer

	Private ReadOnly RPMlists As New SortedDictionary(Of Integer, cRPMlist)
	Public FLC_Parent As cFLD0
	Public FLC_highest As cFLD0
	Public Motoring As cFLD0

	Public Map_n_idle As Single
	Public Map_n_lo As Single
	Public Map_n_pref As Single
	Public Map_n_95h As Single
	Public Map_n_hi As Single

	Private TqStep As Single

	Private FuelMap As cDelaunayMap

	Public Function ReadFile() As Boolean
		Dim file As cFile_V3
		Dim line As String()
		Dim nU As Single
		Dim Tq As Single
		Dim FC As Single

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
					GoTo lbEr
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
		Dim nU_Single As Single
		Dim nUStep As Single
		Dim TqStepMax As Single
		Dim IntervalCount As Integer
		Dim mp0 As cMapPoint
		Dim Matched As Boolean
		Dim TargetRPMs As New List(Of Single)
		Dim rl As cRPMlist
		Dim TqList As New List(Of Single)
		Dim Tq As Single
		Dim TqTarget As Single
		Dim MapSpeed_nA As Single
		Dim MapSpeed_nB As Single
		Dim MapSpeed_n57 As Single

		'Define target rpms for FC Map
		TargetRPMs.Clear()

		MapSpeed_n57 = 0.565 * (0.45 * Map_n_lo + 0.45 * Map_n_pref + 0.1 * Map_n_hi - Map_n_idle) * 2.0327 + Map_n_idle
		MapSpeed_nA = MapSpeed_n57 - 0.05 * (Map_n_95h - Map_n_idle)
		MapSpeed_nB = MapSpeed_n57 + 0.08 * (Map_n_95h - Map_n_idle)

		TargetRPMs.Add(Math.Round(Map_n_idle, 1))
		TargetRPMs.Add(Math.Round(MapSpeed_nA, 1))
		TargetRPMs.Add(Math.Round(MapSpeed_nB, 1))
		TargetRPMs.Add(Math.Round(Map_n_95h, 1))

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

		If IntervalCount <> 4 Then
			WorkerMsg(tMsgID.Err, "Incorrect rpm intervals between n_idle and n_A (" & IntervalCount & " intervals)!")
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

		If IntervalCount <> 4 Then
			WorkerMsg(tMsgID.Err, "Incorrect rpm intervals between n_B and n_95h (" & IntervalCount & " intervals)!")
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

		For Each nU_Single In TargetRPMs
			RPMlists.Add(nU_Single, New cRPMlist(nU_Single))
		Next

		For Each mp0 In MapPoints

			Matched = False

			For Each nU_Single In TargetRPMs
				If Math.Abs(nU_Single - mp0.nU) <= nUStepTol Then
					RPMlists(nU_Single).MapPoints.Add(mp0)
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

		'Check of maximum torque interval allowed (over each map point except highest)
		For Each rl In RPMlists.Values
			If rl.MapPoints.Count > 2 Then

				For i = 1 To rl.MapPoints.Count - 2	'skip highest torque 

					TqStep = rl.MapPoints(i).Tq - rl.MapPoints(i - 1).Tq

					'Check step size
					' 2*Tolerance value (since first can be lower and second can be higher)
					If TqStep > TqStepMax + 2 * TqStepTol Then
						WorkerMsg(tMsgID.Err,
								  "Invalid torque interval in FC map at " & rl.TargetRPM & " [1/min] between " & rl.MapPoints(i - 1).Tq &
								  "[Nm] and " & rl.MapPoints(i).Tq & "[Nm]!")
						WorkerMsg(tMsgID.Err, "	 Calculated torque interval = " & TqStep & " [Nm].")
						WorkerMsg(tMsgID.Err,
								  "	 Maximum allowed torque interval = " & TqStepMax & " [Nm] + " & 2 * TqStepTol & "[Nm] tolerance.")
						Return False
					End If
				Next

			End If

		Next

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
				'Different handling of full-load points (because stationary torque in Map can be higher than R49)
				' Index start from 0, so (i-1) is the last value
				If i < (rl.MapPoints.Count - 1) Then
					'Regular points
					'TqTarget = Math.Min(TqList(i), R49.Tq(mp0.nU))
					TqTarget = TqList(i)
					If Math.Abs(mp0.Tq - TqTarget) > TqStepTol Then
						WorkerMsg(tMsgID.Err, "Invalid torque value in FC map! (" & mp0.nU & " [1/min], " & mp0.Tq & " [Nm])")
						WorkerMsg(tMsgID.Err, "	 Expected torque = " & TqTarget & " [Nm].")
						WorkerMsg(tMsgID.Err, "	 Allowed tolerance is +/- " & TqStepTol & " [Nm].")
						Return False
					End If
				Else
					'Full load points
					TqTarget = FLC_Parent.Tq(mp0.nU)
					If (mp0.Tq - TqTarget) < -TqStepTol OrElse (mp0.Tq - TqTarget) > TqStepMax / 2 Then
						WorkerMsg(tMsgID.Err, "Invalid torque value in FC map! (" & mp0.nU & " [1/min], " & mp0.Tq & " [Nm])")
						WorkerMsg(tMsgID.Err, "	 Expected torque = " & TqTarget & " [Nm].")
						WorkerMsg(tMsgID.Err, "	 Allowed tolerance at full load is -" & TqStepTol & " [Nm] and +" & TqStepMax / 2 & " [Nm].")
						Return False
					End If
				End If
			Next

		Next

		'Copy first rpmlist to idle -100
		rl = New cRPMlist(Map_n_idle - 100)
		For Each mp0 In RPMlists(Map_n_idle).MapPoints
			rl.MapPoints.Add(New cMapPoint(Map_n_idle - 100, mp0.Tq, mp0.FC))
		Next

		RPMlists.Add(Map_n_idle - 100, rl)

		'Copy last rpmlist to last rpm +100
		rl = New cRPMlist(TargetRPMs.Last + 100)
		For Each mp0 In RPMlists(TargetRPMs.Last).MapPoints
			rl.MapPoints.Add(New cMapPoint(TargetRPMs.Last + 100, mp0.Tq, mp0.FC))
		Next

		RPMlists.Add(TargetRPMs.Last + 100, rl)


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
		Dim Tq As Single
		Dim i As Integer
		Dim TqDistMax As Single


		For Each rl In RPMlists.Values

			'Maximum torque distance for extrapolation defined as exactly one torque stepwidth for FC map
			TqDistMax = FLC_Parent.TqMax / 10
			Tq = rl.MapPoints.Last.Tq + TqDistMax

			'Abort if not enough points below R49 Parent full load
			If rl.MapPoints.Count < 3 Then
				WorkerMsg(tMsgID.Err, "Extrapolation not possible: Less than 3 points in Fuel Map at " & rl.TargetRPM & " [1/min] !")
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

			'Add extrapolated point at full load
			rl.MapPoints.Add(New cMapPoint(rl.TargetRPM, Tq, LinRegResult.a + LinRegResult.b * Tq))

		Next

		Return True
	End Function

	Public Function LimitR49toMap() As Boolean
		Dim i As Integer
		Dim nU As Single
		Dim Tq As Single
		Dim TqMaxMap As Single

		For i = 0 To FLC_Parent.iDim
			nU = FLC_Parent.LnU(i)
			Tq = FLC_Parent.LTq(i)
			TqMaxMap = TqMax(nU)
			If Tq > TqMaxMap Then
				FLC_Parent.LTq(i) = TqMaxMap
				WorkerMsg(tMsgID.Warn,
						  "   Parent R49 Full Load torque exceeds Fuel Map max torque at " & Math.Round(nU, 2) & " [1/min] by " &
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
		Dim nU As Single
		Dim Tq As Single
		Dim TqMinMap As Single

		For i = 0 To Motoring.iDim
			nU = Motoring.LnU(i)
			Tq = Motoring.LTq(i)
			TqMinMap = TqMin(nU)
			If Tq < TqMinMap Then
				Motoring.LTq(i) = TqMinMap
			End If
		Next

		Return True
	End Function

	Public Function AddFld() As Boolean
		Dim mp0 As cMapPoint
		Dim mp1 As cMapPoint
		Dim FC As Single
		Dim LinReg As cRegression
		Dim LinRegResult As cRegression.RegressionProcessInfo
		Dim lX As New List(Of Double)
		Dim lY As New List(Of Double)
		Dim Tq As Single
		Dim TqR85 As Single
		Dim TqMaxMap As Single
		Dim PointFoundAbove As Boolean
		Dim i As Integer
		Dim nUInt As Integer
		Dim nU As Single
		Dim rl0 As cRPMlist
		Dim Cancel As Boolean
		Dim TqDistMax As Single
		Dim rl1 As cRPMlist


		'Add map points at R85 rpm steps if over Map torque
		For i = 0 To FLC_highest.iDim

			nU = FLC_highest.LnU(i)
			nUInt = CInt(nU)
			TqR85 = FLC_highest.LTq(i)
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

				'Add drag curve
				rl0.MapPoints.Insert(0, New cMapPoint(nU, Motoring.Tq(nU), 0))

				RPMlists.Add(nUInt, rl0)

			End If
		Next


		'Extrapolate/Interpolate FC values at R85 full load curve and cut above R85 full load
		For Each rl In RPMlists.Values

			'R85 full load at this rpm
			Tq = FLC_highest.Tq(rl.TargetRPM)
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

		'Extrapolate/Interpolate R85 full load curve and cut above full load
		For Each rl In RPMlists.Values

			'Add drag curve
			rl.MapPoints.Insert(0, New cMapPoint(rl.TargetRPM, Motoring.Tq(rl.TargetRPM), 0))

		Next

		Return True
	End Function


	Public Function WriteMap(path As String) As Boolean
		Dim file As New cFile_V3
		Dim mp As cMapPoint
		Dim rl As cRPMlist

		If Not file.OpenWrite(path) Then
			WorkerMsg(tMsgID.Err, "Failed to write file (" & path & ") !")
			Return False
		End If

		file.WriteLine("engine speed [rpm], torque [Nm], fuel consumption [g/h]")

		For Each rl In RPMlists.Values
			For Each mp In rl.MapPoints
				file.WriteLine(mp.nU, mp.Tq, mp.FC)
			Next
		Next


		file.Close()

		Return True
	End Function

	Public Function WriteFLD(path As String) As Boolean
		Dim file As New cFile_V3
		Dim nU As Single
		Dim Tq As Single
		Dim DragTq As Single
		Dim i As Integer
		Dim TqMaxMap As Single

		If Not file.OpenWrite(path) Then
			WorkerMsg(tMsgID.Err, "Failed to write file (" & path & ") !")
			Return False
		End If

		file.WriteLine("engine speed [1/min], full load torque [Nm], motoring torque [Nm]")

		'For Each nU In RPMlists.Keys
		'	Tq = R85.Tq(nU)
		'	DragTq = Drag.Tq(nU)
		'	file.WriteLine(nU, Tq, DragTq)
		'      Next

		For i = 0 To FLC_highest.iDim
			nU = FLC_highest.LnU(i)
			Tq = FLC_highest.LTq(i)
			DragTq = Motoring.Tq(nU)
			TqMaxMap = TqMax(nU)
			If Tq > TqMaxMap Then
				WorkerMsg(tMsgID.Warn,
						  "   Actual engine R49 Full Load torque exceeds Fuel Map max torque at " & Math.Round(nU, 2) &
						  " [1/min] by " & Math.Round(Tq - TqMaxMap, 2) & " [Nm] !")
				WorkerMsg(tMsgID.Warn,
						  "     >>> Maximum torque at " & Math.Round(nU, 2) & " [1/min] will be limited to " &
						  Math.Round(TqMaxMap, 2) & " [Nm] in full load output file!")
				Tq = TqMaxMap
			End If
			file.WriteLine(nU, Tq, DragTq)
		Next

		file.Close()

		Return True
	End Function

	Public Sub WriteXmlComponentFile(filename As String, model As String, job As cJob)

		Dim SchemaLocationBaseUrl = "http://www.ivt.tugraz.at/VECTO/"
		Dim SchemaVersion = "0.6"
		Dim tns As XNamespace = "urn:tugraz:ivt:VectoAPI:DeclarationDefinitions:v0.6"
		Dim rootNamespace As XNamespace = "urn:tugraz:ivt:VectoAPI:DeclarationComponent:v0.6"

		Dim xsi As XNamespace = XNamespace.Get("http://www.w3.org/2001/XMLSchema-instance")
		Dim xsd As XNamespace = XNamespace.Get("http://www.w3.org/2001/XMLSchema")

		Dim report = New XDocument(New XDeclaration("1.0", "utf-8", "yes"))
		'report.Add()

		Dim hasher = MD5Cng.Create()
		Dim hash As Byte() = hasher.ComputeHash(Encoding.UTF8.GetBytes(model))
		Dim hashString As String = Convert.ToBase64String(hash)

		Dim fcMap = New XElement(tns + "FuelConsumptionMap")
		For Each rl In RPMlists.Values
			For Each mp In rl.MapPoints
				'file.WriteLine(mp.nU, mp.Tq, mp.FC)
				fcMap.Add(New XElement(tns + "Entry",
									   New XAttribute("engineSpeed", mp.nU),
									   New XAttribute("torque", mp.Tq),
									   New XAttribute("fuelConsumption", mp.FC)))
			Next
		Next
		Dim fldCurve = New XElement(tns + "FullLoadAndDragCurve")
		For i = 0 To FLC_highest.iDim
			Dim nU As Single = FLC_highest.LnU(i)
			Dim Tq As Single = FLC_highest.LTq(i)
			Dim DragTq As Single = Motoring.Tq(nU)
			Dim TqMaxMap As Single = TqMax(nU)
			If Tq > TqMaxMap Then
				Tq = TqMaxMap
			End If
			'file.WriteLine(nU, Tq, DragTq)
			fldCurve.Add(New XElement(tns + "Entry",
									  New XAttribute("engineSpeed", nU),
									  New XAttribute("maxTorque", Tq),
									  New XAttribute("dragTorque", DragTq)))
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
														  New XAttribute("id", hashString),
														  New XElement(tns + "Vendor", job.Manufacturer.PadRight(5)),
														  New XElement(tns + "Creator", "Not Available"),
														  New XElement(tns + "Date",
																	   XmlConvert.ToString(DateTime.Now,
																						   XmlDateTimeSerializationMode.
																							  Utc)),
														  New XElement(tns + "MakeAndModel", job.Make.PadRight(10)),
														  New XElement(tns + "TypeId", job.TypeID.PadRight(10)),
														  New XElement(tns + "AppVersion", AppName & " " & AppVersionForm),
														  New XElement(tns + "Displacement", job.Displacement),
														  New XElement(tns + "IdlingSpeed", job.Idle),
														  New XElement(tns + "WHTCUrban", job.WHTCurbanFactor),
														  New XElement(tns + "WHTCRural", job.WHTCruralFactor),
														  New XElement(tns + "WHTCMotorway", job.WHTCmotorwayFactor),
														  New XElement(tns + "ColdHotBalancingFactor",
																	   job.ColdHotBalancingFactor),
														  fcMap,
														  fldCurve
														  )
											 )
								)
				   )
		report.Save(filename)
	End Sub

	Public Function fFCdelaunay_Intp(nU As Single, Tq As Single) As Single
		Dim val As Single

		val = FuelMap.Intpol(nU, Tq)

		If FuelMap.ExtrapolError Then
			WorkerMsg(tMsgID.Err,
					  "Cannot extrapolate FC map! n= " & nU.ToString("0.00") & " [1/min], Tq= " & Tq.ToString("0.00") & " [Nm]")
			Return -10000
		Else
			Return val
		End If
	End Function

	Public Function TqMax(nU As Single) As Single
		Dim rpm As Integer
		Dim rpm0 As Integer

		rpm = RPMlists.Keys(1)
		rpm0 = RPMlists.Keys(0)

		'Extrapolation for x < x(1)
		If RPMlists.First.Value.TargetRPM >= nU Then
			GoTo lbInt
		End If

		For Each rl As KeyValuePair(Of Integer, cRPMlist) In RPMlists
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

	Public Function TqMin(nU As Single) As Single
		Dim rpm As Integer
		Dim rpm0 As Integer

		rpm = RPMlists.Keys(1)
		rpm0 = RPMlists.Keys(0)

		'Extrapolation for x < x(1)
		If RPMlists.First.Value.TargetRPM >= nU Then
			GoTo lbInt
		End If

		For Each rl As KeyValuePair(Of Integer, cRPMlist) In RPMlists
			rpm0 = rpm
			rpm = rl.Key
			If rpm >= nU Then Exit For
		Next


lbInt:
		'Interpolation
		Return _
			(nU - rpm0) * (RPMlists(rpm).MapPoints.First.Tq - RPMlists(rpm0).MapPoints.First.Tq) / (rpm - rpm0) +
			RPMlists(rpm0).MapPoints.First.Tq
	End Function

	Private Class cRPMlist
		Public ReadOnly TargetRPM As Integer
		Public MapPoints As New List(Of cMapPoint)

		Public Sub New(rpm As Integer)
			TargetRPM = rpm
		End Sub
	End Class

	Private Class cMapPoint
		Public ReadOnly nU As Single
		Public ReadOnly Tq As Single
		Public ReadOnly FC As Single
		Public MarkedForDeath As Boolean
		Public MarkedForDeathAfterR85Cut As Boolean


		Public Sub New(nU0 As Single, Tq0 As Single, FC0 As Single)
			nU = nU0
			Tq = Tq0
			FC = FC0
			MarkedForDeath = False
			MarkedForDeathAfterR85Cut = False
		End Sub
	End Class
End Class