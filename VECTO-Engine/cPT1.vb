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

Public Class cPT1
	Private ReadOnly lnU As New List(Of Double)
	Private ReadOnly lPT1 As New List(Of Double)
	Private iDim As Integer
	'Public Filepath As String

	Public Function Init() As Boolean

		Dim theAssembly As Assembly = Assembly.GetExecutingAssembly()
		Dim resource As Stream = theAssembly.GetManifestResourceStream("VECTO_Engine.PT1.csv")

		If IsNothing(resource) Then
			Throw New Exception("PT1 data not found")
		End If


		Dim line As String()
		iDim = -1
		lnU.Clear()
		lPT1.Clear()

		'Skip Header
		Dim reader = New StreamReader(resource)
		reader.ReadLine()

		Try

			Do While Not reader.EndOfStream
				line = reader.ReadLine.Split(New Char() {","c}, 2, StringSplitOptions.RemoveEmptyEntries)
				iDim += 1
				lnU.Add(CSng(line(0)))
				lPT1.Add(CSng(line(1)))
			Loop

		Catch ex As Exception
			reader.Close()
			WorkerMsg(tMsgID.Err, "Failed to load PT1.csv!" & ex.Message)
			Return False

		End Try

		reader.Close()

		Return True
	End Function


	Public Function PT1(nU As Double) As Double
		Dim i As Int32

		'Extrapolation for x < x(1)
		If lnU(0) >= nU Then
			i = 1
			GoTo lbInt
		End If

		i = 0
		Do While lnU(i) < nU And i < iDim
			i += 1
		Loop


lbInt:
		'Interpolation
		Return (nU - lnU(i - 1)) * (lPT1(i) - lPT1(i - 1)) / (lnU(i) - lnU(i - 1)) + lPT1(i - 1)
	End Function
End Class
