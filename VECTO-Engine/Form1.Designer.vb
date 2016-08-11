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
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TbFuelMap = New System.Windows.Forms.TextBox()
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
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.GrRpm = New System.Windows.Forms.GroupBox()
        Me.TbFCspecHot = New System.Windows.Forms.TextBox()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TbFLC_highest = New System.Windows.Forms.TextBox()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.TbFLC_lower = New System.Windows.Forms.TextBox()
        Me.TbMotoring = New System.Windows.Forms.TextBox()
        Me.Button4 = New System.Windows.Forms.Button()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.TbFLC_Parent = New System.Windows.Forms.TextBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.BtOpenMap = New System.Windows.Forms.Button()
        Me.GrOutput = New System.Windows.Forms.GroupBox()
        Me.GrWHTCfactors = New System.Windows.Forms.GroupBox()
        Me.TbColdHotFactor = New System.Windows.Forms.TextBox()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.TbUrbanFactor = New System.Windows.Forms.TextBox()
        Me.Label23 = New System.Windows.Forms.Label()
        Me.TbRuralFactor = New System.Windows.Forms.TextBox()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.TbMotorwayFactor = New System.Windows.Forms.TextBox()
        Me.Label22 = New System.Windows.Forms.Label()
        Me.LvMsg = New System.Windows.Forms.ListView()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.TbOutputFolder = New System.Windows.Forms.TextBox()
        Me.Button5 = New System.Windows.Forms.Button()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.BtStart = New System.Windows.Forms.Button()
        Me.BgWorker = New System.ComponentModel.BackgroundWorker()
        Me.BtPrecalc = New System.Windows.Forms.Button()
        Me.GrInput.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GrRpm.SuspendLayout()
        Me.GrOutput.SuspendLayout()
        Me.GrWHTCfactors.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(135, 21)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(51, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Fuel Map"
        '
        'TbFuelMap
        '
        Me.TbFuelMap.Location = New System.Drawing.Point(192, 18)
        Me.TbFuelMap.Name = "TbFuelMap"
        Me.TbFuelMap.Size = New System.Drawing.Size(705, 20)
        Me.TbFuelMap.TabIndex = 0
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
        Me.TbFCspecCold.Size = New System.Drawing.Size(62, 20)
        Me.TbFCspecCold.TabIndex = 0
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(188, 37)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(47, 13)
        Me.Label4.TabIndex = 2
        Me.Label4.Text = "[g/kWh]"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(478, 78)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(89, 13)
        Me.Label5.TabIndex = 0
        Me.Label5.Text = "WHTC-Motorway"
        '
        'TbFCspecMW
        '
        Me.TbFCspecMW.Location = New System.Drawing.Point(573, 75)
        Me.TbFCspecMW.Name = "TbFCspecMW"
        Me.TbFCspecMW.Size = New System.Drawing.Size(62, 20)
        Me.TbFCspecMW.TabIndex = 4
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(641, 78)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(47, 13)
        Me.Label6.TabIndex = 2
        Me.Label6.Text = "[g/kWh]"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(499, 52)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(68, 13)
        Me.Label7.TabIndex = 0
        Me.Label7.Text = "WHTC-Rural"
        '
        'TbFCspecRur
        '
        Me.TbFCspecRur.Location = New System.Drawing.Point(573, 49)
        Me.TbFCspecRur.Name = "TbFCspecRur"
        Me.TbFCspecRur.Size = New System.Drawing.Size(62, 20)
        Me.TbFCspecRur.TabIndex = 3
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(641, 52)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(47, 13)
        Me.Label8.TabIndex = 2
        Me.Label8.Text = "[g/kWh]"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(495, 23)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(72, 13)
        Me.Label9.TabIndex = 0
        Me.Label9.Text = "WHTC-Urban"
        '
        'TbFCspecUrb
        '
        Me.TbFCspecUrb.Location = New System.Drawing.Point(573, 19)
        Me.TbFCspecUrb.Name = "TbFCspecUrb"
        Me.TbFCspecUrb.Size = New System.Drawing.Size(62, 20)
        Me.TbFCspecUrb.TabIndex = 2
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(641, 22)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(47, 13)
        Me.Label10.TabIndex = 2
        Me.Label10.Text = "[g/kWh]"
        '
        'GrInput
        '
        Me.GrInput.Controls.Add(Me.PictureBox1)
        Me.GrInput.Controls.Add(Me.GrRpm)
        Me.GrInput.Controls.Add(Me.Label13)
        Me.GrInput.Controls.Add(Me.Label11)
        Me.GrInput.Controls.Add(Me.Label12)
        Me.GrInput.Controls.Add(Me.Label2)
        Me.GrInput.Controls.Add(Me.Label1)
        Me.GrInput.Controls.Add(Me.TbFLC_highest)
        Me.GrInput.Controls.Add(Me.Button2)
        Me.GrInput.Controls.Add(Me.TbFLC_lower)
        Me.GrInput.Controls.Add(Me.TbMotoring)
        Me.GrInput.Controls.Add(Me.Button4)
        Me.GrInput.Controls.Add(Me.Button3)
        Me.GrInput.Controls.Add(Me.TbFLC_Parent)
        Me.GrInput.Controls.Add(Me.Button1)
        Me.GrInput.Controls.Add(Me.TbFuelMap)
        Me.GrInput.Controls.Add(Me.BtOpenMap)
        Me.GrInput.Location = New System.Drawing.Point(12, 12)
        Me.GrInput.Name = "GrInput"
        Me.GrInput.Size = New System.Drawing.Size(937, 259)
        Me.GrInput.TabIndex = 0
        Me.GrInput.TabStop = False
        Me.GrInput.Text = "Input"
        '
        'PictureBox1
        '
        Me.PictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.PictureBox1.Image = Global.VECTO_Engine.My.Resources.Resources.vecto_engine_test__Custom___2_
        Me.PictureBox1.Location = New System.Drawing.Point(6, 162)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(145, 91)
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
        Me.GrRpm.Location = New System.Drawing.Point(192, 148)
        Me.GrRpm.Name = "GrRpm"
        Me.GrRpm.Size = New System.Drawing.Size(705, 105)
        Me.GrRpm.TabIndex = 10
        Me.GrRpm.TabStop = False
        Me.GrRpm.Text = "Specific fuel consumption measured"
        '
        'TbFCspecHot
        '
        Me.TbFCspecHot.Location = New System.Drawing.Point(120, 75)
        Me.TbFCspecHot.Name = "TbFCspecHot"
        Me.TbFCspecHot.Size = New System.Drawing.Size(62, 20)
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
        Me.Label19.Location = New System.Drawing.Point(188, 78)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(47, 13)
        Me.Label19.TabIndex = 2
        Me.Label19.Text = "[g/kWh]"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(4, 99)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(182, 13)
        Me.Label13.TabIndex = 0
        Me.Label13.Text = "Full-Load for lower gears (red. torque)"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(60, 73)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(126, 13)
        Me.Label11.TabIndex = 0
        Me.Label11.Text = "Full-Load for highest gear"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(107, 125)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(79, 13)
        Me.Label12.TabIndex = 0
        Me.Label12.Text = "Motoring-Curve"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(31, 47)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(155, 13)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Full-Load of CO2-Parent engine"
        '
        'TbFLC_highest
        '
        Me.TbFLC_highest.Location = New System.Drawing.Point(192, 70)
        Me.TbFLC_highest.Name = "TbFLC_highest"
        Me.TbFLC_highest.Size = New System.Drawing.Size(705, 20)
        Me.TbFLC_highest.TabIndex = 4
        '
        'Button2
        '
        Me.Button2.Image = Global.VECTO_Engine.My.Resources.Resources.Open_icon
        Me.Button2.Location = New System.Drawing.Point(903, 68)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(28, 23)
        Me.Button2.TabIndex = 5
        Me.Button2.TabStop = False
        Me.Button2.UseVisualStyleBackColor = True
        '
        'TbFLC_lower
        '
        Me.TbFLC_lower.Location = New System.Drawing.Point(192, 96)
        Me.TbFLC_lower.Name = "TbFLC_lower"
        Me.TbFLC_lower.Size = New System.Drawing.Size(705, 20)
        Me.TbFLC_lower.TabIndex = 6
        '
        'TbMotoring
        '
        Me.TbMotoring.Location = New System.Drawing.Point(192, 122)
        Me.TbMotoring.Name = "TbMotoring"
        Me.TbMotoring.Size = New System.Drawing.Size(705, 20)
        Me.TbMotoring.TabIndex = 8
        '
        'Button4
        '
        Me.Button4.Image = Global.VECTO_Engine.My.Resources.Resources.Open_icon
        Me.Button4.Location = New System.Drawing.Point(903, 94)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(28, 23)
        Me.Button4.TabIndex = 7
        Me.Button4.TabStop = False
        Me.Button4.UseVisualStyleBackColor = True
        '
        'Button3
        '
        Me.Button3.Image = Global.VECTO_Engine.My.Resources.Resources.Open_icon
        Me.Button3.Location = New System.Drawing.Point(903, 120)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(28, 23)
        Me.Button3.TabIndex = 9
        Me.Button3.TabStop = False
        Me.Button3.UseVisualStyleBackColor = True
        '
        'TbFLC_Parent
        '
        Me.TbFLC_Parent.Location = New System.Drawing.Point(192, 44)
        Me.TbFLC_Parent.Name = "TbFLC_Parent"
        Me.TbFLC_Parent.Size = New System.Drawing.Size(705, 20)
        Me.TbFLC_Parent.TabIndex = 2
        '
        'Button1
        '
        Me.Button1.Image = Global.VECTO_Engine.My.Resources.Resources.Open_icon
        Me.Button1.Location = New System.Drawing.Point(903, 42)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(28, 23)
        Me.Button1.TabIndex = 3
        Me.Button1.TabStop = False
        Me.Button1.UseVisualStyleBackColor = True
        '
        'BtOpenMap
        '
        Me.BtOpenMap.Image = Global.VECTO_Engine.My.Resources.Resources.Open_icon
        Me.BtOpenMap.Location = New System.Drawing.Point(903, 16)
        Me.BtOpenMap.Name = "BtOpenMap"
        Me.BtOpenMap.Size = New System.Drawing.Size(28, 23)
        Me.BtOpenMap.TabIndex = 1
        Me.BtOpenMap.TabStop = False
        Me.BtOpenMap.UseVisualStyleBackColor = True
        '
        'GrOutput
        '
        Me.GrOutput.Controls.Add(Me.GrWHTCfactors)
        Me.GrOutput.Controls.Add(Me.LvMsg)
        Me.GrOutput.Controls.Add(Me.TbOutputFolder)
        Me.GrOutput.Controls.Add(Me.Button5)
        Me.GrOutput.Controls.Add(Me.Label14)
        Me.GrOutput.Location = New System.Drawing.Point(12, 324)
        Me.GrOutput.Name = "GrOutput"
        Me.GrOutput.Size = New System.Drawing.Size(937, 309)
        Me.GrOutput.TabIndex = 2
        Me.GrOutput.TabStop = False
        Me.GrOutput.Text = "Output"
        '
        'GrWHTCfactors
        '
        Me.GrWHTCfactors.Controls.Add(Me.TbColdHotFactor)
        Me.GrWHTCfactors.Controls.Add(Me.Label15)
        Me.GrWHTCfactors.Controls.Add(Me.TbUrbanFactor)
        Me.GrWHTCfactors.Controls.Add(Me.Label23)
        Me.GrWHTCfactors.Controls.Add(Me.TbRuralFactor)
        Me.GrWHTCfactors.Controls.Add(Me.Label21)
        Me.GrWHTCfactors.Controls.Add(Me.TbMotorwayFactor)
        Me.GrWHTCfactors.Controls.Add(Me.Label22)
        Me.GrWHTCfactors.Location = New System.Drawing.Point(6, 241)
        Me.GrWHTCfactors.Name = "GrWHTCfactors"
        Me.GrWHTCfactors.Size = New System.Drawing.Size(925, 62)
        Me.GrWHTCfactors.TabIndex = 8
        Me.GrWHTCfactors.TabStop = False
        Me.GrWHTCfactors.Text = "Correction Factors"
        '
        'TbColdHotFactor
        '
        Me.TbColdHotFactor.Location = New System.Drawing.Point(667, 35)
        Me.TbColdHotFactor.Name = "TbColdHotFactor"
        Me.TbColdHotFactor.ReadOnly = True
        Me.TbColdHotFactor.Size = New System.Drawing.Size(62, 20)
        Me.TbColdHotFactor.TabIndex = 3
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(528, 38)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(133, 13)
        Me.Label15.TabIndex = 4
        Me.Label15.Text = "Cold/Hot-Balancing Factor"
        '
        'TbUrbanFactor
        '
        Me.TbUrbanFactor.Location = New System.Drawing.Point(83, 35)
        Me.TbUrbanFactor.Name = "TbUrbanFactor"
        Me.TbUrbanFactor.ReadOnly = True
        Me.TbUrbanFactor.Size = New System.Drawing.Size(62, 20)
        Me.TbUrbanFactor.TabIndex = 0
        '
        'Label23
        '
        Me.Label23.AutoSize = True
        Me.Label23.Location = New System.Drawing.Point(41, 38)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(36, 13)
        Me.Label23.TabIndex = 0
        Me.Label23.Text = "Urban"
        '
        'TbRuralFactor
        '
        Me.TbRuralFactor.Location = New System.Drawing.Point(214, 35)
        Me.TbRuralFactor.Name = "TbRuralFactor"
        Me.TbRuralFactor.ReadOnly = True
        Me.TbRuralFactor.Size = New System.Drawing.Size(62, 20)
        Me.TbRuralFactor.TabIndex = 1
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.Location = New System.Drawing.Point(315, 38)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(53, 13)
        Me.Label21.TabIndex = 0
        Me.Label21.Text = "Motorway"
        '
        'TbMotorwayFactor
        '
        Me.TbMotorwayFactor.Location = New System.Drawing.Point(374, 35)
        Me.TbMotorwayFactor.Name = "TbMotorwayFactor"
        Me.TbMotorwayFactor.ReadOnly = True
        Me.TbMotorwayFactor.Size = New System.Drawing.Size(62, 20)
        Me.TbMotorwayFactor.TabIndex = 2
        '
        'Label22
        '
        Me.Label22.AutoSize = True
        Me.Label22.Location = New System.Drawing.Point(176, 38)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(32, 13)
        Me.Label22.TabIndex = 0
        Me.Label22.Text = "Rural"
        '
        'LvMsg
        '
        Me.LvMsg.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1})
        Me.LvMsg.FullRowSelect = True
        Me.LvMsg.GridLines = True
        Me.LvMsg.LabelWrap = False
        Me.LvMsg.Location = New System.Drawing.Point(6, 45)
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
        Me.BtStart.Location = New System.Drawing.Point(12, 277)
        Me.BtStart.Name = "BtStart"
        Me.BtStart.Size = New System.Drawing.Size(306, 39)
        Me.BtStart.TabIndex = 1
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
        Me.BtPrecalc.Location = New System.Drawing.Point(395, 277)
        Me.BtPrecalc.Name = "BtPrecalc"
        Me.BtPrecalc.Size = New System.Drawing.Size(247, 39)
        Me.BtPrecalc.TabIndex = 3
        Me.BtPrecalc.TabStop = False
        Me.BtPrecalc.Text = "Precalculate characteristic engine speeds" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "and grid for fuel map"
        Me.BtPrecalc.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(961, 645)
        Me.Controls.Add(Me.BtPrecalc)
        Me.Controls.Add(Me.GrOutput)
        Me.Controls.Add(Me.BtStart)
        Me.Controls.Add(Me.GrInput)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.GrInput.ResumeLayout(False)
        Me.GrInput.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GrRpm.ResumeLayout(False)
        Me.GrRpm.PerformLayout()
        Me.GrOutput.ResumeLayout(False)
        Me.GrOutput.PerformLayout()
        Me.GrWHTCfactors.ResumeLayout(False)
        Me.GrWHTCfactors.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TbFuelMap As System.Windows.Forms.TextBox
    Friend WithEvents BtOpenMap As System.Windows.Forms.Button
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
    Friend WithEvents GrWHTCfactors As System.Windows.Forms.GroupBox
    Friend WithEvents TbUrbanFactor As System.Windows.Forms.TextBox
    Friend WithEvents Label23 As System.Windows.Forms.Label
    Friend WithEvents TbRuralFactor As System.Windows.Forms.TextBox
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents TbMotorwayFactor As System.Windows.Forms.TextBox
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents TbFLC_Parent As System.Windows.Forms.TextBox
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents TbFLC_highest As System.Windows.Forms.TextBox
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents TbFLC_lower As System.Windows.Forms.TextBox
    Friend WithEvents Button4 As System.Windows.Forms.Button
    Friend WithEvents TbMotoring As System.Windows.Forms.TextBox
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents TbOutputFolder As System.Windows.Forms.TextBox
    Friend WithEvents Button5 As System.Windows.Forms.Button
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents BtPrecalc As System.Windows.Forms.Button
    Friend WithEvents TbColdHotFactor As System.Windows.Forms.TextBox
    Friend WithEvents Label15 As System.Windows.Forms.Label

End Class
