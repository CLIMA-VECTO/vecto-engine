Namespace My
	' Für MyApplication sind folgende Ereignisse verfügbar:
	' 
	' Startup: Wird beim Starten der Anwendung noch vor dem Erstellen des Startformulars ausgelöst.
	' Shutdown: Wird nach dem Schließen aller Anwendungsformulare ausgelöst. Dieses Ereignis wird nicht ausgelöst, wenn die Anwendung nicht normal beendet wird.
	' UnhandledException: Wird ausgelöst, wenn in der Anwendung ein Ausnahmefehler auftritt.
	' StartupNextInstance: Wird beim Starten einer Einzelinstanzanwendung ausgelöst, wenn diese bereits aktiv ist. 
	' NetworkAvailabilityChanged: Wird beim Herstellen oder Trennen der Netzwerkverbindung ausgelöst.
	Partial Friend Class MyApplication

		Private Sub MyApplication_Startup(sender As Object, e As ApplicationServices.StartupEventArgs) Handles Me.Startup


			MyAppPath = My.Application.Info.DirectoryPath & "\"
			MyConfPath = MyAppPath & "Config\"

			sKey = New csKey

			'Separator!
			SetCulture = False
			If System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator <> "." Then
				SetCulture = True
				Try
					System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("en-US")
					System.Threading.Thread.CurrentThread.CurrentUICulture = New System.Globalization.CultureInfo("en-US")
					'MSGtoForm(8, "Set CurrentCulture to 'en-US'", True)
				Catch ex As Exception
					MsgBox("Failed to set Application Regional Settings to 'en-US'! Check system decimal- and group- separators and restart application!")
				End Try
			End If


		End Sub



	End Class


End Namespace

