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
        Me.TbNlo = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.TbNpref = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.TbNhi = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.TbNidle = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.GrInput = New System.Windows.Forms.GroupBox()
        Me.GrRpm = New System.Windows.Forms.GroupBox()
        Me.TbN95h = New System.Windows.Forms.TextBox()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TbWHTC = New System.Windows.Forms.TextBox()
        Me.TbR85 = New System.Windows.Forms.TextBox()
        Me.TbDrag = New System.Windows.Forms.TextBox()
        Me.TbR49 = New System.Windows.Forms.TextBox()
        Me.GrOutput = New System.Windows.Forms.GroupBox()
        Me.GrWHTCfactors = New System.Windows.Forms.GroupBox()
        Me.TbUrbanFactor = New System.Windows.Forms.TextBox()
        Me.Label23 = New System.Windows.Forms.Label()
        Me.TbRuralFactor = New System.Windows.Forms.TextBox()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.TbMotorwayFactor = New System.Windows.Forms.TextBox()
        Me.Label22 = New System.Windows.Forms.Label()
        Me.LvMsg = New System.Windows.Forms.ListView()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.TbOutputFolder = New System.Windows.Forms.TextBox()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.BtStart = New System.Windows.Forms.Button()
        Me.BgWorker = New System.ComponentModel.BackgroundWorker()
        Me.Button5 = New System.Windows.Forms.Button()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.Button4 = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.BtOpenMap = New System.Windows.Forms.Button()
        Me.GrInput.SuspendLayout()
        Me.GrRpm.SuspendLayout()
        Me.GrOutput.SuspendLayout()
        Me.GrWHTCfactors.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
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
        Me.Label3.Location = New System.Drawing.Point(288, 26)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(27, 13)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "n_lo"
        '
        'TbNlo
        '
        Me.TbNlo.Location = New System.Drawing.Point(321, 23)
        Me.TbNlo.Name = "TbNlo"
        Me.TbNlo.Size = New System.Drawing.Size(62, 20)
        Me.TbNlo.TabIndex = 1
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(389, 26)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(40, 13)
        Me.Label4.TabIndex = 2
        Me.Label4.Text = "[1/min]"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(278, 52)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(37, 13)
        Me.Label5.TabIndex = 0
        Me.Label5.Text = "n_pref"
        '
        'TbNpref
        '
        Me.TbNpref.Location = New System.Drawing.Point(321, 49)
        Me.TbNpref.Name = "TbNpref"
        Me.TbNpref.Size = New System.Drawing.Size(62, 20)
        Me.TbNpref.TabIndex = 2
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(389, 52)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(40, 13)
        Me.Label6.TabIndex = 2
        Me.Label6.Text = "[1/min]"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(515, 52)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(27, 13)
        Me.Label7.TabIndex = 0
        Me.Label7.Text = "n_hi"
        '
        'TbNhi
        '
        Me.TbNhi.Location = New System.Drawing.Point(548, 49)
        Me.TbNhi.Name = "TbNhi"
        Me.TbNhi.Size = New System.Drawing.Size(62, 20)
        Me.TbNhi.TabIndex = 4
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(616, 52)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(40, 13)
        Me.Label8.TabIndex = 2
        Me.Label8.Text = "[1/min]"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(36, 52)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(35, 13)
        Me.Label9.TabIndex = 0
        Me.Label9.Text = "n_idle"
        '
        'TbNidle
        '
        Me.TbNidle.Location = New System.Drawing.Point(77, 49)
        Me.TbNidle.Name = "TbNidle"
        Me.TbNidle.Size = New System.Drawing.Size(62, 20)
        Me.TbNidle.TabIndex = 0
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(145, 52)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(40, 13)
        Me.Label10.TabIndex = 2
        Me.Label10.Text = "[1/min]"
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
        Me.GrInput.Controls.Add(Me.TbWHTC)
        Me.GrInput.Controls.Add(Me.TbR85)
        Me.GrInput.Controls.Add(Me.Button4)
        Me.GrInput.Controls.Add(Me.Button2)
        Me.GrInput.Controls.Add(Me.TbDrag)
        Me.GrInput.Controls.Add(Me.Button3)
        Me.GrInput.Controls.Add(Me.TbR49)
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
        'GrRpm
        '
        Me.GrRpm.Controls.Add(Me.TbN95h)
        Me.GrRpm.Controls.Add(Me.TbNlo)
        Me.GrRpm.Controls.Add(Me.Label7)
        Me.GrRpm.Controls.Add(Me.Label10)
        Me.GrRpm.Controls.Add(Me.TbNpref)
        Me.GrRpm.Controls.Add(Me.TbNhi)
        Me.GrRpm.Controls.Add(Me.Label20)
        Me.GrRpm.Controls.Add(Me.Label8)
        Me.GrRpm.Controls.Add(Me.Label3)
        Me.GrRpm.Controls.Add(Me.Label9)
        Me.GrRpm.Controls.Add(Me.Label6)
        Me.GrRpm.Controls.Add(Me.Label5)
        Me.GrRpm.Controls.Add(Me.Label19)
        Me.GrRpm.Controls.Add(Me.TbNidle)
        Me.GrRpm.Controls.Add(Me.Label4)
        Me.GrRpm.Location = New System.Drawing.Point(192, 176)
        Me.GrRpm.Name = "GrRpm"
        Me.GrRpm.Size = New System.Drawing.Size(705, 77)
        Me.GrRpm.TabIndex = 10
        Me.GrRpm.TabStop = False
        Me.GrRpm.Text = "Engine Test Speeds"
        '
        'TbN95h
        '
        Me.TbN95h.Location = New System.Drawing.Point(548, 23)
        Me.TbN95h.Name = "TbN95h"
        Me.TbN95h.Size = New System.Drawing.Size(62, 20)
        Me.TbN95h.TabIndex = 3
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Location = New System.Drawing.Point(505, 26)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(37, 13)
        Me.Label20.TabIndex = 0
        Me.Label20.Text = "n_95h"
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Location = New System.Drawing.Point(616, 26)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(40, 13)
        Me.Label19.TabIndex = 2
        Me.Label19.Text = "[1/min]"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(79, 125)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(107, 13)
        Me.Label13.TabIndex = 0
        Me.Label13.Text = "WHTC Measurement"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(34, 73)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(152, 13)
        Me.Label11.TabIndex = 0
        Me.Label11.Text = "R49 Full Load of actual engine"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(125, 99)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(61, 13)
        Me.Label12.TabIndex = 0
        Me.Label12.Text = "Drag Curve"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(79, 47)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(107, 13)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Parent R49 Full Load"
        '
        'TbWHTC
        '
        Me.TbWHTC.Location = New System.Drawing.Point(192, 122)
        Me.TbWHTC.Name = "TbWHTC"
        Me.TbWHTC.Size = New System.Drawing.Size(705, 20)
        Me.TbWHTC.TabIndex = 8
        '
        'TbR85
        '
        Me.TbR85.Location = New System.Drawing.Point(192, 70)
        Me.TbR85.Name = "TbR85"
        Me.TbR85.Size = New System.Drawing.Size(705, 20)
        Me.TbR85.TabIndex = 4
        '
        'TbDrag
        '
        Me.TbDrag.Location = New System.Drawing.Point(192, 96)
        Me.TbDrag.Name = "TbDrag"
        Me.TbDrag.Size = New System.Drawing.Size(705, 20)
        Me.TbDrag.TabIndex = 6
        '
        'TbR49
        '
        Me.TbR49.Location = New System.Drawing.Point(192, 44)
        Me.TbR49.Name = "TbR49"
        Me.TbR49.Size = New System.Drawing.Size(705, 20)
        Me.TbR49.TabIndex = 2
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
        Me.GrWHTCfactors.Text = "WHTC Correction Factors"
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
        Me.BtStart.Location = New System.Drawing.Point(12, 277)
        Me.BtStart.Name = "BtStart"
        Me.BtStart.Size = New System.Drawing.Size(937, 39)
        Me.BtStart.TabIndex = 1
        Me.BtStart.TabStop = False
        Me.BtStart.Text = "START"
        Me.BtStart.UseVisualStyleBackColor = True
        '
        'BgWorker
        '
        Me.BgWorker.WorkerReportsProgress = True
        Me.BgWorker.WorkerSupportsCancellation = True
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
        'Button4
        '
        Me.Button4.Image = Global.VECTO_Engine.My.Resources.Resources.Open_icon
        Me.Button4.Location = New System.Drawing.Point(903, 120)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(28, 23)
        Me.Button4.TabIndex = 9
        Me.Button4.TabStop = False
        Me.Button4.UseVisualStyleBackColor = True
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
        'Button3
        '
        Me.Button3.Image = Global.VECTO_Engine.My.Resources.Resources.Open_icon
        Me.Button3.Location = New System.Drawing.Point(903, 94)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(28, 23)
        Me.Button3.TabIndex = 7
        Me.Button3.TabStop = False
        Me.Button3.UseVisualStyleBackColor = True
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
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(961, 645)
        Me.Controls.Add(Me.GrOutput)
        Me.Controls.Add(Me.BtStart)
        Me.Controls.Add(Me.GrInput)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.GrInput.ResumeLayout(False)
        Me.GrInput.PerformLayout()
        Me.GrRpm.ResumeLayout(False)
        Me.GrRpm.PerformLayout()
        Me.GrOutput.ResumeLayout(False)
        Me.GrOutput.PerformLayout()
        Me.GrWHTCfactors.ResumeLayout(False)
        Me.GrWHTCfactors.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
	Friend WithEvents Label1 As System.Windows.Forms.Label
	Friend WithEvents TbFuelMap As System.Windows.Forms.TextBox
	Friend WithEvents BtOpenMap As System.Windows.Forms.Button
	Friend WithEvents Label3 As System.Windows.Forms.Label
	Friend WithEvents TbNlo As System.Windows.Forms.TextBox
	Friend WithEvents Label4 As System.Windows.Forms.Label
	Friend WithEvents Label5 As System.Windows.Forms.Label
	Friend WithEvents TbNpref As System.Windows.Forms.TextBox
	Friend WithEvents Label6 As System.Windows.Forms.Label
	Friend WithEvents Label7 As System.Windows.Forms.Label
	Friend WithEvents TbNhi As System.Windows.Forms.TextBox
	Friend WithEvents Label8 As System.Windows.Forms.Label
	Friend WithEvents Label9 As System.Windows.Forms.Label
	Friend WithEvents TbNidle As System.Windows.Forms.TextBox
	Friend WithEvents Label10 As System.Windows.Forms.Label
	Friend WithEvents GrInput As System.Windows.Forms.GroupBox
	Friend WithEvents GrRpm As System.Windows.Forms.GroupBox
	Friend WithEvents TbN95h As System.Windows.Forms.TextBox
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
	Friend WithEvents TbR49 As System.Windows.Forms.TextBox
	Friend WithEvents Button1 As System.Windows.Forms.Button
	Friend WithEvents Label11 As System.Windows.Forms.Label
	Friend WithEvents TbR85 As System.Windows.Forms.TextBox
	Friend WithEvents Button2 As System.Windows.Forms.Button
	Friend WithEvents Label13 As System.Windows.Forms.Label
	Friend WithEvents Label12 As System.Windows.Forms.Label
	Friend WithEvents TbWHTC As System.Windows.Forms.TextBox
	Friend WithEvents Button4 As System.Windows.Forms.Button
	Friend WithEvents TbDrag As System.Windows.Forms.TextBox
	Friend WithEvents Button3 As System.Windows.Forms.Button
	Friend WithEvents TbOutputFolder As System.Windows.Forms.TextBox
	Friend WithEvents Button5 As System.Windows.Forms.Button
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox

End Class
