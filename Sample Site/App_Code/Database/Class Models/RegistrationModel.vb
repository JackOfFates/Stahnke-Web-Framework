Imports System.Reflection
Imports MicroLibrary.Serialization
Imports Microsoft.VisualBasic
Imports Database
Imports StahnkeFramework
Imports System.Collections.Generic

Public Class RegistrationModel
    Inherits RequestModel(Of account)

    Public Shadows Property SpecialFields As New Dictionary(Of String, RequestField) From {
                               {"id", New RequestField("id", New ReflectionFlags(True))},
                               {"LastName", New RequestField("LastName", New ReflectionFlags(False, False))}}

End Class