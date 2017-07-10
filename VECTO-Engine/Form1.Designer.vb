<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Das Formular überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Wird vom Windows Form-Designer benötigt.
    Private components As System.ComponentModel.IContainer

    'Hinweis: Die folgende Prozedur ist für den Windows Form-Designer erforderlich.
    'Das Bearbeiten ist mit dem Windows Form-Designer möglich.  
    'Das Bearbeiten mit dem Code-Editor ist nicht möglich.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
		Me.Label3 = New System.Windows.Forms.Label()
		Me.TbFCspecCold = New System.Windows.Forms.TextBox()
		Me.Label4 = New System.Windows.Forms.Label()
		Me.Label5 = New System.Windows.Forms.Label()
		Me.TbFCspecMW = New System.Windows.Forms.TextBox()
		Me.Label6 = New System.Windows.Forms.Label()
		Me.Label7 = New System.Windows.Forms.Label()
		Me.TbFCspecRur = New System.Windows.Forms.TextBox()
		Me.Label8 = New System.Windows.Forms.Label()
		Me.Label9 = New System.Windows.Forms.Label()
		Me.TbFCspecUrb = New System.Windows.Forms.TextBox()
		Me.Label10 = New System.Windows.Forms.Label()
		Me.GrInput = New System.Windows.Forms.GroupBox()
		Me.GroupBox3 = New System.Windows.Forms.GroupBox()
		Me.Label31 = New System.Windows.Forms.Label()
		Me.Label32 = New System.Windows.Forms.Label()
		Me.TbRatedSpeed = New System.Windows.Forms.TextBox()
		Me.Label23 = New System.Windows.Forms.Label()
		Me.Label27 = New System.Windows.Forms.Label()
		Me.TbRatedPower = New System.Windows.Forms.TextBox()
		Me.Label22 = New System.Windows.Forms.Label()
		Me.TbCertNumber = New System.Windows.Forms.TextBox()
		Me.Label15 = New System.Windows.Forms.Label()
		Me.TbModel = New System.Windows.Forms.TextBox()
		Me.Label21 = New System.Windows.Forms.Label()
		Me.TbManufacturer = New System.Windows.Forms.TextBox()
		Me.CbFuelType = New System.Windows.Forms.ComboBox()
		Me.Label29 = New System.Windows.Forms.Label()
		Me.Label30 = New System.Windows.Forms.Label()
		Me.TbNCVfuel = New System.Windows.Forms.TextBox()
		Me.Label28 = New System.Windows.Forms.Label()
		Me.Label25 = New System.Windows.Forms.Label()
		Me.Label26 = New System.Windows.Forms.Label()
		Me.TbDisplacement = New System.Windows.Forms.TextBox()
		Me.Label18 = New System.Windows.Forms.Label()
		Me.Label24 = New System.Windows.Forms.Label()
		Me.TbIdle = New System.Windows.Forms.TextBox()
		Me.Label13 = New System.Windows.Forms.Label()
		Me.Label16 = New System.Windows.Forms.Label()
		Me.TbIdle_Parent = New System.Windows.Forms.TextBox()
		Me.GroupBox2 = New System.Windows.Forms.GroupBox()
		Me.Label11 = New System.Windows.Forms.Label()
		Me.Label12 = New System.Windows.Forms.Label()
		Me.Label2 = New System.Windows.Forms.Label()
		Me.Label1 = New System.Windows.Forms.Label()
		Me.TbFLC = New System.Windows.Forms.TextBox()
		Me.Button2 = New System.Windows.Forms.Button()
		Me.TbMotoring = New System.Windows.Forms.TextBox()
		Me.Button3 = New System.Windows.Forms.Button()
		Me.TbFLC_Parent = New System.Windows.Forms.TextBox()
		Me.Button1 = New System.Windows.Forms.Button()
		Me.TbFuelMap = New System.Windows.Forms.TextBox()
		Me.BtOpenMap = New System.Windows.Forms.Button()
		Me.GroupBox1 = New System.Windows.Forms.GroupBox()
		Me.Label17 = New System.Windows.Forms.Label()
		Me.TbCF_RegPer = New System.Windows.Forms.TextBox()
		Me.PictureBox1 = New System.Windows.Forms.PictureBox()
		Me.GrRpm = New System.Windows.Forms.GroupBox()
		Me.TbFCspecHot = New System.Windows.Forms.TextBox()
		Me.Label20 = New System.Windows.Forms.Label()
		Me.Label19 = New System.Windows.Forms.Label()
		Me.GrOutput = New System.Windows.Forms.GroupBox()
		Me.LvMsg = New System.Windows.Forms.ListView()
		Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
		Me.TbOutputFolder = New System.Windows.Forms.TextBox()
		Me.Button5 = New System.Windows.Forms.Button()
		Me.Label14 = New System.Windows.Forms.Label()
		Me.BtStart = New System.Windows.Forms.Button()
		Me.BgWorker = New System.ComponentModel.BackgroundWorker()
		Me.BtPrecalc = New System.Windows.Forms.Button()
		Me.GrInput.SuspendLayout()
		Me.GroupBox3.SuspendLayout()
		Me.GroupBox2.SuspendLayout()
		Me.GroupBox1.SuspendLayout()
		CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.GrRpm.SuspendLayout()
		Me.GrOutput.SuspendLayout()
		Me.SuspendLayout()
		'
		'Label3
		'
		Me.Label3.AutoSize = True
		Me.Label3.Location = New System.Drawing.Point(8, 37)
		Me.Label3.Name = "Label3"
		Me.Label3.Size = New System.Drawing.Size(106, 13)
		Me.Label3.TabIndex = 0
		Me.Label3.Text = "WHTC coldstart total"
		'
		'TbFCspecCold
		'
		Me.TbFCspecCold.Location = New System.Drawing.Point(120, 34)
		Me.TbFCspecCold.Name = "TbFCspecCold"
		Me.TbFCspecCold.Size = New System.Drawing.Size(91, 20)
		Me.TbFCspecCold.TabIndex = 0
		'
		'Label4
		'
		Me.Label4.AutoSize = True
		Me.Label4.Location = New System.Drawing.Point(217, 37)
		Me.Label4.Name = "Label4"
		Me.Label4.Size = New System.Drawing.Size(47, 13)
		Me.Label4.TabIndex = 2
		Me.Label4.Text = "[g/kWh]"
		'
		'Label5
		'
		Me.Label5.AutoSize = True
		Me.Label5.Location = New System.Drawing.Point(374, 78)
		Me.Label5.Name = "Label5"
		Me.Label5.Size = New System.Drawing.Size(89, 13)
		Me.Label5.TabIndex = 0
		Me.Label5.Text = "WHTC-Motorway"
		'
		'TbFCspecMW
		'
		Me.TbFCspecMW.Location = New System.Drawing.Point(469, 75)
		Me.TbFCspecMW.Name = "TbFCspecMW"
		Me.TbFCspecMW.Size = New System.Drawing.Size(91, 20)
		Me.TbFCspecMW.TabIndex = 4
		'
		'Label6
		'
		Me.Label6.AutoSize = True
		Me.Label6.Location = New System.Drawing.Point(566, 78)
		Me.Label6.Name = "Label6"
		Me.Label6.Size = New System.Drawing.Size(47, 13)
		Me.Label6.TabIndex = 2
		Me.Label6.Text = "[g/kWh]"
		'
		'Label7
		'
		Me.Label7.AutoSize = True
		Me.Label7.Location = New System.Drawing.Point(395, 50)
		Me.Label7.Name = "Label7"
		Me.Label7.Size = New System.Drawing.Size(68, 13)
		Me.Label7.TabIndex = 0
		Me.Label7.Text = "WHTC-Rural"
		'
		'TbFCspecRur
		'
		Me.TbFCspecRur.Location = New System.Drawing.Point(469, 47)
		Me.TbFCspecRur.Name = "TbFCspecRur"
		Me.TbFCspecRur.Size = New System.Drawing.Size(91, 20)
		Me.TbFCspecRur.TabIndex = 3
		'
		'Label8
		'
		Me.Label8.AutoSize = True
		Me.Label8.Location = New System.Drawing.Point(566, 50)
		Me.Label8.Name = "Label8"
		Me.Label8.Size = New System.Drawing.Size(47, 13)
		Me.Label8.TabIndex = 2
		Me.Label8.Text = "[g/kWh]"
		'
		'Label9
		'
		Me.Label9.AutoSize = True
		Me.Label9.Location = New System.Drawing.Point(391, 23)
		Me.Label9.Name = "Label9"
		Me.Label9.Size = New System.Drawing.Size(72, 13)
		Me.Label9.TabIndex = 0
		Me.Label9.Text = "WHTC-Urban"
		'
		'TbFCspecUrb
		'
		Me.TbFCspecUrb.Location = New System.Drawing.Point(469, 19)
		Me.TbFCspecUrb.Name = "TbFCspecUrb"
		Me.TbFCspecUrb.Size = New System.Drawing.Size(91, 20)
		Me.TbFCspecUrb.TabIndex = 2
		'
		'Label10
		'
		Me.Label10.AutoSize = True
		Me.Label10.Location = New System.Drawing.Point(566, 22)
		Me.Label10.Name = "Label10"
		Me.Label10.Size = New System.Drawing.Size(47, 13)
		Me.Label10.TabIndex = 2
		Me.Label10.Text = "[g/kWh]"
		'
		'GrInput
		'
		Me.GrInput.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
		Me.GrInput.Controls.Add(Me.GroupBox3)
		Me.GrInput.Controls.Add(Me.GroupBox2)
		Me.GrInput.Controls.Add(Me.GroupBox1)
		Me.GrInput.Controls.Add(Me.PictureBox1)
		Me.GrInput.Controls.Add(Me.GrRpm)
		Me.GrInput.Location = New System.Drawing.Point(12, 12)
		Me.GrInput.Name = "GrInput"
		Me.GrInput.Size = New System.Drawing.Size(941, 549)
		Me.GrInput.TabIndex = 0
		Me.GrInput.TabStop = False
		Me.GrInput.Text = "Input"
		'
		'GroupBox3
		'
		Me.GroupBox3.Controls.Add(Me.Label31)
		Me.GroupBox3.Controls.Add(Me.Label32)
		Me.GroupBox3.Controls.Add(Me.TbRatedSpeed)
		Me.GroupBox3.Controls.Add(Me.Label23)
		Me.GroupBox3.Controls.Add(Me.Label27)
		Me.GroupBox3.Controls.Add(Me.TbRatedPower)
		Me.GroupBox3.Controls.Add(Me.Label22)
		Me.GroupBox3.Controls.Add(Me.TbCertNumber)
		Me.GroupBox3.Controls.Add(Me.Label15)
		Me.GroupBox3.Controls.Add(Me.TbModel)
		Me.GroupBox3.Controls.Add(Me.Label21)
		Me.GroupBox3.Controls.Add(Me.TbManufacturer)
		Me.GroupBox3.Controls.Add(Me.CbFuelType)
		Me.GroupBox3.Controls.Add(Me.Label29)
		Me.GroupBox3.Controls.Add(Me.Label30)
		Me.GroupBox3.Controls.Add(Me.TbNCVfuel)
		Me.GroupBox3.Controls.Add(Me.Label28)
		Me.GroupBox3.Controls.Add(Me.Label25)
		Me.GroupBox3.Controls.Add(Me.Label26)
		Me.GroupBox3.Controls.Add(Me.TbDisplacement)
		Me.GroupBox3.Controls.Add(Me.Label18)
		Me.GroupBox3.Controls.Add(Me.Label24)
		Me.GroupBox3.Controls.Add(Me.TbIdle)
		Me.GroupBox3.Controls.Add(Me.Label13)
		Me.GroupBox3.Controls.Add(Me.Label16)
		Me.GroupBox3.Controls.Add(Me.TbIdle_Parent)
		Me.GroupBox3.Location = New System.Drawing.Point(6, 30)
		Me.GroupBox3.Name = "GroupBox3"
		Me.GroupBox3.Size = New System.Drawing.Size(624, 259)
		Me.GroupBox3.TabIndex = 0
		Me.GroupBox3.TabStop = False
		Me.GroupBox3.Text = "Component data"
		'
		'Label31
		'
		Me.Label31.AutoSize = True
		Me.Label31.Location = New System.Drawing.Point(278, 229)
		Me.Label31.Name = "Label31"
		Me.Label31.Size = New System.Drawing.Size(40, 13)
		Me.Label31.TabIndex = 30
		Me.Label31.Text = "[1/min]"
		'
		'Label32
		'
		Me.Label32.AutoSize = True
		Me.Label32.Location = New System.Drawing.Point(73, 229)
		Me.Label32.Name = "Label32"
		Me.Label32.Size = New System.Drawing.Size(99, 13)
		Me.Label32.TabIndex = 29
		Me.Label32.Text = "Engine rated speed"
		'
		'TbRatedSpeed
		'
		Me.TbRatedSpeed.Location = New System.Drawing.Point(181, 226)
		Me.TbRatedSpeed.Name = "TbRatedSpeed"
		Me.TbRatedSpeed.Size = New System.Drawing.Size(91, 20)
		Me.TbRatedSpeed.TabIndex = 7
		'
		'Label23
		'
		Me.Label23.AutoSize = True
		Me.Label23.Location = New System.Drawing.Point(278, 203)
		Me.Label23.Name = "Label23"
		Me.Label23.Size = New System.Drawing.Size(30, 13)
		Me.Label23.TabIndex = 27
		Me.Label23.Text = "[kW]"
		'
		'Label27
		'
		Me.Label27.AutoSize = True
		Me.Label27.Location = New System.Drawing.Point(73, 203)
		Me.Label27.Name = "Label27"
		Me.Label27.Size = New System.Drawing.Size(99, 13)
		Me.Label27.TabIndex = 26
		Me.Label27.Text = "Engine rated power"
		'
		'TbRatedPower
		'
		Me.TbRatedPower.Location = New System.Drawing.Point(181, 200)
		Me.TbRatedPower.Name = "TbRatedPower"
		Me.TbRatedPower.Size = New System.Drawing.Size(91, 20)
		Me.TbRatedPower.TabIndex = 6
		'
		'Label22
		'
		Me.Label22.AutoSize = True
		Me.Label22.Location = New System.Drawing.Point(12, 82)
		Me.Label22.Name = "Label22"
		Me.Label22.Size = New System.Drawing.Size(102, 13)
		Me.Label22.TabIndex = 24
		Me.Label22.Text = "Certification Number"
		'
		'TbCertNumber
		'
		Me.TbCertNumber.Location = New System.Drawing.Point(120, 79)
		Me.TbCertNumber.Name = "TbCertNumber"
		Me.TbCertNumber.Size = New System.Drawing.Size(495, 20)
		Me.TbCertNumber.TabIndex = 2
		'
		'Label15
		'
		Me.Label15.AutoSize = True
		Me.Label15.Location = New System.Drawing.Point(78, 56)
		Me.Label15.Name = "Label15"
		Me.Label15.Size = New System.Drawing.Size(36, 13)
		Me.Label15.TabIndex = 15
		Me.Label15.Text = "Model"
		'
		'TbModel
		'
		Me.TbModel.Location = New System.Drawing.Point(120, 53)
		Me.TbModel.Name = "TbModel"
		Me.TbModel.Size = New System.Drawing.Size(495, 20)
		Me.TbModel.TabIndex = 1
		'
		'Label21
		'
		Me.Label21.AutoSize = True
		Me.Label21.Location = New System.Drawing.Point(44, 30)
		Me.Label21.Name = "Label21"
		Me.Label21.Size = New System.Drawing.Size(70, 13)
		Me.Label21.TabIndex = 8
		Me.Label21.Text = "Manufacturer"
		'
		'TbManufacturer
		'
		Me.TbManufacturer.Location = New System.Drawing.Point(120, 27)
		Me.TbManufacturer.Name = "TbManufacturer"
		Me.TbManufacturer.Size = New System.Drawing.Size(495, 20)
		Me.TbManufacturer.TabIndex = 0
		'
		'CbFuelType
		'
		Me.CbFuelType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.CbFuelType.FormattingEnabled = True
		Me.CbFuelType.Items.AddRange(New Object() {"Diesel / CI", "Ethanol / CI", "Petrol / PI", "Ethanol / PI", "LPG / PI", "Natural Gas / PI"})
		Me.CbFuelType.Location = New System.Drawing.Point(474, 120)
		Me.CbFuelType.Name = "CbFuelType"
		Me.CbFuelType.Size = New System.Drawing.Size(141, 21)
		Me.CbFuelType.TabIndex = 8
		'
		'Label29
		'
		Me.Label29.AutoSize = True
		Me.Label29.Location = New System.Drawing.Point(571, 151)
		Me.Label29.Name = "Label29"
		Me.Label29.Size = New System.Drawing.Size(44, 13)
		Me.Label29.TabIndex = 16
		Me.Label29.Text = "[MJ/kg]"
		'
		'Label30
		'
		Me.Label30.AutoSize = True
		Me.Label30.Location = New System.Drawing.Point(384, 151)
		Me.Label30.Name = "Label30"
		Me.Label30.Size = New System.Drawing.Size(81, 13)
		Me.Label30.TabIndex = 15
		Me.Label30.Text = "NCV of test fuel"
		'
		'TbNCVfuel
		'
		Me.TbNCVfuel.Location = New System.Drawing.Point(474, 148)
		Me.TbNCVfuel.Name = "TbNCVfuel"
		Me.TbNCVfuel.Size = New System.Drawing.Size(91, 20)
		Me.TbNCVfuel.TabIndex = 9
		'
		'Label28
		'
		Me.Label28.AutoSize = True
		Me.Label28.Location = New System.Drawing.Point(382, 125)
		Me.Label28.Name = "Label28"
		Me.Label28.Size = New System.Drawing.Size(83, 13)
		Me.Label28.TabIndex = 12
		Me.Label28.Text = "Type of test fuel"
		'
		'Label25
		'
		Me.Label25.AutoSize = True
		Me.Label25.Location = New System.Drawing.Point(278, 177)
		Me.Label25.Name = "Label25"
		Me.Label25.Size = New System.Drawing.Size(33, 13)
		Me.Label25.TabIndex = 10
		Me.Label25.Text = "[ccm]"
		'
		'Label26
		'
		Me.Label26.AutoSize = True
		Me.Label26.Location = New System.Drawing.Point(67, 177)
		Me.Label26.Name = "Label26"
		Me.Label26.Size = New System.Drawing.Size(105, 13)
		Me.Label26.TabIndex = 9
		Me.Label26.Text = "Engine displacement"
		'
		'TbDisplacement
		'
		Me.TbDisplacement.Location = New System.Drawing.Point(181, 174)
		Me.TbDisplacement.Name = "TbDisplacement"
		Me.TbDisplacement.Size = New System.Drawing.Size(91, 20)
		Me.TbDisplacement.TabIndex = 5
		'
		'Label18
		'
		Me.Label18.AutoSize = True
		Me.Label18.Location = New System.Drawing.Point(278, 151)
		Me.Label18.Name = "Label18"
		Me.Label18.Size = New System.Drawing.Size(40, 13)
		Me.Label18.TabIndex = 7
		Me.Label18.Text = "[1/min]"
		'
		'Label24
		'
		Me.Label24.AutoSize = True
		Me.Label24.Location = New System.Drawing.Point(81, 151)
		Me.Label24.Name = "Label24"
		Me.Label24.Size = New System.Drawing.Size(91, 13)
		Me.Label24.TabIndex = 6
		Me.Label24.Text = "Engine idle speed"
		'
		'TbIdle
		'
		Me.TbIdle.Location = New System.Drawing.Point(181, 148)
		Me.TbIdle.Name = "TbIdle"
		Me.TbIdle.Size = New System.Drawing.Size(91, 20)
		Me.TbIdle.TabIndex = 4
		'
		'Label13
		'
		Me.Label13.AutoSize = True
		Me.Label13.Location = New System.Drawing.Point(278, 124)
		Me.Label13.Name = "Label13"
		Me.Label13.Size = New System.Drawing.Size(40, 13)
		Me.Label13.TabIndex = 0
		Me.Label13.Text = "[1/min]"
		'
		'Label16
		'
		Me.Label16.AutoSize = True
		Me.Label16.Location = New System.Drawing.Point(12, 125)
		Me.Label16.Name = "Label16"
		Me.Label16.Size = New System.Drawing.Size(160, 13)
		Me.Label16.TabIndex = 3
		Me.Label16.Text = "Idle speed of CO2-parent engine"
		'
		'TbIdle_Parent
		'
		Me.TbIdle_Parent.Location = New System.Drawing.Point(181, 121)
		Me.TbIdle_Parent.Name = "TbIdle_Parent"
		Me.TbIdle_Parent.Size = New System.Drawing.Size(91, 20)
		Me.TbIdle_Parent.TabIndex = 3
		'
		'GroupBox2
		'
		Me.GroupBox2.Controls.Add(Me.Label11)
		Me.GroupBox2.Controls.Add(Me.Label12)
		Me.GroupBox2.Controls.Add(Me.Label2)
		Me.GroupBox2.Controls.Add(Me.Label1)
		Me.GroupBox2.Controls.Add(Me.TbFLC)
		Me.GroupBox2.Controls.Add(Me.Button2)
		Me.GroupBox2.Controls.Add(Me.TbMotoring)
		Me.GroupBox2.Controls.Add(Me.Button3)
		Me.GroupBox2.Controls.Add(Me.TbFLC_Parent)
		Me.GroupBox2.Controls.Add(Me.Button1)
		Me.GroupBox2.Controls.Add(Me.TbFuelMap)
		Me.GroupBox2.Controls.Add(Me.BtOpenMap)
		Me.GroupBox2.Location = New System.Drawing.Point(6, 296)
		Me.GroupBox2.Name = "GroupBox2"
		Me.GroupBox2.Size = New System.Drawing.Size(924, 127)
		Me.GroupBox2.TabIndex = 1
		Me.GroupBox2.TabStop = False
		Me.GroupBox2.Text = "Data files"
		'
		'Label11
		'
		Me.Label11.AutoSize = True
		Me.Label11.Location = New System.Drawing.Point(153, 76)
		Me.Label11.Name = "Label11"
		Me.Label11.Size = New System.Drawing.Size(76, 13)
		Me.Label11.TabIndex = 10
		Me.Label11.Text = "Full-load curve"
		'
		'Label12
		'
		Me.Label12.AutoSize = True
		Me.Label12.Location = New System.Drawing.Point(18, 102)
		Me.Label12.Name = "Label12"
		Me.Label12.Size = New System.Drawing.Size(212, 13)
		Me.Label12.TabIndex = 11
		Me.Label12.Text = "Motoring curve curve of CO2-parent engine"
		'
		'Label2
		'
		Me.Label2.AutoSize = True
		Me.Label2.Location = New System.Drawing.Point(49, 50)
		Me.Label2.Name = "Label2"
		Me.Label2.Size = New System.Drawing.Size(180, 13)
		Me.Label2.TabIndex = 12
		Me.Label2.Text = "Full-load curve of CO2-parent engine"
		'
		'Label1
		'
		Me.Label1.AutoSize = True
		Me.Label1.Location = New System.Drawing.Point(12, 24)
		Me.Label1.Name = "Label1"
		Me.Label1.Size = New System.Drawing.Size(217, 13)
		Me.Label1.TabIndex = 13
		Me.Label1.Text = "Fuel consumption map of CO2-parent engine"
		'
		'TbFLC
		'
		Me.TbFLC.Location = New System.Drawing.Point(236, 73)
		Me.TbFLC.Name = "TbFLC"
		Me.TbFLC.Size = New System.Drawing.Size(644, 20)
		Me.TbFLC.TabIndex = 2
		'
		'Button2
		'
		Me.Button2.Image = Global.VECTO_Engine.My.Resources.Resources.Open_icon
		Me.Button2.Location = New System.Drawing.Point(884, 71)
		Me.Button2.Name = "Button2"
		Me.Button2.Size = New System.Drawing.Size(28, 23)
		Me.Button2.TabIndex = 19
		Me.Button2.TabStop = False
		Me.Button2.UseVisualStyleBackColor = True
		'
		'TbMotoring
		'
		Me.TbMotoring.Location = New System.Drawing.Point(236, 99)
		Me.TbMotoring.Name = "TbMotoring"
		Me.TbMotoring.Size = New System.Drawing.Size(644, 20)
		Me.TbMotoring.TabIndex = 3
		'
		'Button3
		'
		Me.Button3.Image = Global.VECTO_Engine.My.Resources.Resources.Open_icon
		Me.Button3.Location = New System.Drawing.Point(884, 96)
		Me.Button3.Name = "Button3"
		Me.Button3.Size = New System.Drawing.Size(28, 23)
		Me.Button3.TabIndex = 21
		Me.Button3.TabStop = False
		Me.Button3.UseVisualStyleBackColor = True
		'
		'TbFLC_Parent
		'
		Me.TbFLC_Parent.Location = New System.Drawing.Point(236, 47)
		Me.TbFLC_Parent.Name = "TbFLC_Parent"
		Me.TbFLC_Parent.Size = New System.Drawing.Size(644, 20)
		Me.TbFLC_Parent.TabIndex = 1
		'
		'Button1
		'
		Me.Button1.Image = Global.VECTO_Engine.My.Resources.Resources.Open_icon
		Me.Button1.Location = New System.Drawing.Point(884, 45)
		Me.Button1.Name = "Button1"
		Me.Button1.Size = New System.Drawing.Size(28, 23)
		Me.Button1.TabIndex = 17
		Me.Button1.TabStop = False
		Me.Button1.UseVisualStyleBackColor = True
		'
		'TbFuelMap
		'
		Me.TbFuelMap.Location = New System.Drawing.Point(236, 21)
		Me.TbFuelMap.Name = "TbFuelMap"
		Me.TbFuelMap.Size = New System.Drawing.Size(644, 20)
		Me.TbFuelMap.TabIndex = 0
		'
		'BtOpenMap
		'
		Me.BtOpenMap.Image = Global.VECTO_Engine.My.Resources.Resources.Open_icon
		Me.BtOpenMap.Location = New System.Drawing.Point(884, 19)
		Me.BtOpenMap.Name = "BtOpenMap"
		Me.BtOpenMap.Size = New System.Drawing.Size(28, 23)
		Me.BtOpenMap.TabIndex = 15
		Me.BtOpenMap.TabStop = False
		Me.BtOpenMap.UseVisualStyleBackColor = True
		'
		'GroupBox1
		'
		Me.GroupBox1.Controls.Add(Me.Label17)
		Me.GroupBox1.Controls.Add(Me.TbCF_RegPer)
		Me.GroupBox1.Location = New System.Drawing.Point(647, 434)
		Me.GroupBox1.Name = "GroupBox1"
		Me.GroupBox1.Size = New System.Drawing.Size(284, 105)
		Me.GroupBox1.TabIndex = 3
		Me.GroupBox1.TabStop = False
		Me.GroupBox1.Text = "Correction factors"
		'
		'Label17
		'
		Me.Label17.AutoSize = True
		Me.Label17.Location = New System.Drawing.Point(6, 50)
		Me.Label17.Name = "Label17"
		Me.Label17.Size = New System.Drawing.Size(59, 13)
		Me.Label17.TabIndex = 3
		Me.Label17.Text = "CF-RegPer"
		'
		'TbCF_RegPer
		'
		Me.TbCF_RegPer.Location = New System.Drawing.Point(71, 47)
		Me.TbCF_RegPer.Name = "TbCF_RegPer"
		Me.TbCF_RegPer.Size = New System.Drawing.Size(111, 20)
		Me.TbCF_RegPer.TabIndex = 0
		'
		'PictureBox1
		'
		Me.PictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
		Me.PictureBox1.Image = Global.VECTO_Engine.My.Resources.Resources.VectoEngine_6_cropV164pix
		Me.PictureBox1.Location = New System.Drawing.Point(647, 37)
		Me.PictureBox1.Name = "PictureBox1"
		Me.PictureBox1.Size = New System.Drawing.Size(280, 164)
		Me.PictureBox1.TabIndex = 11
		Me.PictureBox1.TabStop = False
		'
		'GrRpm
		'
		Me.GrRpm.Controls.Add(Me.TbFCspecHot)
		Me.GrRpm.Controls.Add(Me.TbFCspecCold)
		Me.GrRpm.Controls.Add(Me.Label7)
		Me.GrRpm.Controls.Add(Me.Label10)
		Me.GrRpm.Controls.Add(Me.TbFCspecMW)
		Me.GrRpm.Controls.Add(Me.TbFCspecRur)
		Me.GrRpm.Controls.Add(Me.Label20)
		Me.GrRpm.Controls.Add(Me.Label8)
		Me.GrRpm.Controls.Add(Me.Label3)
		Me.GrRpm.Controls.Add(Me.Label9)
		Me.GrRpm.Controls.Add(Me.Label6)
		Me.GrRpm.Controls.Add(Me.Label5)
		Me.GrRpm.Controls.Add(Me.Label19)
		Me.GrRpm.Controls.Add(Me.TbFCspecUrb)
		Me.GrRpm.Controls.Add(Me.Label4)
		Me.GrRpm.Location = New System.Drawing.Point(6, 434)
		Me.GrRpm.Name = "GrRpm"
		Me.GrRpm.Size = New System.Drawing.Size(624, 105)
		Me.GrRpm.TabIndex = 2
		Me.GrRpm.TabStop = False
		Me.GrRpm.Text = "Specific fuel consumption measured"
		'
		'TbFCspecHot
		'
		Me.TbFCspecHot.Location = New System.Drawing.Point(120, 75)
		Me.TbFCspecHot.Name = "TbFCspecHot"
		Me.TbFCspecHot.Size = New System.Drawing.Size(91, 20)
		Me.TbFCspecHot.TabIndex = 1
		'
		'Label20
		'
		Me.Label20.AutoSize = True
		Me.Label20.Location = New System.Drawing.Point(13, 78)
		Me.Label20.Name = "Label20"
		Me.Label20.Size = New System.Drawing.Size(101, 13)
		Me.Label20.TabIndex = 0
		Me.Label20.Text = "WHTC hotstart total"
		'
		'Label19
		'
		Me.Label19.AutoSize = True
		Me.Label19.Location = New System.Drawing.Point(217, 78)
		Me.Label19.Name = "Label19"
		Me.Label19.Size = New System.Drawing.Size(47, 13)
		Me.Label19.TabIndex = 2
		Me.Label19.Text = "[g/kWh]"
		'
		'GrOutput
		'
		Me.GrOutput.Controls.Add(Me.LvMsg)
		Me.GrOutput.Controls.Add(Me.TbOutputFolder)
		Me.GrOutput.Controls.Add(Me.Button5)
		Me.GrOutput.Controls.Add(Me.Label14)
		Me.GrOutput.Location = New System.Drawing.Point(12, 634)
		Me.GrOutput.Name = "GrOutput"
		Me.GrOutput.Size = New System.Drawing.Size(941, 244)
		Me.GrOutput.TabIndex = 1
		Me.GrOutput.TabStop = False
		Me.GrOutput.Text = "Output"
		'
		'LvMsg
		'
		Me.LvMsg.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1})
		Me.LvMsg.FullRowSelect = True
		Me.LvMsg.GridLines = True
		Me.LvMsg.LabelWrap = False
		Me.LvMsg.Location = New System.Drawing.Point(6, 52)
		Me.LvMsg.Name = "LvMsg"
		Me.LvMsg.Size = New System.Drawing.Size(925, 182)
		Me.LvMsg.TabIndex = 2
		Me.LvMsg.UseCompatibleStateImageBehavior = False
		Me.LvMsg.View = System.Windows.Forms.View.Details
		'
		'ColumnHeader1
		'
		Me.ColumnHeader1.Text = "Message"
		Me.ColumnHeader1.Width = 758
		'
		'TbOutputFolder
		'
		Me.TbOutputFolder.Location = New System.Drawing.Point(147, 19)
		Me.TbOutputFolder.Name = "TbOutputFolder"
		Me.TbOutputFolder.Size = New System.Drawing.Size(750, 20)
		Me.TbOutputFolder.TabIndex = 0
		'
		'Button5
		'
		Me.Button5.Image = Global.VECTO_Engine.My.Resources.Resources.Open_icon
		Me.Button5.Location = New System.Drawing.Point(903, 17)
		Me.Button5.Name = "Button5"
		Me.Button5.Size = New System.Drawing.Size(28, 23)
		Me.Button5.TabIndex = 1
		Me.Button5.TabStop = False
		Me.Button5.UseVisualStyleBackColor = True
		'
		'Label14
		'
		Me.Label14.AutoSize = True
		Me.Label14.Location = New System.Drawing.Point(57, 22)
		Me.Label14.Name = "Label14"
		Me.Label14.Size = New System.Drawing.Size(84, 13)
		Me.Label14.TabIndex = 0
		Me.Label14.Text = "Output Directory"
		'
		'BtStart
		'
		Me.BtStart.BackColor = System.Drawing.Color.Lime
		Me.BtStart.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
		Me.BtStart.FlatAppearance.BorderColor = System.Drawing.Color.Black
		Me.BtStart.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.BtStart.Location = New System.Drawing.Point(12, 575)
		Me.BtStart.Name = "BtStart"
		Me.BtStart.Size = New System.Drawing.Size(630, 39)
		Me.BtStart.TabIndex = 3
		Me.BtStart.TabStop = False
		Me.BtStart.Text = "START FULL DATA EVALUATION"
		Me.BtStart.UseVisualStyleBackColor = False
		'
		'BgWorker
		'
		Me.BgWorker.WorkerReportsProgress = True
		Me.BgWorker.WorkerSupportsCancellation = True
		'
		'BtPrecalc
		'
		Me.BtPrecalc.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.BtPrecalc.Location = New System.Drawing.Point(659, 575)
		Me.BtPrecalc.Name = "BtPrecalc"
		Me.BtPrecalc.Size = New System.Drawing.Size(284, 39)
		Me.BtPrecalc.TabIndex = 3
		Me.BtPrecalc.TabStop = False
		Me.BtPrecalc.Text = "Precalculate characteristic engine speeds" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "and grid for fuel map"
		Me.BtPrecalc.UseVisualStyleBackColor = True
		'
		'Form1
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(964, 887)
		Me.Controls.Add(Me.BtPrecalc)
		Me.Controls.Add(Me.GrOutput)
		Me.Controls.Add(Me.BtStart)
		Me.Controls.Add(Me.GrInput)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
		Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.MaximizeBox = False
		Me.Name = "Form1"
		Me.Text = "Form1"
		Me.GrInput.ResumeLayout(False)
		Me.GroupBox3.ResumeLayout(False)
		Me.GroupBox3.PerformLayout()
		Me.GroupBox2.ResumeLayout(False)
		Me.GroupBox2.PerformLayout()
		Me.GroupBox1.ResumeLayout(False)
		Me.GroupBox1.PerformLayout()
		CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.GrRpm.ResumeLayout(False)
		Me.GrRpm.PerformLayout()
		Me.GrOutput.ResumeLayout(False)
		Me.GrOutput.PerformLayout()
		Me.ResumeLayout(False)

	End Sub
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents TbFCspecCold As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents TbFCspecMW As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents TbFCspecRur As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents TbFCspecUrb As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents GrInput As System.Windows.Forms.GroupBox
    Friend WithEvents GrRpm As System.Windows.Forms.GroupBox
    Friend WithEvents TbFCspecHot As System.Windows.Forms.TextBox
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents GrOutput As System.Windows.Forms.GroupBox
    Friend WithEvents BtStart As System.Windows.Forms.Button
    Friend WithEvents BgWorker As System.ComponentModel.BackgroundWorker
    Friend WithEvents LvMsg As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents TbOutputFolder As System.Windows.Forms.TextBox
    Friend WithEvents Button5 As System.Windows.Forms.Button
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents BtPrecalc As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents TbIdle_Parent As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TbFLC As System.Windows.Forms.TextBox
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents TbMotoring As System.Windows.Forms.TextBox
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents TbFLC_Parent As System.Windows.Forms.TextBox
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents TbFuelMap As System.Windows.Forms.TextBox
    Friend WithEvents BtOpenMap As System.Windows.Forms.Button
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents TbCF_RegPer As System.Windows.Forms.TextBox
    Friend WithEvents CbFuelType As System.Windows.Forms.ComboBox
    Friend WithEvents Label29 As System.Windows.Forms.Label
    Friend WithEvents Label30 As System.Windows.Forms.Label
    Friend WithEvents TbNCVfuel As System.Windows.Forms.TextBox
    Friend WithEvents Label28 As System.Windows.Forms.Label
    Friend WithEvents Label25 As System.Windows.Forms.Label
    Friend WithEvents Label26 As System.Windows.Forms.Label
    Friend WithEvents TbDisplacement As System.Windows.Forms.TextBox
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents Label24 As System.Windows.Forms.Label
    Friend WithEvents TbIdle As System.Windows.Forms.TextBox
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents TbCertNumber As System.Windows.Forms.TextBox
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents TbModel As System.Windows.Forms.TextBox
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents TbManufacturer As System.Windows.Forms.TextBox
    Friend WithEvents Label31 As System.Windows.Forms.Label
    Friend WithEvents Label32 As System.Windows.Forms.Label
    Friend WithEvents TbRatedSpeed As System.Windows.Forms.TextBox
    Friend WithEvents Label23 As System.Windows.Forms.Label
    Friend WithEvents Label27 As System.Windows.Forms.Label
    Friend WithEvents TbRatedPower As System.Windows.Forms.TextBox

End Class
