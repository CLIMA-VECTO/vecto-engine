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
Imports System.Text
Imports Microsoft.VisualBasic.FileIO

Public Class cFile_V3
	Private ReadOnly FileFormat As Encoding = Encoding.UTF8

	Private TxtFldParser As TextFieldParser
	Private StrWrter As StreamWriter
	Private Mode As FileMode
	Private Path As String
	Private Sepp As String
	Private SkipCom As Boolean
	Private StopE As Boolean
	Private FileOpen As Boolean
	Private PreLine As String()
	Private FileEnd As Boolean

	Public Sub New()
		Me.Reset()
	End Sub

	Private Sub Reset()
		FileOpen = False
		Mode = FileMode.Undefined
		PreLine = Nothing
		FileEnd = False
	End Sub

	Public Function OpenRead(FileName As String, Optional ByVal Separator As String = ",",
							 Optional ByVal SkipComment As Boolean = True, Optional ByVal StopAtE As Boolean = False) _
		As Boolean
		Me.Reset()
		StopE = StopAtE
		Path = FileName
		Sepp = Separator
		SkipCom = SkipComment
		If Not (Mode = FileMode.Undefined) Then Return False
		If Not File.Exists(Path) Then Return False
		Mode = FileMode.Read
		Try
			TxtFldParser = New TextFieldParser(Path, Encoding.Default)
			FileOpen = True
		Catch ex As Exception
			Return False
		End Try
		TxtFldParser.TextFieldType = FieldType.Delimited
		TxtFldParser.Delimiters = New String() {Sepp}

		'If TxtFldParser.EndOfData Then Return False

		Me.ReadLine()
		Return True
	End Function

	Public Function ReadLine() As String()
		Dim line As String()
		Dim line0 As String

		line = PreLine

lb10:
		If TxtFldParser.EndOfData Then

			FileEnd = True

		Else

			PreLine = TxtFldParser.ReadFields
			line0 = UCase(Trim(PreLine(0)))

			If SkipCom Then
				If Left(line0, 1) = "#" Then GoTo lb10
			End If

			If StopE Then FileEnd = (line0 = "E")

		End If

		Return line
	End Function

	Public Sub Close()
		Select Case Mode
			Case FileMode.Read
				If FileOpen Then TxtFldParser.Close()
				TxtFldParser = Nothing
			Case FileMode.Write
				If FileOpen Then StrWrter.Close()
				StrWrter = Nothing
		End Select
		Me.Reset()
	End Sub

	Public ReadOnly Property EndOfFile As Boolean
		Get
			Return FileEnd
		End Get
	End Property

	Public Function OpenWrite(FileName As String, Optional ByVal Separator As String = ",",
							  Optional ByVal AutoFlush As Boolean = False, Optional ByVal Append As Boolean = False) _
		As Boolean
		Me.Reset()
		Path = FileName
		Sepp = Separator
		If Not (Mode = FileMode.Undefined) Then Return False
		Mode = FileMode.Write
		Try
			StrWrter = My.Computer.FileSystem.OpenTextFileWriter(Path, Append, FileFormat)
			FileOpen = True
		Catch ex As Exception
			Return False
		End Try
		StrWrter.AutoFlush = AutoFlush
		Return True
	End Function

	Public Sub WriteLine(ByVal ParamArray x() As Object)
		Dim St As String
		Dim StB As New StringBuilder
		Dim Skip As Boolean
		Skip = True
		For Each St In x
			If Skip Then
				StB.Append(St)
				Skip = False
			Else
				StB.Append(Sepp & St)
			End If
		Next
		StrWrter.WriteLine(StB.ToString)
		StB = Nothing
	End Sub

	Public Sub WriteLine(x As String)
		StrWrter.WriteLine(x)
	End Sub

	Private Enum FileMode
		Undefined
		Read
		Write
	End Enum
End Class
