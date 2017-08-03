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
Imports System.Reflection

Public Class cWHTC
	Public FullLoad As cFLD0
	Public Drag As cFLD0
	Public Map As cMAP0
	Public PT1 As cPT1

    Private ReadOnly lTime As New List(Of Double)
    Private ReadOnly lnU As New List(Of Double)
    Private ReadOnly lTq As New List(Of Double)
    Private ReadOnly lFC As New List(Of Double)
    Private ReadOnly lPe As New List(Of Double)
	Private iDim As Integer

    Public Urban As Double
    Public Rural As Double
    Public Motorway As Double
    Public TotWork As Double
    Public TotFCspec As Double

    Public WHTC_n_idle As Double
    Public WHTC_n_lo As Double
    Public WHTC_n_hi As Double
    Public WHTC_n_pref As Double


	Public Function InitCycle(Measurement) As Boolean

		Dim line As String()
        Dim nU As Double
        Dim Tq As Double

		'Dim theAssembly As Assembly = Assembly.GetExecutingAssembly()
		Dim resource As Stream = ResourceHelper.ReadStream("VECTO_Engine.WHTC.csv") ' theAssembly.GetManifestResourceStream("VECTO_Engine.WHTC.csv")

		If IsNothing(resource) Then
			Throw New Exception("WHTC data not found")
		End If


		lTime.Clear()
		lnU.Clear()
		lTq.Clear()
		lFC.Clear()
		Dim reader = New StreamReader(resource)

		Try
			'Skip header
			reader.ReadLine()
			reader.ReadLine()
			'Read lines
			iDim = -1
			Do While Not reader.EndOfStream
				Dim input As String = reader.ReadLine
				If input.StartsWith("#") Then
					Continue Do
				End If
				line = Input.Split(New Char() {","c}, 3, StringSplitOptions.RemoveEmptyEntries)

				iDim += 1

				If Measurement Then

					nU = line(1)
					Tq = line(2)
					lFC.Add(line(3))

				Else

                    nU = line(1)
                    Tq = line(2)

					'Denorm
					'nU = nU * 0.01 * (0.45 * FullLoad.n_lo + 0.45 * FullLoad.n_pref + 0.1 * FullLoad.n_hi - FullLoad.n_idle) * 2.0327 + FullLoad.n_idle
					'Calculate WHTC engine speeds with values from input form
					nU = nU * 0.01 * (0.45 * WHTC_n_lo + 0.45 * WHTC_n_pref + 0.1 * WHTC_n_hi - WHTC_n_idle) * 2.0327 + WHTC_n_idle

					If Tq < 0 Then
						Tq = Drag.Tq(nU)
					Else
						Tq = Tq * 0.01 * (FullLoad.Tq(nU))
					End If

					lFC.Add(0)

				End If

				lTime.Add(line(0))
				lnU.Add(nU)
				lTq.Add(Tq)
				lPe.Add(nTqtoPe(nU, Tq))

				If iDim > 0 Then
					If lTime(iDim) <= lTime(iDim - 1) Then
						WorkerMsg(tMsgID.Err, "Invalid time step in WHTC cycle (" & lTime(iDim) & " [s])!")
						Return False
					End If
				End If

			Loop
		Catch ex As Exception

			reader.Close()

			WorkerMsg(tMsgID.Err, "Failed to load WHTC cycle (" & ex.Message & ")!")
			Return False
		End Try

		reader.Close()
		Return True
	End Function

	Public Function CalcFC() As Boolean

		Dim i As Integer
        Dim Pe As Double
        Dim nU As Double
        Dim Tq As Double
        Dim TqMax As Double
        Dim Pmax As Double
        Dim FC As Double
        Dim PT1val As Double

		Dim FCout As Boolean


		'Power calculation
		For i = 0 To iDim

			nU = lnU(i)
			Tq = lTq(i)

			'FLD Check
			TqMax = FullLoad.Tq(nU)
			Pmax = nTqtoPe(nU, TqMax)
			If i > 0 Then
				PT1val = PT1.PT1(nU)
				Pmax = Math.Min((1 / (PT1val + 1)) * (Pmax + PT1val * Pe), Pmax)  'Pe = Power from previous time step!!
			End If

			'Power - after PT1 correction!!!!!
			Pe = nTqtoPe(nU, Tq)

			If Pe > Pmax Then
				Tq = nPeToTq(nU, Pmax)
			End If

			'MAP max Tq check
			TqMax = Map.TqMax(nU)
			FCout = False
			If Tq > TqMax Then
				WorkerMsg(tMsgID.Warn,
						  "WHTC Calc: torque exceeds Fuel Map max torque at " & Math.Round(nU, 2) & " [1/min] by " &
						  Math.Round(Tq - TqMax, 2) & " [Nm] !")
				'Tq = TqMax
				'FCout = True
			End If

			'Recalc power  (if torque corrected by full load)
			Pe = nTqtoPe(nU, Tq)


			'FC Calc
			If Tq < 0 Then
                'Torque < 0 in reference WHTC is always exactly motoring torque, thus FC is 0 per definition
				'Interpolation from FC map would also deliver 0
				FC = 0
			Else
				FC = Map.fFCdelaunay_Intp(nU, Tq)
			End If

			If FCout Then
				WorkerMsg(tMsgID.Warn,
						  "WHTC Calc: " & Math.Round(nU, 2) & " [1/min], " & Math.Round(Tq, 2) & " [Nm], " & Math.Round(FC, 4) &
						  " [g/h]")
			End If

			If FC < 0 And FC > -999 Then FC = 0

			If FC < -999 Then Return False 'Error msg in fFCdelaunay_Intp

			'Overwrite Torque and power (if corrected by full load)
			lTq(i) = Tq
			lPe(i) = Pe

			lFC(i) = FC

		Next


		Return True
	End Function

	Public Function CalcResults(WriteTotalResults As Boolean) As Boolean
		Dim FCsum As Double
		Dim Work As Double
		Dim WorkSum As Double
		Dim i As Integer
        Dim dt As Double
        Dim dtFC As Double
        Dim PeAvg As Double
		Dim part As Integer
        Dim timelimit(2) As Double
        Dim k As Double
        Dim d As Double
        Dim t0 As Double
		Dim TotalWork As Double
		Dim TotalFC As Double
		'Dim TotalFCspec As Double


		timelimit(0) = 900
		timelimit(1) = 1380
		timelimit(2) = 1800

		TotalWork = 0
		TotalFC = 0

		If lTime(iDim) < timelimit(2) Then
			WorkerMsg(tMsgID.Err, "Cannot calculate WHTC results! Cycle is too short.")
			Return False
		End If

		'Skip values until time > 0
		i = 0
		Do While lTime(i) < 0.01
			i += 1
		Loop

		'Calc results
		For part = 0 To 2

			FCsum = 0
			WorkSum = 0
			Do

				'Skip first entry (because "i-1" not available)
				If i > 0 Then

					If (lPe(i) <= 0 AndAlso lPe(i - 1) <= 0) Then
                        'Both time steps idle/motoring

						PeAvg = 0
						'corr GS 18.9.2015
						'dt = 0
						dt = (lTime(i) - lTime(i - 1))

					ElseIf lPe(i) * lPe(i - 1) <= 0 Then
                        'One time step driving, other idle/motoring

						k = (lPe(i) - lPe(i - 1)) / (lTime(i) - lTime(i - 1))
						d = lPe(i) - k * lTime(i)

						t0 = (0 - d) / k

						If lPe(i - 1) <= 0 Then
							dt = (lTime(i) - t0)
							PeAvg = lPe(i) / 2
						Else
							dt = (t0 - lTime(i - 1))
							PeAvg = lPe(i - 1) / 2
						End If

					Else
						'Both time steps driving

						PeAvg = (lPe(i) + lPe(i - 1)) / 2
						dt = (lTime(i) - lTime(i - 1))

					End If

					'dt for FC calc is different that for work
					dtFC = (lTime(i) - lTime(i - 1))

					'Work calculation
					Work = dt * PeAvg / 3600   '[s] * [kW] / 3600[s/h] = [kWh]

					WorkSum += Work	'[kWh]
					FCsum += dtFC * (lFC(i) + lFC(i - 1)) / 2 / 3600   '[s] * [g/h] / 3600[s/h] = [g]
				End If

				i = i + 1
			Loop Until lTime(i - 1) >= timelimit(part)

			Select Case part
				Case 0
					Urban = FCsum / WorkSum			'[g/kWh]
				Case 1
					Rural = FCsum / WorkSum			'[g/kWh]
				Case Else '2
					Motorway = FCsum / WorkSum			'[g/kWh]
			End Select

			TotalWork += WorkSum
			TotalFC += FCsum

		Next


		TotFCspec = TotalFC / TotalWork
		TotWork = TotalWork

		'If WriteTotalResults Then
		'    TotalFCspec = TotalFC / TotalWork
		'    WorkerMsg(tMsgID.Normal, "Total FC = " & TotalFCspec.ToString("0.00") & " [g/kWh]")
		'    WorkerMsg(tMsgID.Normal, "Total Work = " & TotalWork.ToString("0.00") & " [kWh]")
		'End If

		Return True
	End Function
End Class
