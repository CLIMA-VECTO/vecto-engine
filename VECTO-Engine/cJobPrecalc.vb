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

Public Class cJobPrecalc
	Public FlcParentFile As String
	Private FlcParent As cFLD0
	Public Idle_Parent As Double


	Public Function Run() As Boolean

		Dim TargetRPMs As New List(Of Double)
		Dim nUStep As Double
		Dim IntervalCount(1) As Integer
		Dim i As Integer
		Dim dn_idle_A_44 As Double
		Dim dn_B_95h_44 As Double
		Dim dn_idle_A_35 As Double
		Dim dn_B_95h_35 As Double
		Dim dn_idle_A_53 As Double
		Dim dn_B_95h_53 As Double
		Dim dn_44 As Double
		Dim dn_35 As Double
		Dim dn_53 As Double


		'Initialize Warning counter
		NumWarnings = 0

		WorkerMsg(tMsgID.Normal, "Analyzing input file")

		FlcParent = New cFLD0
		FlcParent.FilePath = FlcParentFile
		FlcParent.IdleSpeedValueForCheck = Idle_Parent
		If Not FlcParent.ReadFile(True, True) Then Return False

		If Not FlcParent.RpmCalc("Analysing full-load of CO2-Parent engine") Then Return False

		WorkerMsg(tMsgID.Normal, "Characteristic engine speeds calculated from full-load of CO2-Parent engine:")
		WorkerMsg(tMsgID.Normal, "   n_idle: " & FlcParent.n_idle.ToString("0.00") & " [1/min]")
		WorkerMsg(tMsgID.Normal, "   n_lo: " & FlcParent.n_lo.ToString("0.00") & " [1/min]")
		WorkerMsg(tMsgID.Normal, "   n_pref: " & FlcParent.n_pref.ToString("0.00") & " [1/min]")
		WorkerMsg(tMsgID.Normal, "   n_95h: " & FlcParent.n_95h.ToString("0.00") & " [1/min]")
		WorkerMsg(tMsgID.Normal, "   n_hi: " & FlcParent.n_hi.ToString("0.00") & " [1/min]")
		WorkerMsg(tMsgID.Normal, "   *****")
		WorkerMsg(tMsgID.Normal, "   n_57: " & FlcParent.n_57.ToString("0.00") & " [1/min]")
		WorkerMsg(tMsgID.Normal, "   n_A: " & FlcParent.n_A.ToString("0.00") & " [1/min]")
		WorkerMsg(tMsgID.Normal, "   n_B: " & FlcParent.n_B.ToString("0.00") & " [1/min]")
		WorkerMsg(tMsgID.Normal, " ")


		'Calculate target rpms for FC Map
		TargetRPMs.Clear()
		TargetRPMs.Add(Math.Round(FlcParent.n_idle, 2))
		TargetRPMs.Add(Math.Round(FlcParent.n_A, 2))
		TargetRPMs.Add(Math.Round(FlcParent.n_B, 2))
		TargetRPMs.Add(Math.Round(FlcParent.n_95h, 2))

		'Calculate speed intervals for definition of 4/4, 3/5 or 5/3 distribution
		dn_idle_A_44 = (FlcParent.n_A - FlcParent.n_idle) / 4
		dn_B_95h_44 = (FlcParent.n_95h - FlcParent.n_B) / 4
		dn_idle_A_35 = (FlcParent.n_A - FlcParent.n_idle) / 3
		dn_B_95h_35 = (FlcParent.n_95h - FlcParent.n_B) / 5
		dn_idle_A_53 = (FlcParent.n_A - FlcParent.n_idle) / 5
		dn_B_95h_53 = (FlcParent.n_95h - FlcParent.n_B) / 3

		dn_44 = Math.Abs(dn_idle_A_44 - dn_B_95h_44)
		dn_35 = Math.Abs(dn_idle_A_35 - dn_B_95h_35)
		dn_53 = Math.Abs(dn_idle_A_53 - dn_B_95h_53)

		If (dn_44 < dn_35) And (dn_44 < dn_53) Then
			IntervalCount(0) = 4
			IntervalCount(1) = 4
		ElseIf (dn_35 < dn_44) And (dn_35 < dn_53) Then
			IntervalCount(0) = 3
			IntervalCount(1) = 5
		ElseIf (dn_53 < dn_44) And (dn_53 < dn_35) Then
			IntervalCount(0) = 5
			IntervalCount(1) = 3
		End If

		'Fill in list part1
		nUStep = (FlcParent.n_A - FlcParent.n_idle) / IntervalCount(0)
		For i = 1 To IntervalCount(0) - 1
			TargetRPMs.Add(Math.Round(FlcParent.n_idle + nUStep * i, 1))
		Next
		'Fill in list part2
		nUStep = (FlcParent.n_95h - FlcParent.n_B) / IntervalCount(1)
		For i = 1 To IntervalCount(1) - 1
			TargetRPMs.Add(Math.Round(FlcParent.n_B + nUStep * i, 1))
		Next

		'Sort list
		TargetRPMs.Sort()


		WorkerMsg(tMsgID.Normal, "Target grid for fuel map calculated from full-load of CO2-Parent engine:")
		WorkerMsg(tMsgID.Normal,
				  "   engine speed points: " & String.Join(", ", TargetRPMs.Select(Function(x) x.ToString("F2")).ToArray) &
				  " [1/min]")
		WorkerMsg(tMsgID.Normal, "   engine torque stepsize: " & (FlcParent.TqMax / 10).ToString("0.00") & " [Nm]")

		WorkerMsg(tMsgID.Normal, "Completed.")

		If NumWarnings > 0 Then
			WorkerMsg(tMsgID.Warn,
					  "ATTENTION:  " & NumWarnings &
					  " Warnings occured: Please check detailled descriptions in 'Message Window'!")
		End If


		Return True
	End Function
End Class
