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

Public Class cSubPath
	Private sFullPath As String
	Private sOglPath As String
	Private bDefined As Boolean

	Public Sub New()
		bDefined = False
	End Sub

	Public Sub Init(ParentDir As String, Path As String)
		If fFileOrNot(Path) = "" Then
			bDefined = False
		Else
			bDefined = True
			sOglPath = Path
			sFullPath = fFileRepl(Path, ParentDir)
		End If
	End Sub

	Private Function fFileOrNot(f As String) As String
		If Trim(UCase(f)) = sKey.NoFile Then
			Return ""
		Else
			Return f
		End If
	End Function


	Public Sub Clear()
		bDefined = False
	End Sub

	Public ReadOnly Property FullPath As String
		Get
			If bDefined Then
				Return sFullPath
			Else
				Return ""
			End If
		End Get
	End Property

	Public ReadOnly Property OriginalPath As String
		Get
			If bDefined Then
				Return sOglPath
			Else
				Return ""
			End If
		End Get
	End Property

	Public ReadOnly Property PathOrDummy As String
		Get
			If bDefined Then
				Return sOglPath
			Else
				Return sKey.NoFile
			End If
		End Get
	End Property
End Class
