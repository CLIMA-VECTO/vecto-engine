Public Class cFLD0

	Public FilePath As String
    Public LnU As New List(Of Double)
    Public LTq As New List(Of Double)
    Public iDim As Integer
    

    Public TqMax As Double
    Private Pmax As Double

    Public n_idle As Double
    Public n_lo As Double
    Public n_hi As Double
    Public n_pref As Double
    Public n_95h As Double
    Public n_57 As Double
    Public n_A As Double
    Public n_B As Double

    Public IdleSpeedValueForCheck As Double
    Public delta_nU As Double


    'Read, sort and check step size
    Public Function ReadFile(ByVal ChecknUSteps As Boolean, ByVal typeFullLoad As Boolean) As Boolean
        Dim file As cFile_V3
        Dim line As String()
        'Dim sortedlist As IEnumerable(Of KeyValuePair(Of Double, Double))
        'Dim dic As New Dictionary(Of Double, Double)
        Dim StringForSplit As String
        Dim StringsAfterSplit As String()
        Dim nEng_forLoop As Double
        Dim CountNumVal As Integer
        Dim nU_forLoop As Double
        Dim Tq_forLoop As Double
        Dim nURange_forLoop As Double
        Dim LnU_temp As New List(Of Double)
        Dim LTq_temp As New List(Of Double)
        Dim iDim_temp As Integer
        Dim lowest_nU As Double
        Dim highest_nU As Double
        Dim LnU_DevAvgCheck As New List(Of Double)
        Dim LTq_DevAvgCheck As New List(Of Double)
        Dim Tq_AvgCheck As Double

        'Reset
        LnU.Clear()
        LTq.Clear()
        LnU_temp.Clear()
        LTq_temp.Clear()
        iDim_temp = -1
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

                'If dic.ContainsKey(line(0)) Then
                ' NOT NEEDED ANYMORE  --> averaging done further below
                'WorkerMsg(tMsgID.Err, "Duplicate entries for " & line(0) & " [1/min]! Line number: " & iDim_original + 1 & " (" & FilePath & ")")
                'GoTo lbEr
                'Else

                StringForSplit = line(0)
                If StringForSplit.Contains(".") Then
                    StringsAfterSplit = StringForSplit.Split(New String() {"."c}, StringSplitOptions.RemoveEmptyEntries)
                    If StringsAfterSplit(1).Length <> 2 Then
                        WorkerMsg(tMsgID.Err, "Value needs to have exactly 2 digits after the decimal point: " & StringForSplit & " [1/min]! Line number: " & iDim_temp + 1 & " (" & FilePath & ")")
                        GoTo lbEr
                    End If
                Else
                    WorkerMsg(tMsgID.Err, "Value does not contain a valid decimal separator: " & StringForSplit & " [1/min]! Line number: " & iDim_temp + 1 & " (" & FilePath & ")")
                    GoTo lbEr
                End If
                StringForSplit = line(1)
                If StringForSplit.Contains(".") Then
                    StringsAfterSplit = StringForSplit.Split(New String() {"."c}, StringSplitOptions.RemoveEmptyEntries)
                    If StringsAfterSplit(1).Length <> 2 Then
                        WorkerMsg(tMsgID.Err, "Value needs to have exactly 2 digits after the decimal point: " & StringForSplit & " [Nm]! Line number: " & iDim_temp + 1 & " (" & FilePath & ")")
                        GoTo lbEr
                    End If
                Else
                    WorkerMsg(tMsgID.Err, "Value does not contain a valid decimal separator: " & StringForSplit & " [Nm]! Line number: " & iDim_temp + 1 & " (" & FilePath & ")")
                    GoTo lbEr
                End If

                'dic.Add(line(0), line(1))

                'add values also to lists for later processing
                LnU_temp.Add(line(0))
                LTq_temp.Add(line(1))

                'End If

                iDim_temp += 1

            Loop

        Catch ex As Exception

            WorkerMsg(tMsgID.Err, "Error during file read! Line number: " & iDim_temp + 1 & " (" & FilePath & ")")
            GoTo lbEr

        End Try

        'Close file
        file.Close()




        '####################################
        ' START: PRE-PROCESSING OF INPUT DATA
        '####################################

        'identify lowest and highest engine speed in dataset
        lowest_nU = LnU_temp(0)
        highest_nU = LnU_temp(0)

        For i = 1 To iDim_temp
            If LnU_temp(i) < lowest_nU Then
                lowest_nU = LnU_temp(i)
            End If
            If LnU_temp(i) > highest_nU Then
                highest_nU = LnU_temp(i)
            End If
        Next

        'sort data by increasing engine speed (necessary for handling full load and motoring correctly later on)
        nEng_forLoop = lowest_nU
        Do Until nEng_forLoop > (highest_nU + 0.005)
            For i = 0 To iDim_temp
                If LnU_temp(i) > (nEng_forLoop - 0.001) And LnU_temp(i) < (nEng_forLoop + 0.001) Then
                    LnU.Add(LnU_temp(i))
                    LTq.Add(LTq_temp(i))
                End If
            Next
            nEng_forLoop += 0.01
        Loop

        LnU_temp.Clear()
        LTq_temp.Clear()
        LnU_temp.AddRange(LnU)
        LTq_temp.AddRange(LTq)
        'values are from now on stored in "temp" variables
        iDim_temp = LnU_temp.Count - 1
        LnU.Clear()
        LTq.Clear()


        'Check if negative torque values are existing in full load curve (only highest 5% of all points are allowed to have negative torque values)
        If typeFullLoad Then
            For i = 0 To iDim_temp
                If LTq_temp(i) < 0 And LnU_temp(i) < highest_nU * 0.95 Then
                    WorkerMsg(tMsgID.Err, "Negative torque values in full load curve at " & LnU_temp(i).ToString("f2") & " [1/min] !" & " (" & FilePath & ")")
                    Return False
                End If
            Next
        End If


        'Calculate average rpm stepsize over dataset
        delta_nU = 0
        CountNumVal = 0

        For i = 0 To (iDim_temp - 1)
            If LnU_temp(i + 1) > LnU_temp(i) Then
                delta_nU += LnU_temp(i + 1) - LnU_temp(i)
                CountNumVal += 1

                'Check stepsize
                If ChecknUSteps Then
                    If LnU_temp(i + 1) - LnU_temp(i) > 9 Then
                        WorkerMsg(tMsgID.Warn, "Possible data error in full load or motoring curve!" & " (" & FilePath & ")")
                        WorkerMsg(tMsgID.Warn, "     >>> Engine speed steps > 9 [1/min] occur!")
                        Return False
                    End If
                End If

            End If

        Next
        delta_nU = delta_nU / CountNumVal



        'Calculate average torque values from original dataset
        '  over intervals of +/-0.5*delta_nU with stepsize of delta_nU
        '     # avoids strange shapes of curve for short drops in engine speed during recording
        '     # avoids errors if duplicate entries exist
        '
        '     # Extend range for averaging for plus 0.1 percent, if no values are found within current range
        '

        nURange_forLoop = 0.5 * delta_nU

        'keep first entry (lowest engine speed)
        LnU.Add(LnU_temp(0) - 0.01)
        LTq.Add(LTq_temp(0))
        nEng_forLoop = lowest_nU

        'loop starting from lowest speed in dataset
        Do While (nEng_forLoop + nURange_forLoop) < highest_nU

            nURange_forLoop = 0.5 * delta_nU

            Do
                LnU_DevAvgCheck.Clear()
                LTq_DevAvgCheck.Clear()
                CountNumVal = 0
                nU_forLoop = 0
                Tq_forLoop = 0

                For i = 0 To iDim_temp
                    If (LnU_temp(i) > (nEng_forLoop - nURange_forLoop)) And (LnU_temp(i) <= (nEng_forLoop + nURange_forLoop)) Then
                        nU_forLoop += LnU_temp(i)
                        Tq_forLoop += LTq_temp(i)
                        CountNumVal += 1
                        LnU_DevAvgCheck.Add(LnU_temp(i))
                        LTq_DevAvgCheck.Add(LTq_temp(i))
                    End If
                Next

                nURange_forLoop += nURange_forLoop * 1.05
            Loop Until CountNumVal > 0

            '     # check also for deviations individual original values vs. average of more than 5% in torque ONLY FOR FULL LOAD CURVE
            '       --> indicates faulty input data (e.g. outside of defined mapping speed range for full load with negative torque values)
            If typeFullLoad Then
                Tq_AvgCheck = Tq_forLoop / CountNumVal
                For i = 0 To (LnU_DevAvgCheck.Count - 1)
                    If Math.Abs(LTq_DevAvgCheck(i) - Tq_AvgCheck) > 0.1 * Tq_AvgCheck Then
                        WorkerMsg(tMsgID.Warn, "Possible data error in engine full load or motoring curve!" & " (" & FilePath & ")")
                        WorkerMsg(tMsgID.Warn, "     >>> Single entries for engine torque in interval " & Math.Floor(Math.Abs(nEng_forLoop - nURange_forLoop) * 100) / 100 &
                                  "-" & Math.Ceiling(Math.Abs(nEng_forLoop + nURange_forLoop) * 100) / 100 & " [1/min] deviate by more than 10% from average value!")
                        'Return False
                    End If
                Next
            End If

            LnU.Add(nU_forLoop / CountNumVal)
            LTq.Add(Tq_forLoop / CountNumVal)

            nEng_forLoop += delta_nU
        Loop


        'last entry (might be shorter interval)
        nURange_forLoop = 0.5 * delta_nU

        Do
            LnU_DevAvgCheck.Clear()
            LTq_DevAvgCheck.Clear()
            CountNumVal = 0
            nU_forLoop = 0
            Tq_forLoop = 0

            For i = 0 To iDim_temp
                If LnU_temp(i) >= (nEng_forLoop - nURange_forLoop) Then
                    nU_forLoop += LnU_temp(i)
                    Tq_forLoop += LTq_temp(i)
                    CountNumVal += 1
                    LnU_DevAvgCheck.Add(LnU_temp(i))
                    LTq_DevAvgCheck.Add(LTq_temp(i))
                End If
            Next

            nURange_forLoop += nURange_forLoop * 1.05
        Loop Until CountNumVal > 0

        '     # check also for deviations individual original values vs. average of more than 10% in torque ONLY FOR FULL LOAD CURVE
        '       --> indicates faulty input data (e.g. outside of defined mapping speed range for full load with negative torque values)
        If typeFullLoad Then
            Tq_AvgCheck = Tq_forLoop / CountNumVal
            For i = 0 To (LnU_DevAvgCheck.Count - 1)
                If Math.Abs(LTq_DevAvgCheck(i) - Tq_AvgCheck) > 0.05 * Tq_AvgCheck Then
                    WorkerMsg(tMsgID.Warn, "Possible data error in engine full load curve!" & " (" & FilePath & ")")
                    WorkerMsg(tMsgID.Warn, "     >>> Single entries for engine torque in interval >= " & Math.Floor(Math.Abs(nEng_forLoop - nURange_forLoop) * 100) / 100 &
                              " [1/min] deviate by more than 10% from average value!")
                    'Return False
                End If
            Next
        End If

        LnU.Add(nU_forLoop / CountNumVal)
        LTq.Add(Tq_forLoop / CountNumVal)
        'END last entry (might be shorter interval)

        'keep last entry (highest engine speed)
        LnU.Add(LnU_temp(iDim_temp) + 0.01)
        LTq.Add(LTq_temp(iDim_temp))



        'values are from now on stored in "final" variables
        ' --> values are sorted by ascending engine speed
        LnU_temp.Clear()
        LTq_temp.Clear()

        'index of vector is starting with 0, thus number of entries minus 1
        iDim = LnU.Count - 1


        If typeFullLoad Then
            'Remove entries before last point below idle speed and after last point above high idle speed (if existing)
            For i = 0 To iDim

                If LnU(i) < IdleSpeedValueForCheck Then
                    If LnU(i + 1) >= IdleSpeedValueForCheck Then
                        LnU_temp.Add(LnU(i))
                        LTq_temp.Add(LTq(i))

                    End If
                Else
                    If i < iDim Then
                        If LTq(i) >= 0 And LTq(i + 1) >= 0 Then
                            LnU_temp.Add(LnU(i))
                            LTq_temp.Add(LTq(i))
                        ElseIf LTq(i) >= 0 And LTq(i + 1) < 0 Then
                            LnU_temp.Add(LnU(i))
                            LTq_temp.Add(LTq(i))
                            LnU_temp.Add(LnU(i + 1))
                            LTq_temp.Add(LTq(i + 1))
                            Exit For
                        Else
                            'Error that full load has alternating positive/negative torque values
                            WorkerMsg(tMsgID.Err, "Data error in engine full load curve (alternating pos./neg. torque values)!" & " (" & FilePath & ")")
                        End If
                    Else
                        'separate handling of last entry if FOR-loop was not terminated by above conditions
                        LnU_temp.Add(LnU(i))
                        LTq_temp.Add(LTq(i))
                    End If
                End If

            Next

            'values are from now on stored in "temp" variables
            ' --> values are sorted by ascending engine speed
            LnU.Clear()
            LTq.Clear()

            LnU.AddRange(LnU_temp)
            LTq.AddRange(LTq_temp)

            'values are from now on stored in "final" variables
            ' --> values are sorted by ascending engine speed
            LnU_temp.Clear()
            LTq_temp.Clear()

            'index of vector is starting with 0, thus number of entries minus 1
            iDim = LnU.Count - 1

        End If

        '####################################
        ' END: PRE-PROCESSING OF INPUT DATA
        '####################################





        'Sort dictionary
        'sortedlist = From item As KeyValuePair(Of Double, Double) In dic Order By item.Key

        'Generate sorted lists of speed and torque
        'LnU_temp.AddRange(From item As KeyValuePair(Of Double, Double) In sortedlist Select item.Key)
        'LTq_temp.AddRange(From item As KeyValuePair(Of Double, Double) In sortedlist Select item.Value)
        '= SAME AS:
        'For Each item As KeyValuePair(Of Double, Double) In sortedlist
        '	LnU_original.Add(item.Key)
        '	LTq_original.Add(item.Value)
        '      Next






        'Check if full-load dataset contains idle speed (first entry <= idle speed)
        If LnU(0) > (IdleSpeedValueForCheck + 8) Then
            WorkerMsg(tMsgID.Err, "Engine full load or motoring curve does not start at idle speed (declared idle speed + 8 [1/min] tolerance)!" & " (" & FilePath & ")")
            Return False
        End If

        'Add idle point and one point 8rpm below idle (might be necessary for main VECTO)
        If LnU(0) > IdleSpeedValueForCheck Then

            LnU_temp.Add(IdleSpeedValueForCheck - 8)
            LTq_temp.Add(LTq(0))
            LnU_temp.Add(IdleSpeedValueForCheck)
            LTq_temp.Add(LTq(0))
            LnU_temp.AddRange(LnU)
            LTq_temp.AddRange(LTq)

            'values are from now on stored in "temp" variables
            ' --> values are sorted by ascending engine speed
            LnU.Clear()
            LTq.Clear()

            LnU.AddRange(LnU_temp)
            LTq.AddRange(LTq_temp)

            'values are from now on stored in "final" variables
            ' --> values are sorted by ascending engine speed
            LnU_temp.Clear()
            LTq_temp.Clear()

            'index of vector is starting with 0, thus number of entries minus 1
            iDim = LnU.Count - 1

        ElseIf LnU(0) > (IdleSpeedValueForCheck - 8) Then

            LnU_temp.Add(IdleSpeedValueForCheck - 8)
            LTq_temp.Add(LTq(0))
            LnU_temp.AddRange(LnU)
            LTq_temp.AddRange(LTq)

            'values are from now on stored in "temp" variables
            ' --> values are sorted by ascending engine speed
            LnU.Clear()
            LTq.Clear()

            LnU.AddRange(LnU_temp)
            LTq.AddRange(LTq_temp)

            'values are from now on stored in "final" variables
            ' --> values are sorted by ascending engine speed
            LnU_temp.Clear()
            LTq_temp.Clear()

            'index of vector is starting with 0, thus number of entries minus 1
            iDim = LnU.Count - 1

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

        n_idle = IdleSpeedValueForCheck

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
        Dim nU As Double
        Dim nUmax As Double
        Dim nUrated As Double
        Dim dnU As Double
        Dim P As Double

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

    Private Function fnUofPfull(ByVal PeTarget As Double, ByVal FromLeft As Boolean) As Double
        Dim Pe As Double
        Dim LastPe As Double
        Dim nU As Double
        Dim nUmin As Double
        Dim nUmax As Double
        Dim nUtarget As Double
        Dim dnU As Double

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

    Private Function Area(ByVal nFrom As Double, ByVal nTo As Double) As Double
        Dim A As Double
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


    Private Function fNpref() As Double
        Dim i As Integer
        Dim Amax As Double
        Dim N95h As Double
        Dim n As Double
        Dim T0 As Double
        Dim dn As Double
        Dim A As Double
        Dim k As Double


        dn = 0.001

        N95h = n_95h

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


    Public Function Tq(ByVal nU As Double) As Double
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
