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
Imports System.Globalization
Imports System.Threading
Imports Microsoft.VisualBasic.ApplicationServices

Namespace My
	' Für MyApplication sind folgende Ereignisse verfügbar:
	' 
	' Startup: Wird beim Starten der Anwendung noch vor dem Erstellen des Startformulars ausgelöst.
	' Shutdown: Wird nach dem Schließen aller Anwendungsformulare ausgelöst. Dieses Ereignis wird nicht ausgelöst, wenn die Anwendung nicht normal beendet wird.
	' UnhandledException: Wird ausgelöst, wenn in der Anwendung ein Ausnahmefehler auftritt.
	' StartupNextInstance: Wird beim Starten einer Einzelinstanzanwendung ausgelöst, wenn diese bereits aktiv ist. 
	' NetworkAvailabilityChanged: Wird beim Herstellen oder Trennen der Netzwerkverbindung ausgelöst.
	Partial Friend Class MyApplication
		Private Sub MyApplication_Startup(sender As Object, e As StartupEventArgs) Handles Me.Startup


			MyAppPath = Application.Info.DirectoryPath & "\"
			MyConfPath = MyAppPath & "Config\"

			sKey = New csKey

			'Separator!
			SetCulture = False
			If CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator <> "." Then
				SetCulture = True
				Try
					Thread.CurrentThread.CurrentCulture = New CultureInfo("en-US")
					Thread.CurrentThread.CurrentUICulture = New CultureInfo("en-US")
					'MSGtoForm(8, "Set CurrentCulture to 'en-US'", True)
				Catch ex As Exception
					MsgBox(
						"Failed to set Application Regional Settings to 'en-US'! Check system decimal- and group- separators and restart application!")
				End Try
			End If
		End Sub
	End Class
End Namespace

