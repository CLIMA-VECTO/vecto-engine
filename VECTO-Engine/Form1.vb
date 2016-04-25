Imports System.Collections.Generic


Public Class Form1

	Private Job As cJob
    Private JobSuccess As Boolean
    Public Lic As New ivtlic.cLicense


	Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load

        Me.Text = AppName & AppVersionForm

		Worker = Me.BgWorker

        ' Licencemodul
        Lic.FilePath = MyAppPath & "License.dat"
        Lic.AppCode = AppName
        Lic.AppVersion = AppVersion

#If DEBUG Then
		Const LicCheck As Boolean = False
#Else
		Const LicCheck as Boolean = True
#End If

        'Lizenz checken
		If LicCheck And Not Lic.LICcheck() Then
			MsgBox(Lic.FailMsg, MsgBoxStyle.Critical)
			Lic.CreateActFile(MyAppPath & "ActivationCode.dat")
			MsgBox("Activation code created under: " & MyAppPath & "ActivationCode.dat")
			Me.Close()
		End If

        ' Abfragen ob Ablauf der License bevorsteht
        If Lic.TimeWarn Then
            WorkerMsg(tMsgID.Warn, "!!!     License expiring date (y/m/d): " & Lic.ExpTime & "     !!!")
        End If

	End Sub

	Private Sub BtStart_Click(sender As Object, e As EventArgs) Handles BtStart.Click

		Job = New cJob

		Try
			JobInit()

			If Not CheckInput() Then
				JobEnd()
				Exit Sub
			End If

			Job = New cJob

			Job.MapFile = Me.TbFuelMap.Text
			Job.R85TqFile = Me.TbR85.Text
			Job.R49TqFile = Me.TbR49.Text
			Job.R49DragFile = Me.TbDrag.Text
			Job.WHTCmeasFile = Me.TbWHTC.Text

			Job.n_idle = Me.TbNidle.Text
			Job.n_lo = Me.TbNlo.Text
			Job.n_pref = Me.TbNpref.Text
			Job.n_95h = Me.TbN95h.Text
			Job.n_hi = Me.TbNhi.Text

			Job.OutPath = Me.TbOutputFolder.Text & "\"

		Catch ex As Exception
			ShowMsgDirect(ex.Message, tMsgID.Err)
			Exit Sub
		End Try

		Worker.RunWorkerAsync()

	End Sub

	Private Sub JobInit()

		Me.GrInput.Enabled = False
		Me.LvMsg.Items.Clear()
		Me.TbUrbanFactor.Clear()
		Me.TbRuralFactor.Clear()
		Me.TbMotorwayFactor.Clear()

	End Sub

	Private Sub JobEnd()
		Me.GrInput.Enabled = True

	End Sub

	Private Sub BgWorker_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BgWorker.DoWork

		If SetCulture Then
			Try
				System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("en-US")
			Catch ex As Exception
				ShowMsgDirect("Failed to set thread culture 'en-US'! Check system decimal- and group- separators and restart application!", tMsgID.Err)
				Exit Sub
			End Try
		End If

		JobSuccess = Job.Run()

	End Sub

	Private Sub BgWorker_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles BgWorker.ProgressChanged
		Dim WorkerMsg As cWorkerMsg

		WorkerMsg = e.UserState

		ShowMsgDirect(WorkerMsg.Msg, WorkerMsg.MsgType)
	End Sub

	Private Sub BgWorker_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BgWorker.RunWorkerCompleted
		JobEnd()

		If JobSuccess Then
            Me.TbUrbanFactor.Text = Job.WHTCurbanFactor.ToString("0.000")
            Me.TbRuralFactor.Text = Job.WHTCruralFactor.ToString("0.000")
            Me.TbMotorwayFactor.Text = Job.WHTCmotorwayFactor.ToString("0.000")
		End If

	End Sub

	Private Function CheckInput() As Boolean
		Dim Result As Boolean

		Result = True

		'Check if numbers are numbers and files do exist
		If Trim(Me.TbFuelMap.Text) = "" OrElse Not IO.File.Exists(Me.TbFuelMap.Text) Then
			ShowMsgDirect("Fuel Map not found!", tMsgID.Err)
			Result = False
		End If

		If Trim(Me.TbR49.Text) = "" OrElse Not IO.File.Exists(Me.TbR49.Text) Then
            ShowMsgDirect("Parent R49 file not found!", tMsgID.Err)
			Result = False
		End If

		If Trim(Me.TbR85.Text) = "" OrElse Not IO.File.Exists(Me.TbR85.Text) Then
            ShowMsgDirect("Child R49 file not found!", tMsgID.Err)
			Result = False
		End If

		If Trim(Me.TbDrag.Text) = "" OrElse Not IO.File.Exists(Me.TbDrag.Text) Then
			ShowMsgDirect("Drag file not found!", tMsgID.Err)
			Result = False
		End If

		If Trim(Me.TbWHTC.Text) = "" OrElse Not IO.File.Exists(Me.TbWHTC.Text) Then
			ShowMsgDirect("WHTC file not found!", tMsgID.Err)
			Result = False
		End If

		If Trim(Me.TbOutputFolder.Text) = "" OrElse Not IO.Directory.Exists(Me.TbOutputFolder.Text) Then
			ShowMsgDirect("Output folder not found!", tMsgID.Err)
			Result = False
		End If


		If Not IsNumeric(Me.TbNidle.Text) Then
			ShowMsgDirect("n_idle is not valid!", tMsgID.Err)
			Result = False
		End If

		If Not IsNumeric(Me.TbNlo.Text) Then
			ShowMsgDirect("n_lo is not valid!", tMsgID.Err)
			Result = False
		End If

		If Not IsNumeric(Me.TbNpref.Text) Then
			ShowMsgDirect("n_pref is not valid!", tMsgID.Err)
			Result = False
		End If

		If Not IsNumeric(Me.TbN95h.Text) Then
			ShowMsgDirect("n_95h is not valid!", tMsgID.Err)
			Result = False
		End If

		If Not IsNumeric(Me.TbNhi.Text) Then
			ShowMsgDirect("n_hi is not valid!", tMsgID.Err)
			Result = False
		End If




		Return Result

	End Function

	Private Sub ShowMsgDirect(ByVal Msg As String, ByVal MsgType As tMsgID)
		Dim lv0 As ListViewItem

		lv0 = New ListViewItem
		lv0.Text = Msg

		Select Case MsgType
			Case tMsgID.Err
				lv0.ForeColor = Color.Red
			Case tMsgID.Warn
				lv0.ForeColor = Color.DarkOrange
		End Select

		Me.LvMsg.Items.Add(lv0)

		lv0.EnsureVisible()

	End Sub



	Private Sub BtOpenMap_Click(sender As Object, e As EventArgs) Handles BtOpenMap.Click
		Dim dlog As New OpenFileDialog
		dlog.Filter = "VECTO Fuel Map (*.vmap)|*.vmap|All files (*.*)|*.*"
		If dlog.ShowDialog() = Windows.Forms.DialogResult.OK Then Me.TbFuelMap.Text = dlog.FileName
	End Sub

	Private Sub TbFuelMap_TextChanged(sender As Object, e As EventArgs) Handles TbFuelMap.TextChanged
		If Me.TbOutputFolder.Text = "" AndAlso Me.TbFuelMap.Text <> "" AndAlso IO.File.Exists(Me.TbFuelMap.Text) Then
			Me.TbOutputFolder.Text = System.IO.Path.GetDirectoryName(Me.TbFuelMap.Text)
		End If
	End Sub

	Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
		Dim dlog As New OpenFileDialog
		dlog.Filter = "Comma-separated values (*.csv)|*.csv|All files (*.*)|*.*"
		If dlog.ShowDialog() = Windows.Forms.DialogResult.OK Then Me.TbR49.Text = dlog.FileName
	End Sub

	Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
		Dim dlog As New OpenFileDialog
		dlog.Filter = "Comma-separated values (*.csv)|*.csv|All files (*.*)|*.*"
		If dlog.ShowDialog() = Windows.Forms.DialogResult.OK Then Me.TbR85.Text = dlog.FileName
	End Sub


	Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
		Dim dlog As New OpenFileDialog
		dlog.Filter = "Comma-separated values (*.csv)|*.csv|All files (*.*)|*.*"
		If dlog.ShowDialog() = Windows.Forms.DialogResult.OK Then Me.TbDrag.Text = dlog.FileName
	End Sub

	Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
		Dim dlog As New OpenFileDialog
		dlog.Filter = "Comma-separated values (*.csv)|*.csv|All files (*.*)|*.*"
		If dlog.ShowDialog() = Windows.Forms.DialogResult.OK Then Me.TbWHTC.Text = dlog.FileName
	End Sub

	Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
		Dim dlog As New FolderBrowserDialog
		If dlog.ShowDialog = Windows.Forms.DialogResult.OK Then Me.TbOutputFolder.Text = dlog.SelectedPath
	End Sub

	'Text-to-number
	Public Function fTextboxToNumString(ByVal txt As String) As String
		If Not IsNumeric(txt) Then
			Return "0"
		Else
			Return txt
		End If
	End Function



	

	
End Class
