' Copyright 2014 European Union.
' Licensed under the EUPL (the 'Licence');
'
' * You may not use this work except in compliance with the Licence.
' * You may obtain a copy of the Licence at: http://ec.europa.eu/idabc/eupl
' * Unless required by applicable law or agreed to in writing,
'   software distributed under the Licence is distributed on an "AS IS" basis,
'   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
'
' See the LICENSE.txt for the specific language governing permissions and limitations.
Imports System.Collections.Generic

''' <summary>
''' Engine input file
''' </summary>
''' <remarks></remarks>
Public Class cENG

    ''' <summary>
    ''' Current format version
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FormatVersion As Short = 2

    ''' <summary>
    ''' Format version of input file. Defined in ReadFile.
    ''' </summary>
    ''' <remarks></remarks>
    Private FileVersion As Short

    ''' <summary>
    ''' Engine description (model, type, etc.). Saved in input file.
    ''' </summary>
    ''' <remarks></remarks>
    Public ModelName As String

    ''' <summary>
    ''' Engine displacement [ccm]. Saved in input file.
    ''' </summary>
    ''' <remarks></remarks>
    Public Displ As Double

    ''' <summary>
    ''' Idling speed [1/min]. Saved in input file.
    ''' </summary>
    ''' <remarks></remarks>
    Public Nidle As Double

    ''' <summary>
    ''' Rotational inertia including flywheel [kgm²]. Saved in input file. Overwritten by generic value in Declaration mode.
    ''' </summary>
    ''' <remarks></remarks>
    Public I_mot As Double

    ''' <summary>
    ''' List of full load/motoring curve files (.vfld)
    ''' </summary>
    ''' <remarks></remarks>
    Public fFLD As List(Of cSubPath)

    ''' <summary>
    ''' Path to fuel consumption map
    ''' </summary>
    ''' <remarks></remarks>
    Private fMAP As cSubPath

    ''' <summary>
    ''' List of gear-assignments for the given .vfld files.
    ''' </summary>
    ''' <remarks></remarks>
    Public FLDgears As List(Of String)

    ''' <summary>
    ''' Directory of engine file. Defined in FilePath property (Set)
    ''' </summary>
    ''' <remarks></remarks>
    Private MyPath As String

    ''' <summary>
    ''' Full file path. Needs to be defined via FilePath property before calling ReadFile or SaveFile.
    ''' </summary>
    ''' <remarks></remarks>
    Private sFilePath As String

    ''' <summary>
    ''' List of sub input files (e.g. FC map). Can be accessed by FileList property. Generated by CreateFileList.
    ''' </summary>
    ''' <remarks></remarks>
    Private MyFileList As List(Of String)

    ''' <summary>
    ''' WHTC Urban test results. Saved in input file. 
    ''' </summary>
    ''' <remarks></remarks>
    Public WHTCurban As Double

    ''' <summary>
    ''' WHTC Rural test results. Saved in input file. 
    ''' </summary>
    ''' <remarks></remarks>
    Public WHTCrural As Double

    ''' <summary>
    ''' WHTC Motorway test results. Saved in input file. 
    ''' </summary>
    ''' <remarks></remarks>
    Public WHTCmw As Double

    ''' <summary>
    ''' Rated engine speed [1/min]. Engine speed at max. power. Defined in Init.
    ''' </summary>
    ''' <remarks></remarks>
    Public Nrated As Double

    ''' <summary>
    ''' Maximum engine power [kW]. Power at rated engine speed.
    ''' </summary>
    ''' <remarks></remarks>
    Public Pmax As Double

    Public SavedInDeclMode As Boolean


    ''' <summary>
    ''' Generates list of all sub input files (e.g. FC map). Sets MyFileList.
    ''' </summary>
    ''' <returns>True if successful.</returns>
    ''' <remarks></remarks>
    Public Function CreateFileList() As Boolean
        Dim sb As cSubPath

        MyFileList = New List(Of String)

        For Each sb In Me.fFLD
            MyFileList.Add(sb.FullPath)
        Next

        MyFileList.Add(PathMAP)

        'Not used!!! MyFileList.Add(PathWHTC)

        Return True

    End Function

    ''' <summary>
    ''' New instance. Initialise
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        MyPath = ""
        sFilePath = ""
        fMAP = New cSubPath
        SetDefault()
    End Sub

    ''' <summary>
    ''' Set default values
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetDefault()
        ModelName = "Undefined"
        Displ = 0
        Nidle = 0
        I_mot = 0
        Nrated = 0
        Pmax = 0

        fFLD = New List(Of cSubPath)
        FLDgears = New List(Of String)

        fMAP.Clear()

        WHTCurban = 0
        WHTCrural = 0
        WHTCmw = 0

        SavedInDeclMode = False

    End Sub

    ''' <summary>
    ''' Save file. <see cref="P:VECTO.cENG.FilePath" /> must be set before calling.
    ''' </summary>
    ''' <returns>True if successful.</returns>
    ''' <remarks></remarks>
    Public Function SaveFile() As Boolean
        Dim i As Integer
        Dim JSON As New cJSON
        Dim dic As Dictionary(Of String, Object)
        Dim dic0 As Dictionary(Of String, Object)
        Dim ls As List(Of Object)

        'Header
        dic = New Dictionary(Of String, Object)
		dic.Add("CreatedBy", "VECTO-Engine " & AppVersion)
        dic.Add("Date", Now.ToString)
        dic.Add("FileVersion", FormatVersion)
        JSON.Content.Add("Header", dic)

        'Body
        dic = New Dictionary(Of String, Object)

		dic.Add("SavedInDeclMode", True)
		SavedInDeclMode = True

        dic.Add("ModelName", ModelName)

        dic.Add("Displacement", Displ)
        dic.Add("IdlingSpeed", Nidle)
        dic.Add("Inertia", I_mot)

        ls = New List(Of Object)
        For i = 0 To fFLD.Count - 1
            dic0 = New Dictionary(Of String, Object)
            dic0.Add("Path", fFLD(i).PathOrDummy)
            dic0.Add("Gears", FLDgears(i))
            ls.Add(dic0)
        Next
        dic.Add("FullLoadCurves", ls)

        dic.Add("FuelMap", fMAP.PathOrDummy)

        dic.Add("WHTC-Urban", WHTCurban)
        dic.Add("WHTC-Rural", WHTCrural)
        dic.Add("WHTC-Motorway", WHTCmw)


        JSON.Content.Add("Body", dic)


        Return JSON.WriteFile(sFilePath)


    End Function

    ''' <summary>
    ''' Read file. <see cref="P:VECTO.cENG.FilePath" /> must be set before calling.
    ''' </summary>
    ''' <returns>True if successful.</returns>
    ''' <remarks></remarks>
    Public Function ReadFile(Optional ByVal ShowMsg As Boolean = True) As Boolean
        Dim MsgSrc As String
        Dim i As Integer
        Dim JSON As New cJSON
        Dim dic As Object

        MsgSrc = "ENG/ReadFile"

        SetDefault()


        If Not JSON.ReadFile(sFilePath) Then Return False

        Try

            FileVersion = JSON.Content("Header")("FileVersion")

            If FileVersion > 1 Then
                SavedInDeclMode = JSON.Content("Body")("SavedInDeclMode")
            Else
				SavedInDeclMode = True
            End If

            ModelName = JSON.Content("Body")("ModelName")

            Displ = JSON.Content("Body")("Displacement")
            Nidle = JSON.Content("Body")("IdlingSpeed")
            I_mot = JSON.Content("Body")("Inertia")

            i = -1
            For Each dic In JSON.Content("Body")("FullLoadCurves")
                i += 1
                fFLD.Add(New cSubPath)
                fFLD(i).Init(MyPath, dic("Path"))
                FLDgears.Add(dic("Gears"))
            Next

            fMAP.Init(MyPath, JSON.Content("Body")("FuelMap"))

            If Not JSON.Content("Body")("WHTC-Urban") Is Nothing Then
                WHTCurban = CSng(JSON.Content("Body")("WHTC-Urban"))
                WHTCrural = CSng(JSON.Content("Body")("WHTC-Rural"))
                WHTCmw = CSng(JSON.Content("Body")("WHTC-Motorway"))
            End If

        Catch ex As Exception
            If ShowMsg Then WorkerMsg(tMsgID.Err, "Failed to read VECTO file! " & ex.Message, MsgSrc)
            Return False
        End Try

        Return True

    End Function

  
    ''' <summary>
    ''' Returns list of sub input files after calling CreateFileList.
    ''' </summary>
    ''' <value></value>
    ''' <returns>list of sub input files</returns>
    ''' <remarks></remarks>
    Public ReadOnly Property FileList As List(Of String)
        Get
            Return MyFileList
        End Get
    End Property

    ''' <summary>
    ''' Get or set Filepath before calling <see cref="M:VECTO.cENG.ReadFile" /> or <see cref="M:VECTO.cENG.SaveFile" />
    ''' </summary>
    ''' <value></value>
    ''' <returns>Full filepath</returns>
    ''' <remarks></remarks>
    Public Property FilePath() As String
        Get
            Return sFilePath
        End Get
        Set(ByVal value As String)
            sFilePath = value
            If sFilePath = "" Then
                MyPath = ""
            Else
                MyPath = IO.Path.GetDirectoryName(sFilePath) & "\"
            End If
        End Set
    End Property

    ''' <summary>
    ''' Get or set file path (cSubPath) of .vfld file for each gear range.
    ''' </summary>
    ''' <param name="x">Index</param>
    ''' <param name="Original">True= (relative) file path as saved in file; False= full file path</param>
    ''' <value></value>
    ''' <returns>Relative or absolute file path for each gear range</returns>
    ''' <remarks></remarks>
    Public Property PathFLD(ByVal x As Short, Optional ByVal Original As Boolean = False) As String
        Get
            If Original Then
                Return fFLD(x).OriginalPath
            Else
                Return fFLD(x).FullPath
            End If
        End Get
        Set(ByVal value As String)
            fFLD(x).Init(MyPath, value)
        End Set
    End Property

    ''' <summary>
    ''' Get or set file path (cSubPath) to FC map (.vmap)
    ''' </summary>
    ''' <param name="Original">True= (relative) file path as saved in file; False= full file path</param>
    ''' <value></value>
    ''' <returns>Relative or absolute file path to FC map</returns>
    ''' <remarks></remarks>
    Public Property PathMAP(Optional ByVal Original As Boolean = False) As String
        Get
            If Original Then
                Return fMAP.OriginalPath
            Else
                Return fMAP.FullPath
            End If
        End Get
        Set(ByVal value As String)
            fMAP.Init(MyPath, value)
        End Set
    End Property

End Class
