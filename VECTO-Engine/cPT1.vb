Imports System.Collections.Generic

Public Class cPT1

	Private lnU As New List(Of Single)
	Private lPT1 As New List(Of Single)
	Private iDim As Integer
	Public Filepath As String

	Public Function Init() As Boolean

		Dim file As New cFile_V3
		Dim line As String()

		If Not file.OpenRead(Filepath) Then
			WorkerMsg(tMsgID.Err, "PT1 file not found!")
			Return False
		End If

		iDim = -1
		lnU.Clear()
		lPT1.Clear()

		'Skip Header
		file.ReadLine()

		Try

			Do While Not file.EndOfFile
				line = file.ReadLine
				iDim += 1
				lnU.Add(CSng(line(0)))
				lPT1.Add(CSng(line(1)))
			Loop

		Catch ex As Exception
			file.Close()
			WorkerMsg(tMsgID.Err, "Failed to load PT1.csv!" & ex.Message)
			Return False
		End Try

		file.Close()

		Return True

	End Function



	Public Function PT1(ByVal nU As Single) As Single
		Dim i As Int32

		'Extrapolation for x < x(1)
		If lnU(0) >= nU Then
			i = 1
			GoTo lbInt
		End If

		i = 0
		Do While lnU(i) < nU And i < iDim
			i += 1
		Loop



lbInt:
		'Interpolation
		Return (nU - lnU(i - 1)) * (lPT1(i) - lPT1(i - 1)) / (lnU(i) - lnU(i - 1)) + lPT1(i - 1)

	End Function

End Class
