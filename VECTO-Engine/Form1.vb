Imports System.ComponentModel
Imports System.Globalization
Imports System.IO
Imports System.Threading
Imports ivtlic


Public Class Form1
	Private Job As cJob
	Private JobSuccess As Boolean
	Public Lic As New cLicense

	Private JobPrecalc As cJobPrecalc


	Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load

		Me.Text = AppName & AppVersionForm

		Worker = Me.BgWorker

		' Licencemodul
		Lic.FilePath = MyAppPath & "License.dat"
		Lic.AppCode = AppName
		Lic.AppVersion = AppVersion

#If DEBUG Then
		Const LicCheck = False
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
		JobSuccess = False

		Try

			'**********************************************
			'REAL PART FOR INPUT DATA ASSIGNMENT
			'**********************************************


			JobInit()

			If Not CheckInput() Then
				JobEnd()
				Exit Sub
			End If


			Job.Manufacturer = Me.TbManufacturer.Text
			Job.Model = Me.TbModel.Text
			Job.CertNumber = Me.TbCertNumber.Text
			Job.Idle_Parent = Me.TbIdle_Parent.Text
			Job.Idle = Me.TbIdle.Text
			Job.Displacement = Me.TbDisplacement.Text
			Job.RatedPower = Me.TbRatedPower.Text
			Job.RatedSpeed = Me.TbRatedSpeed.Text
			Job.FuelType = Me.CbFuelType.Text
			Job.NCVfuel = Me.TbNCVfuel.Text

			Job.MapFile = Me.TbFuelMap.Text
			Job.FlcFile = Me.TbFLC.Text
			Job.FlcParentFile = Me.TbFLC_Parent.Text
			Job.DragFile = Me.TbMotoring.Text

			Job.FCspecMeas_ColdTot = Me.TbFCspecCold.Text
			Job.FCspecMeas_HotTot = Me.TbFCspecHot.Text
			Job.FCspecMeas_HotUrb = Me.TbFCspecUrb.Text
			Job.FCspecMeas_HotRur = Me.TbFCspecRur.Text
			Job.FCspecMeas_HotMw = Me.TbFCspecMW.Text
			Job.CF_RegPer = Me.TbCF_RegPer.Text


			Job.OutPath = Me.TbOutputFolder.Text & "\"


			'**********************************************
			'REAL PART FOR INPUT DATA ASSIGNMENT - END
			'**********************************************


			'**********************************************
			'**********************************************
			'**********************************************
			'      TEST DATA  START
			'**********************************************
			'**********************************************
			'**********************************************


			'Test DataSet 1

			'JobInit()


			'Job.Manufacturer = "TUG"
			'Job.Model = "Testengine"
			'Job.CertNumber = "Engine0815"
			'Job.Idle_Parent = 600
			'Job.Idle = 600
			'Job.Displacement = 7700
			'Job.RatedPower = Me.TbRatedPower.Text
			'Job.RatedSpeed = Me.TbRatedSpeed.Text
			'Job.FuelType = "Ethanol / CI"
			'Job.NCVfuel = 42.3

			'Job.MapFile = "J:\TE-Em\Projekte\I_2013_08_HDV_CO2_LOT_4_SR7\VECTO-Engine\VECTO_Engine ab v1.1\Testfolder\Tests v1.3\TUG demo data\Demo_Map_v2_2Nachkomma.csv"
			''Job.MapFile = "J:\TE-Em\Projekte\I_2013_08_HDV_CO2_LOT_4_SR7\VECTO-Engine\VECTO_Engine ab v1.1\Testfolder\Tests v1.3\TUG demo data\Demo_Map_v2_2Nachkomma_1pTorque5percent.csv"
			'Job.FlcFile = "J:\TE-Em\Projekte\I_2013_08_HDV_CO2_LOT_4_SR7\VECTO-Engine\VECTO_Engine ab v1.1\Testfolder\Tests v1.3\TUG demo data\Demo_FullLoad_Parent_2Nachkomma_1rpmSteps.csv"
			'Job.FlcParentFile = "J:\TE-Em\Projekte\I_2013_08_HDV_CO2_LOT_4_SR7\VECTO-Engine\VECTO_Engine ab v1.1\Testfolder\Tests v1.3\TUG demo data\Demo_FullLoad_Parent_2Nachkomma_1rpmSteps.csv"
			'Job.DragFile = "J:\TE-Em\Projekte\I_2013_08_HDV_CO2_LOT_4_SR7\VECTO-Engine\VECTO_Engine ab v1.1\Testfolder\Tests v1.3\TUG demo data\Demo_Motoring_2Nachkomma_1rpmSteps.csv"

			'Job.FCspecMeas_ColdTot = 200
			'Job.FCspecMeas_HotTot = 200
			'Job.FCspecMeas_HotUrb = 200
			'Job.FCspecMeas_HotRur = 200
			'Job.FCspecMeas_HotMw = 200
			'Job.CF_RegPer = 1


			'Job.OutPath = "J:\TE-Em\Projekte\I_2013_08_HDV_CO2_LOT_4_SR7\VECTO-Engine\VECTO_Engine ab v1.1\Testfolder\Tests v1.3\TUG demo data\results\"


			'Test DataSet 2

			'JobInit()


			'Job.Manufacturer = "TUG"
			'Job.Model = "Testengine"
			'Job.CertNumber = "Engine0815"
			'Job.Idle_Parent = 550
			'Job.Idle = 550
			'Job.Displacement = 7700
			'Job.RatedPower = 390
			'Job.RatedSpeed = 2000
			'Job.FuelType = "Diesel / CI"
			'Job.NCVfuel = 42.3

			'Job.MapFile = "J:\TE-Em\Projekte\I_2013_08_HDV_CO2_LOT_4_SR7\VECTO-Engine\VECTO_Engine ab v1.1\Testfolder\Tests v1.3\Volvo\Volvo_map.csv"
			'Job.FlcFile = "J:\TE-Em\Projekte\I_2013_08_HDV_CO2_LOT_4_SR7\VECTO-Engine\VECTO_Engine ab v1.1\Testfolder\Tests v1.3\Volvo\Volvo_flc.csv"
			'Job.FlcParentFile = "J:\TE-Em\Projekte\I_2013_08_HDV_CO2_LOT_4_SR7\VECTO-Engine\VECTO_Engine ab v1.1\Testfolder\Tests v1.3\Volvo\Volvo_flc.csv"
			'Job.DragFile = "J:\TE-Em\Projekte\I_2013_08_HDV_CO2_LOT_4_SR7\VECTO-Engine\VECTO_Engine ab v1.1\Testfolder\Tests v1.3\Volvo\Volvo_mot.csv"

			'Job.FCspecMeas_ColdTot = 200
			'Job.FCspecMeas_HotTot = 200
			'Job.FCspecMeas_HotUrb = 200
			'Job.FCspecMeas_HotRur = 200
			'Job.FCspecMeas_HotMw = 200
			'Job.CF_RegPer = 1


			'Job.OutPath = "J:\TE-Em\Projekte\I_2013_08_HDV_CO2_LOT_4_SR7\VECTO-Engine\VECTO_Engine ab v1.1\Testfolder\Tests v1.3\Volvo\"


			'**********************************************
			'**********************************************
			'**********************************************
			'      TEST DATA  END
			'**********************************************
			'**********************************************
			'**********************************************


		Catch ex As Exception
			ShowMsgDirect(ex.Message, tMsgID.Err)
			Exit Sub
		End Try

		Worker.RunWorkerAsync()
	End Sub

	Private Sub JobInit()

		Me.GrInput.Enabled = False
		Me.LvMsg.Items.Clear()
		'Me.TbUrbanFactor.Clear()
		'Me.TbRuralFactor.Clear()
		'Me.TbMotorwayFactor.Clear()
	End Sub

	Private Sub JobEnd()

		'Set calc mode back again to normal from Precalc (=1)
		CalcMode = 0

		Me.GrInput.Enabled = True
	End Sub

	Private Sub BgWorker_DoWork(sender As Object, e As DoWorkEventArgs) Handles BgWorker.DoWork

		If SetCulture Then
			Try
				Thread.CurrentThread.CurrentCulture = New CultureInfo("en-US")
			Catch ex As Exception
				ShowMsgDirect(
					"Failed to set thread culture 'en-US'! Check system decimal- and group- separators and restart application!",
					tMsgID.Err)
				Exit Sub
			End Try
		End If

		'Calc mode 1 = only precalculation
		If CalcMode = 1 Then
			JobPrecalc.Run()
		Else
			JobSuccess = Job.Run()
			If JobSuccess Then
				'Messagebox: VALID DESPITE OF WARNINGS
				Dim newline = Environment.NewLine
				MsgBox(
					"DATA EVALUATION IS COMPLETED." & newline & newline &
					"The results produced are valid for certification despite any warnings displayed in the message window!" & newline &
					newline &
					"Nevertheless, causes for warnings shall be analyzed together with the Technical Service or Type Approval Authority.",
					MsgBoxStyle.Information, MsgBoxStyle.OkOnly)

			End If
		End If
	End Sub

	Private Sub BgWorker_ProgressChanged(sender As Object, e As ProgressChangedEventArgs) Handles BgWorker.ProgressChanged
		Dim WorkerMsg As cWorkerMsg

		WorkerMsg = e.UserState

		ShowMsgDirect(WorkerMsg.Msg, WorkerMsg.MsgType)
	End Sub

	Private Sub BgWorker_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) _
		Handles BgWorker.RunWorkerCompleted
		JobEnd()

		'If JobSuccess Then
		'    Me.TbUrbanFactor.Text = Job.WHTCurbanFactor.ToString("0.0000")
		'    Me.TbRuralFactor.Text = Job.WHTCruralFactor.ToString("0.0000")
		'    Me.TbMotorwayFactor.Text = Job.WHTCmotorwayFactor.ToString("0.0000")
		'    Me.TbColdHotFactor.Text = Job.ColdHotBalancingFactor.ToString("0.0000")
		'End If
	End Sub

	Private Function CheckInput() As Boolean
		Dim Result As Boolean
		Dim StringForSplit As String
		Dim StringsAfterSplit As String()

		Result = True

		'Check if numbers are numbers and files do exist

		'check all file references
		If Trim(Me.TbFuelMap.Text) = "" OrElse Not File.Exists(Me.TbFuelMap.Text) Then
			ShowMsgDirect("File for fuel consumption map of CO2-parent engine not found!", tMsgID.Err)
			Result = False
		End If

		If Trim(Me.TbFLC_Parent.Text) = "" OrElse Not File.Exists(Me.TbFLC_Parent.Text) Then
			ShowMsgDirect("File for full-Load curve of CO2-parent engine not found!", tMsgID.Err)
			Result = False
		End If

		If Trim(Me.TbFLC.Text) = "" OrElse Not File.Exists(Me.TbFLC.Text) Then
			ShowMsgDirect("File for full-Load curve of actual engine not found!", tMsgID.Err)
			Result = False
		End If

		'If Trim(Me.TbWHTC.Text) = "" OrElse Not IO.File.Exists(Me.TbWHTC.Text) Then
		'    ShowMsgDirect("WHTC file not found!", tMsgID.Err)
		'    Result = False
		'End If

		If Trim(Me.TbMotoring.Text) = "" OrElse Not File.Exists(Me.TbMotoring.Text) Then
			ShowMsgDirect("File for motoring curve of CO2-parent engine not found!", tMsgID.Err)
			Result = False
		End If

		If Trim(Me.TbOutputFolder.Text) = "" OrElse Not Directory.Exists(Me.TbOutputFolder.Text) Then
			ShowMsgDirect("Output folder not found!", tMsgID.Err)
			Result = False
		End If


		'check all input fields, either string or numeric value
		If Trim(Me.TbManufacturer.Text) = "" Then
			ShowMsgDirect("Field ""Manufacturer"" is empty!", tMsgID.Err)
			Result = False
		End If

		If Trim(Me.TbModel.Text) = "" Then
			ShowMsgDirect("Field ""Model"" is empty!", tMsgID.Err)
			Result = False
		End If

		If Trim(Me.TbCertNumber.Text) = "" Then
			ShowMsgDirect("Field ""Certification Number"" is empty!", tMsgID.Err)
			Result = False
		End If

		If Not IsNumeric(Me.TbIdle_Parent.Text) Then
			ShowMsgDirect("Idle speed of CO2-parent engine is not valid!", tMsgID.Err)
			Result = False
		Else
			If Not IsInteger(Me.TbIdle_Parent.Text) Then
				ShowMsgDirect("Idle speed of CO2-parent engine needs to be rounded to the nearest whole number!", tMsgID.Err)
				Result = False
			End If
		End If

		If Not IsNumeric(Me.TbIdle.Text) Then
			ShowMsgDirect("Engine idle speed is not valid!", tMsgID.Err)
			Result = False
		Else
			If Not IsInteger(Me.TbIdle.Text) Then
				ShowMsgDirect("Engine idle speed needs to be rounded to the nearest whole number!", tMsgID.Err)
				Result = False
			End If
		End If

		If Not IsNumeric(Me.TbDisplacement.Text) Then
			ShowMsgDirect("Engine displacement is not valid!", tMsgID.Err)
			Result = False
		Else
			If Not IsInteger(Me.TbDisplacement.Text) Then
				ShowMsgDirect("Engine displacement needs to be rounded to the nearest whole number!", tMsgID.Err)
				Result = False
			End If
		End If

		If Not IsNumeric(Me.TbRatedPower.Text) Then
			ShowMsgDirect("Engine rated power is not valid!", tMsgID.Err)
			Result = False
		Else
			If Not IsInteger(Me.TbRatedPower.Text) Then
				ShowMsgDirect("Engine rated power needs to be rounded to the nearest whole number!", tMsgID.Err)
				Result = False
			End If
		End If
		If Not IsNumeric(Me.TbRatedSpeed.Text) Then
			ShowMsgDirect("Engine rated speed is not valid!", tMsgID.Err)
			Result = False
		Else
			If Not IsInteger(Me.TbRatedSpeed.Text) Then
				ShowMsgDirect("Engine rated speed needs to be rounded to the nearest whole number!", tMsgID.Err)
				Result = False
			End If
		End If

		If Trim(Me.CbFuelType.Text) = "" Then
			ShowMsgDirect("Field ""Type of test fuel"" is empty!", tMsgID.Err)
			Result = False
		End If

		If Not IsNumeric(Me.TbNCVfuel.Text) Then
			ShowMsgDirect("NCV of test fuel is not valid!", tMsgID.Err)
			Result = False
		Else
			StringForSplit = Me.TbNCVfuel.Text
			If StringForSplit.Contains(".") Then
				StringsAfterSplit = StringForSplit.Split(New String() {"."c}, StringSplitOptions.RemoveEmptyEntries)
				If StringsAfterSplit(1).Length <> 3 Then
					ShowMsgDirect("NCV of test fuel needs to have exactly 3 digits after the decimal point!", tMsgID.Err)
					Result = False
				End If
			Else
				ShowMsgDirect("NCV of test fuel does not contain a valid decimal separator!", tMsgID.Err)
				Result = False
			End If
		End If

		If Not IsNumeric(Me.TbCF_RegPer.Text) Then
			ShowMsgDirect("CF-RegPer is not valid!", tMsgID.Err)
			Result = False
		Else
			StringForSplit = Me.TbCF_RegPer.Text
			If StringForSplit.Contains(".") Then
				StringsAfterSplit = StringForSplit.Split(New String() {"."c}, StringSplitOptions.RemoveEmptyEntries)
				If StringsAfterSplit(1).Length <> 2 Then
					ShowMsgDirect("CF-RegPer needs to have exactly 2 digits after the decimal point!", tMsgID.Err)
					Result = False
				End If
			Else
				ShowMsgDirect("CF-RegPer does not contain a valid decimal separator!", tMsgID.Err)
				Result = False
			End If
		End If


		If Not IsNumeric(Me.TbFCspecCold.Text) Then
			ShowMsgDirect("Specific FC of WHTC coldstart is not valid!", tMsgID.Err)
			Result = False
		Else
			StringForSplit = Me.TbFCspecCold.Text
			If StringForSplit.Contains(".") Then
				StringsAfterSplit = StringForSplit.Split(New String() {"."c}, StringSplitOptions.RemoveEmptyEntries)
				If StringsAfterSplit(1).Length <> 2 Then
					ShowMsgDirect("Specific FC of WHTC coldstart needs to have exactly 2 digits after the decimal point!", tMsgID.Err)
					Result = False
				End If
			Else
				ShowMsgDirect("Specific FC of WHTC coldstart does not contain a valid decimal separator!", tMsgID.Err)
				Result = False
			End If
		End If


		If Not IsNumeric(Me.TbFCspecHot.Text) Then
			ShowMsgDirect("Specific FC of WHTC hotstart is not valid!", tMsgID.Err)
			Result = False
		Else
			StringForSplit = Me.TbFCspecHot.Text
			If StringForSplit.Contains(".") Then
				StringsAfterSplit = StringForSplit.Split(New String() {"."c}, StringSplitOptions.RemoveEmptyEntries)
				If StringsAfterSplit(1).Length <> 2 Then
					ShowMsgDirect("Specific FC of WHTC hotstart needs to have exactly 2 digits after the decimal point!", tMsgID.Err)
					Result = False
				End If
			Else
				ShowMsgDirect("Specific FC of WHTC hotstart does not contain a valid decimal separator!", tMsgID.Err)
				Result = False
			End If
		End If


		If Not IsNumeric(Me.TbFCspecUrb.Text) Then
			ShowMsgDirect("Specific FC of WHTC-Urban is not valid!", tMsgID.Err)
			Result = False
		Else
			StringForSplit = Me.TbFCspecUrb.Text
			If StringForSplit.Contains(".") Then
				StringsAfterSplit = StringForSplit.Split(New String() {"."c}, StringSplitOptions.RemoveEmptyEntries)
				If StringsAfterSplit(1).Length <> 2 Then
					ShowMsgDirect("Specific FC of WHTC-Urban needs to have exactly 2 digits after the decimal point!", tMsgID.Err)
					Result = False
				End If
			Else
				ShowMsgDirect("Specific FC of WHTC-Urban does not contain a valid decimal separator!", tMsgID.Err)
				Result = False
			End If
		End If


		If Not IsNumeric(Me.TbFCspecRur.Text) Then
			ShowMsgDirect("Specific FC of WHTC-Rural is not valid!", tMsgID.Err)
			Result = False
		Else
			StringForSplit = Me.TbFCspecRur.Text
			If StringForSplit.Contains(".") Then
				StringsAfterSplit = StringForSplit.Split(New String() {"."c}, StringSplitOptions.RemoveEmptyEntries)
				If StringsAfterSplit(1).Length <> 2 Then
					ShowMsgDirect("Specific FC of WHTC-Rural needs to have exactly 2 digits after the decimal point!", tMsgID.Err)
					Result = False
				End If
			Else
				ShowMsgDirect("Specific FC of WHTC-Rural does not contain a valid decimal separator!", tMsgID.Err)
				Result = False
			End If
		End If


		If Not IsNumeric(Me.TbFCspecMW.Text) Then
			ShowMsgDirect("Specific FC of WHTC-Motorway is not valid!", tMsgID.Err)
			Result = False
		Else
			StringForSplit = Me.TbFCspecMW.Text
			If StringForSplit.Contains(".") Then
				StringsAfterSplit = StringForSplit.Split(New String() {"."c}, StringSplitOptions.RemoveEmptyEntries)
				If StringsAfterSplit(1).Length <> 2 Then
					ShowMsgDirect("Specific FC of WHTC-Motorway needs to have exactly 2 digits after the decimal point!", tMsgID.Err)
					Result = False
				End If
			Else
				ShowMsgDirect("Specific FC of WHTC-Motorway does not contain a valid decimal separator!", tMsgID.Err)
				Result = False
			End If
		End If


		Return Result
	End Function

	Private Sub ShowMsgDirect(Msg As String, MsgType As tMsgID)
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
		dlog.Filter = "Comma-separated values (*.csv)|*.csv|All files (*.*)|*.*"
		If dlog.ShowDialog() = DialogResult.OK Then Me.TbFuelMap.Text = dlog.FileName
	End Sub

	Private Sub TbFuelMap_TextChanged(sender As Object, e As EventArgs)
		If Me.TbOutputFolder.Text = "" AndAlso Me.TbFuelMap.Text <> "" AndAlso File.Exists(Me.TbFuelMap.Text) Then
			Me.TbOutputFolder.Text = Path.GetDirectoryName(Me.TbFuelMap.Text)
		End If
	End Sub

	Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
		Dim dlog As New OpenFileDialog
		dlog.Filter = "Comma-separated values (*.csv)|*.csv|All files (*.*)|*.*"
		If dlog.ShowDialog() = DialogResult.OK Then Me.TbFLC_Parent.Text = dlog.FileName
	End Sub

	Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
		Dim dlog As New OpenFileDialog
		dlog.Filter = "Comma-separated values (*.csv)|*.csv|All files (*.*)|*.*"
		If dlog.ShowDialog() = DialogResult.OK Then Me.TbFLC.Text = dlog.FileName
	End Sub


	Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
		Dim dlog As New OpenFileDialog
		dlog.Filter = "Comma-separated values (*.csv)|*.csv|All files (*.*)|*.*"
		If dlog.ShowDialog() = DialogResult.OK Then Me.TbMotoring.Text = dlog.FileName
	End Sub


	Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
		Dim dlog As New FolderBrowserDialog
		If dlog.ShowDialog = DialogResult.OK Then Me.TbOutputFolder.Text = dlog.SelectedPath
	End Sub

	'Text-to-number
	Public Function fTextboxToNumString(txt As String) As String
		If Not IsNumeric(txt) Then
			Return "0"
		Else
			Return txt
		End If
	End Function


	' Precalculation of WHTC test speeds and grid for fuel map
	Private Sub BtPrecalc_Click(sender As Object, e As EventArgs) Handles BtPrecalc.Click

		JobPrecalc = New cJobPrecalc
		CalcMode = 1

		Try
			JobInit()

			If Not CheckInputPrecalc() Then
				JobEnd()
				Exit Sub
			End If

			JobPrecalc.FlcParentFile = Me.TbFLC_Parent.Text
			JobPrecalc.Idle_Parent = Me.TbIdle_Parent.Text

		Catch ex As Exception
			ShowMsgDirect(ex.Message, tMsgID.Err)
			Exit Sub
		End Try

		Worker.RunWorkerAsync()
	End Sub


	Private Function CheckInputPrecalc() As Boolean
		Dim Result As Boolean

		Result = True

		'Check if files do exist       
		If Trim(Me.TbFLC_Parent.Text) = "" OrElse Not File.Exists(Me.TbFLC_Parent.Text) Then
			ShowMsgDirect("File for full-Load curve of CO2-parent engine not found!", tMsgID.Err)
			Result = False
		End If

		If Not IsNumeric(Me.TbIdle_Parent.Text) Then
			ShowMsgDirect("Idle speed of CO2-parent engine is not valid!", tMsgID.Err)
			Result = False
		Else
			If Not IsInteger(Me.TbIdle_Parent.Text) Then
				ShowMsgDirect("Idle speed of CO2-parent engine needs to be rounded to the nearest whole number!", tMsgID.Err)
				Result = False
			End If
		End If


		Return Result
	End Function
End Class
