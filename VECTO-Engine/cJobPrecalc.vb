Imports System.Xml.Linq

Public Class cJobPrecalc

    Public R49TqFile As String
    Private R49Tq As cFLD0


    Public Function Run() As Boolean

        Dim TargetRPMs As New List(Of Single)
        Dim nUStep As Single
        Dim IntervalCount As Integer
        Dim i As Integer

        'Initialize Warning counter
        NumWarnings = 0

        WorkerMsg(tMsgID.Normal, "Reading files")

        R49Tq = New cFLD0
        R49Tq.FilePath = R49TqFile
        If Not R49Tq.ReadFile(False) Then Return False

        If Not R49Tq.RpmCalc("Analysing full-load of CO2-Parent engine") Then Return False

        WorkerMsg(tMsgID.Normal, "Characteristic engine speeds calculated from full-load of CO2-Parent engine:")
        WorkerMsg(tMsgID.Normal, "   n_idle: " & R49Tq.n_idle.ToString("0.0") & " [1/min]")
        WorkerMsg(tMsgID.Normal, "   n_lo: " & R49Tq.n_lo.ToString("0.0") & " [1/min]")
        WorkerMsg(tMsgID.Normal, "   n_pref: " & R49Tq.n_pref.ToString("0.0") & " [1/min]")
        WorkerMsg(tMsgID.Normal, "   n_95h: " & R49Tq.n_95h.ToString("0.0") & " [1/min]")
        WorkerMsg(tMsgID.Normal, "   n_hi: " & R49Tq.n_hi.ToString("0.0") & " [1/min]")
        WorkerMsg(tMsgID.Normal, "   *****")
        WorkerMsg(tMsgID.Normal, "   n_57: " & R49Tq.n_57.ToString("0.0") & " [1/min]")
        WorkerMsg(tMsgID.Normal, "   n_A: " & R49Tq.n_A.ToString("0.0") & " [1/min]")
        WorkerMsg(tMsgID.Normal, "   n_B: " & R49Tq.n_B.ToString("0.0") & " [1/min]")
        WorkerMsg(tMsgID.Normal, " ")


        'Calculate target rpms for FC Map
        TargetRPMs.Clear()
        TargetRPMs.Add(Math.Round(R49Tq.n_idle, 1))
        TargetRPMs.Add(Math.Round(R49Tq.n_A, 1))
        TargetRPMs.Add(Math.Round(R49Tq.n_B, 1))
        TargetRPMs.Add(Math.Round(R49Tq.n_95h, 1))

        IntervalCount = 4
        'Fill in list part1
        nUStep = (R49Tq.n_A - R49Tq.n_idle) / IntervalCount
        For i = 1 To IntervalCount - 1
            TargetRPMs.Add(Math.Round(R49Tq.n_idle + nUStep * i, 1))
        Next
        'Fill in list part2
        nUStep = (R49Tq.n_95h - R49Tq.n_B) / IntervalCount
        For i = 1 To IntervalCount - 1
            TargetRPMs.Add(Math.Round(R49Tq.n_B + nUStep * i, 1))
        Next

        'Sort list
        TargetRPMs.Sort()



        WorkerMsg(tMsgID.Normal, "Target grid for fuel map calculated from full-load of CO2-Parent engine:")
        WorkerMsg(tMsgID.Normal, "   engine speed points: " & String.Join(", ", TargetRPMs.ToArray) & " [1/min]")
        WorkerMsg(tMsgID.Normal, "   engine torque stepsize: " & (R49Tq.TqMax / 10).ToString("0.0") & " [Nm]")

        WorkerMsg(tMsgID.Normal, "Completed.")

        If NumWarnings > 0 Then
            WorkerMsg(tMsgID.Warn, "ATTENTION:  " & NumWarnings & " Warnings occured: Please check detailled descriptions in 'Message Window'!")
        End If


        Return True

    End Function

End Class
