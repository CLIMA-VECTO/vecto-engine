Imports System.IO
Imports System.Xml

Public Class XmlResourceResolver
	Inherits XmlUrlResolver
	'public class XmlResourceResolver : XmlUrlResolver
	'{
	Const BaseUri As String = "schema://"

	Public Overrides Function GetEntity(absoluteUri As Uri, role As String, ofObjectToReturn As Type) As Object

		If (absoluteUri.Scheme = "schema") Then
			Return LoadResourceAsStream(ResourceType.XMLSchema,
										Path.GetFileName(absoluteUri.LocalPath))
		End If
		Return MyBase.GetEntity(absoluteUri, role, ofObjectToReturn)
	End Function
	'}
End Class