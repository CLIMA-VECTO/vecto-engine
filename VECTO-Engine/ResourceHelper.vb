Imports System.IO
Imports System.Reflection

Public Module ResourceHelper
	Public Function ReadStream(resourceName As String) As Stream

		Dim assembly As Assembly = assembly.GetExecutingAssembly()
		Dim resource As Stream = assembly.GetManifestResourceStream(resourceName)
		If (resource Is Nothing) Then
			Throw New Exception("Resource file not found: " + resourceName)
		End If

		Return resource
	End Function

	Public Enum ResourceType

		XMLSchema = 1
	End Enum

	Public Function LoadResourceAsStream(type As ResourceType, resourceName As String) As Stream

		Dim resourceBase As String
		Select type
			Case ResourceType.XMLSchema
				resourceBase = "VECTO_Engine."
			Case Else
				Throw New ArgumentOutOfRangeException("type", type, Nothing)
		End Select
		Return ReadStream(resourceBase + resourceName)
	End Function
End Module
