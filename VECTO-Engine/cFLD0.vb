Public Class cFLD0

	Public FilePath As String
	Public LnU As New List(Of Single)
	Public LTq As New List(Of Single)
	Public iDim As Integer

	Public TqMax As Single
	Private Pmax As Single

	Public n_idle As Single
	Public n_lo As Single
	Public n_hi As Single
	Public n_pref As Single
    Public n_95h As Single
    Public n_57 As Single
    Public n_A As Single
    Public n_B As Single


	'Read, sort and check step size
	Public Function ReadFile(ByVal ChecknUSteps As Boolean) As Boolean
		Dim file As cFile_V3
		Dim line As String()
		Dim sortedlist As Object
		Dim dic As New Dictionary(Of Single, Single)

		'Reset
		LnU.Clear()
		LTq.Clear()
		iDim = -1

		'Stop if there's no file
		If FilePath = "" OrElse Not IO.File.Exists(FilePath) Then
			WorkerMsg(tMsgID.Err, "File '" & FilePath & "' not found!")
			Return False
		End If

		'Open file
		file = New cFile_V3
		If Not file.OpenRead(FilePath) Then
			WorkerMsg(tMsgID.Err, "Failed to open file (" & FilePath & ") !")
			file = Nothing
			Return False
		End If

		'Skip Header
		file.ReadLine()


		Try

			Do While Not file.EndOfFile

				'Read Line
				line = file.ReadLine

				If dic.ContainsKey(line(0)) Then
					WorkerMsg(tMsgID.Err, "Duplicate entries for " & line(0) & " [1/min]! (" & FilePath & ")")
					GoTo lbEr
				Else
					dic.Add(line(0), line(1))
				End If

				iDim += 1

			Loop

		Catch ex As Exception

			WorkerMsg(tMsgID.Err, "Error during file read! Line number: " & iDim + 1 & " (" & FilePath & ")")
			GoTo lbEr

		End Try

		'Close file
		file.Close()

		'Sort dictionary
		sortedlist = From item As KeyValuePair(Of Single, Single) In dic Order By item.Key

		For Each item As KeyValuePair(Of Single, Single) In sortedlist
			LnU.Add(item.Key)
			LTq.Add(item.Value)
		Next

		'Check steps
		If ChecknUSteps Then
			For i = 1 To iDim
                If LnU(i) - LnU(i - 1) > 9 Or LnU(i) - LnU(i - 1) < 7 Then
                    WorkerMsg(tMsgID.Err, "Engine speed steps in Full Load & Drag Curve file must be within 8+/-1 [1/min] !" & " (" & FilePath & ")")
                    Return False
                End If
			Next
		End If


		'Calc TqMax
		TqMax = LTq(0)
		For i = 1 To iDim
			If LTq(i) > TqMax Then
				TqMax = LTq(i)
			End If
		Next

		Return True



lbEr:
		file.Close()
		file = Nothing

		Return False

	End Function

	'Calc Pmax and WHTC rpms
    Public Function RpmCalc(ByVal InfoStringFLC As String)

        PmaxCalc()

        n_idle = LnU(0)

        n_lo = fnUofPfull(0.55 * Pmax, True)

        If n_lo < 0 Then
            WorkerMsg(tMsgID.Err, "Failed to calculate n_lo for " & InfoStringFLC & "!")
            Return False
        End If

        n_95h = fnUofPfull(0.95 * Pmax, False)

        If n_95h < 0 Then
            WorkerMsg(tMsgID.Err, "Failed to calculate n_95h for " & InfoStringFLC & "!")
            Return False
        End If

        n_pref = fNpref()

        If n_pref < 0 Then
            WorkerMsg(tMsgID.Err, "Failed to calculate n_pref for " & InfoStringFLC & "!")
            Return False
        End If

        n_hi = fnUofPfull(0.7 * Pmax, False)

        If n_hi < 0 Then
            WorkerMsg(tMsgID.Err, "Failed to calculate n_hi for " & InfoStringFLC & "!")
            Return False
        End If

        n_57 = 0.565 * (0.45 * n_lo + 0.45 * n_pref + 0.1 * n_hi - n_idle) * 2.0327 + n_idle
        n_A = n_57 - 0.05 * (n_95h - n_idle)
        n_B = n_57 + 0.08 * (n_95h - n_idle)

        Return True

    End Function

	Private Sub PmaxCalc()
		Dim nU As Single
		Dim nUmax As Single
		Dim nUrated As Single
		Dim dnU As Single
		Dim P As Single

		dnU = 1
		Pmax = 0
		nU = LnU(0)
		nUmax = LnU(iDim)
		nUrated = nU

		Do
			P = nTqtoPe(nU, Tq(nU))
			If P > Pmax Then
				Pmax = P
				nUrated = nU
			End If
			nU += dnU
		Loop Until nU > nUmax

	End Sub

	Private Function fnUofPfull(ByVal PeTarget As Single, ByVal FromLeft As Boolean) As Single
		Dim Pe As Single
		Dim LastPe As Single
		Dim nU As Single
		Dim nUmin As Single
		Dim nUmax As Single
		Dim nUtarget As Single
		Dim dnU As Single

        dnU = 0.1
		nUmin = LnU(0)
		nUmax = LnU(iDim)

		If FromLeft Then

			nU = nUmin
			LastPe = nTqtoPe(nU, Tq(nU))
			nUtarget = nU

			If LastPe > PeTarget Then Return -1

			Do
				Pe = nTqtoPe(nU, Tq(nU))

				If Pe > PeTarget Then
					If Math.Abs(LastPe - PeTarget) < Math.Abs(Pe - PeTarget) Then
						Return nU - dnU
					Else
						Return nU
					End If
				End If

				LastPe = Pe
				nU += dnU
			Loop Until nU > nUmax

		Else

			nU = nUmax
			LastPe = nTqtoPe(nU, Tq(nU))
			nUtarget = nU

			If LastPe > PeTarget Then Return -1

			Do
				Pe = nTqtoPe(nU, Tq(nU))

				If Pe > PeTarget Then
					If Math.Abs(LastPe - PeTarget) < Math.Abs(Pe - PeTarget) Then
						Return nU + dnU
					Else
						Return nU
					End If
				End If

				LastPe = Pe
				nU -= dnU
			Loop Until nU < nUmin

		End If

		Return nUtarget

	End Function

	Private Function Area(ByVal nFrom As Single, ByVal nTo As Single) As Single
		Dim A As Single
		Dim i As Integer


		A = 0
		For i = 1 To iDim

			If LnU(i - 1) >= nTo Then Exit For

			If LnU(i - 1) >= nFrom Then


				If LnU(i) <= nTo Then

					'Add full segment
					A += (LnU(i) - LnU(i - 1)) * (LTq(i) + LTq(i - 1)) / 2

				Else

					'Add segment till nTo
					A += (nTo - LnU(i - 1)) * (Tq(nTo) + LTq(i - 1)) / 2

				End If

			Else

				If LnU(i) > nFrom Then

					'Add segment starting from nFrom
					A += (LnU(i) - nFrom) * (LTq(i) + Tq(nFrom)) / 2

				End If

			End If

		Next

		Return A

	End Function


	Private Function fNpref() As Single
		Dim i As Integer
		Dim Amax As Single
		Dim N95h As Single
		Dim n As Single
		Dim T0 As Single
		Dim dn As Single
		Dim A As Single
		Dim k As Single


		dn = 0.001

		N95h = fnUofPfull(0.95 * Pmax, False)

		If N95h < 0 Then Return -1

		Amax = Area(n_idle, N95h)

		For i = 0 To iDim - 1

			If Area(n_idle, LnU(i + 1)) > 0.51 * Amax Then

				n = LnU(i)
				T0 = LTq(i)
				A = Area(n_idle, n)

				k = (LTq(i + 1) - LTq(i)) / (LnU(i + 1) - LnU(i))

				Do While A < 0.51 * Amax
					n += dn
					A += dn * (2 * T0 + k * dn) / 2
				Loop

				Exit For

			End If

		Next

		Return n

	End Function


	Public Function Tq(ByVal nU As Single) As Single
		Dim i As Int32

		'Extrapolation for x < x(1)
		If LnU(0) >= nU Then
			i = 1
			GoTo lbInt
		End If

		i = 0
		Do While LnU(i) < nU And i < iDim
			i += 1
		Loop


lbInt:
		'Interpolation
		Return (nU - LnU(i - 1)) * (LTq(i) - LTq(i - 1)) / (LnU(i) - LnU(i - 1)) + LTq(i - 1)
	End Function


End Class
