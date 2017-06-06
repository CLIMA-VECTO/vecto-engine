Imports System.IO
Imports System.Reflection

Public Class cPT1
    Private ReadOnly lnU As New List(Of Double)
    Private ReadOnly lPT1 As New List(Of Double)
	Private iDim As Integer
    'Public Filepath As String

	Public Function Init() As Boolean

		Dim theAssembly As Assembly = Assembly.GetExecutingAssembly()
		Dim resource As Stream = theAssembly.GetManifestResourceStream("VECTO_Engine.PT1.csv")

		If IsNothing(resource) Then
			Throw New Exception("PT1 data not found")
		End If


		Dim line As String()
		iDim = -1
		lnU.Clear()
		lPT1.Clear()

		'Skip Header
		Dim reader = New StreamReader(resource)
		reader.ReadLine()

		Try

			Do While Not reader.EndOfStream
				line = reader.ReadLine.Split(New Char() {","c}, 2, StringSplitOptions.RemoveEmptyEntries)
				iDim += 1
				lnU.Add(CSng(line(0)))
				lPT1.Add(CSng(line(1)))
			Loop

		Catch ex As Exception
			reader.Close()
			WorkerMsg(tMsgID.Err, "Failed to load PT1.csv!" & ex.Message)
			Return False

		End Try

		reader.Close()

		Return True
	End Function


    Public Function PT1(nU As Double) As Double
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
