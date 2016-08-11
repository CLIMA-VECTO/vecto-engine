Module GlobalDefinitions

    Public Const AppVersion As String = "1.0"
    Public Const AppName As String = "VECTO-Engine"
    Public Const AppVersionForm As String = " 1.1"

	Public Worker As System.ComponentModel.BackgroundWorker
	Public MyAppPath As String
	Public MyConfPath As String

	Public sKey As csKey

    Public Const TqStepTol As Single = 20
    Public Const nUStepTol As Single = 5

    Public SetCulture As Boolean

    Public NumWarnings As Integer
    Public CalcMode As Integer = 0



	Public Sub WorkerMsg(ByVal MsgType As tMsgID, ByVal Msg As String, Optional ByVal MsgSrc As String = "", Optional ByVal FilePath As String = "")
		Dim WorkMsg As New cWorkerMsg

		WorkMsg.Msg = Msg
        WorkMsg.MsgType = MsgType

        If MsgType = 1 Then NumWarnings += 1

        Worker.ReportProgress(0, WorkMsg)
    End Sub

	Public Function nTqtoPe(ByVal nU As Double, ByVal M As Double) As Double
		Return ((nU * 2 * Math.PI / 60) * M / 1000)
	End Function

	Public Function nPeToTq(ByVal nU As Single, ByVal Pe As Double) As Single
		Return Pe * 1000 / (nU * 2 * Math.PI / 60)
	End Function


	

	Public Class cWorkerMsg
		Public Msg As String
		Public MsgType As tMsgID
	End Class

	Public Function fAuxComp(ByVal sK As String) As tAuxComp
		Dim x As Integer
		sK = Trim(UCase(sK))

		x = sK.IndexOf("_")

		If x = -1 Then Return tAuxComp.Undefined

		sK = Left(sK, x + 1)

		Select Case sK
			Case sKey.PauxSply
				Return tAuxComp.Psupply
			Case Else
				Return tAuxComp.Undefined
		End Select
	End Function


	Public Function fCompSubStr(ByVal sK As String) As String
		Dim x As Integer

		sK = Trim(UCase(sK))

		x = sK.IndexOf("_")

		If x = -1 Then Return ""

		sK = Right(sK, Len(sK) - x - 1)

		x = CShort(sK.IndexOf(">"))

		If x = -1 Then Return ""

		sK = Left(sK, x)

		Return sK
	End Function

	Public Function fDriComp(ByVal sK As String) As tDriComp
		sK = Trim(UCase(sK))
		Select Case sK
			Case sKey.DRI.t
				Return tDriComp.t
			Case sKey.DRI.V
				Return tDriComp.V
			Case sKey.DRI.Grad
				Return tDriComp.Grad
			Case sKey.DRI.nU
				Return tDriComp.nU
			Case sKey.DRI.Gears
				Return tDriComp.Gears
			Case sKey.DRI.Padd
				Return tDriComp.Padd
			Case sKey.DRI.Pe
				Return tDriComp.Pe
			Case sKey.DRI.VairVres
				Return tDriComp.VairVres
			Case sKey.DRI.VairBeta
				Return tDriComp.VairBeta
			Case sKey.DRI.s
				Return tDriComp.s
			Case sKey.DRI.StopTime
				Return tDriComp.StopTime
			Case sKey.DRI.Torque
				Return tDriComp.Torque
			Case sKey.DRI.Alt
				Return tDriComp.Alt
			Case sKey.DRI.Pwheel
				Return tDriComp.Pwheel
			Case Else
				Return tDriComp.Undefined

		End Select
	End Function


#Region "File path functions"

	'When no path is specified, then insert either HomeDir or MainDir   Special-folders
	Public Function fFileRepl(ByVal file As String, Optional ByVal MainDir As String = "") As String

		Dim ReplPath As String

		'Trim Path
		file = Trim(file)

		'If empty file => Abort
		If file = "" Then Return ""

		'Replace sKeys
		file = Microsoft.VisualBasic.Strings.Replace(file, sKey.DefVehPath & "\", MyAppPath & "Default Vehicles\", 1, -1,
													CompareMethod.Text)
		file = Microsoft.VisualBasic.Strings.Replace(file, sKey.HomePath & "\", MyAppPath, 1, -1, CompareMethod.Text)

		'Replace - Determine folder
		If MainDir = "" Then
			ReplPath = MyAppPath
		Else
			ReplPath = MainDir
		End If

		' "..\" => One folder-level up
		Do While ReplPath.Length > 0 AndAlso Left(file, 3) = "..\"
			ReplPath = fPathUp(ReplPath)
			file = file.Substring(3)
		Loop


		'Supplement Path, if not available
		If fPATH(file) = "" Then

			Return ReplPath & file

		Else
			Return file
		End If
	End Function

	'Path one-level-up      "C:\temp\ordner1\"  >>  "C:\temp\"
	Private Function fPathUp(ByVal Pfad As String) As String
		Dim x As Int16

		Pfad = Pfad.Substring(0, Pfad.Length - 1)

		x = Pfad.LastIndexOf("\")

		If x = -1 Then Return ""

		Return Pfad.Substring(0, x + 1)
	End Function

	'File name without the path    "C:\temp\TEST.txt"  >>  "TEST.txt" oder "TEST"
	Public Function fFILE(ByVal Pfad As String, ByVal MitEndung As Boolean) As String
		Dim x As Int16
		x = Pfad.LastIndexOf("\") + 1
		Pfad = Microsoft.VisualBasic.Right(Pfad, Microsoft.VisualBasic.Len(Pfad) - x)
		If Not MitEndung Then
			x = Pfad.LastIndexOf(".")
			If x > 0 Then Pfad = Microsoft.VisualBasic.Left(Pfad, x)
		End If
		Return Pfad
	End Function

	'Filename without extension   "C:\temp\TEST.txt" >> "C:\temp\TEST"
	Public Function fFileWoExt(ByVal Path As String) As String
		Return fPATH(Path) & fFILE(Path, False)
	End Function

	'Filename without path if Path = WorkDir or MainDir
	Public Function fFileWoDir(ByVal file As String, Optional ByVal MainDir As String = "") As String
		Dim path As String

		If MainDir = "" Then
			path = MyAppPath
		Else
			path = MainDir
		End If

		If UCase(fPATH(file)) = UCase(path) Then file = fFILE(file, True)

		Return file
	End Function

	'Path alone        "C:\temp\TEST.txt"  >>  "C:\temp\"
	'                   "TEST.txt"          >>  ""
	Public Function fPATH(ByVal Pfad As String) As String
		Dim x As Int16
		If Pfad Is Nothing OrElse Pfad.Length < 3 OrElse Pfad.Substring(1, 2) <> ":\" Then Return ""
		x = Pfad.LastIndexOf("\")
		Return Microsoft.VisualBasic.Left(Pfad, x + 1)
	End Function

	'Extension alone      "C:\temp\TEST.txt" >> ".txt"
	Public Function fEXT(ByVal Pfad As String) As String
		Dim x As Int16
		x = Pfad.LastIndexOf(".")
		If x = -1 Then
			Return ""
		Else
			Return Microsoft.VisualBasic.Right(Pfad, Microsoft.VisualBasic.Len(Pfad) - x)
		End If
	End Function


#End Region

	Public Class csKey
		Public DRI As csKeyDRI
		Public AUX As csKeyAux

		Public HomePath As String = "<HOME>"
		Public JobPath As String = "<JOBPATH>"
		Public DefVehPath As String = "<VEHDIR>"
		Public NoFile As String = "<NOFILE>"
		Public EmptyString As String = "<EMPTYSTRING>"
		Public Break As String = "<//>"

		Public Normed As String = "NORM"

		Public PauxSply As String = "<AUX_"

		Public EngDrag As String = "<DRAG>"

		Public Sub New()
			DRI = New csKeyDRI
			AUX = New csKeyAux
		End Sub

		Public Class csKeyDRI
			Public t As String = "<T>"
			Public V As String = "<V>"
			Public Grad As String = "<GRAD>"
			Public Alt As String = "<ALT>"
			Public Gears As String = "<GEAR>"
			Public nU As String = "<N>"
			Public Pe As String = "<PE>"
			Public Padd As String = "<PADD>"
			Public VairVres As String = "<VAIR_RES>"
			Public VairBeta As String = "<VAIR_BETA>"
			Public s As String = "<S>"
			Public StopTime As String = "<STOP>"
			Public Torque As String = "<ME>"
			Public Pwheel As String = "<PWHEEL>"
		End Class

		Public Class csKeyAux
			Public Fan As String = "FAN"
			Public SteerPump As String = "STP"
			Public HVAC As String = "AC"
			Public ElecSys As String = "ES"
			Public PneumSys As String = "PS"
		End Class
	End Class

End Module

