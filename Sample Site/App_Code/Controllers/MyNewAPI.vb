Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Net
Imports System.Web
Imports Database
Imports MicroLibrary.Serialization
Imports Microsoft.VisualBasic
Imports StahnkeFramework
Imports StahnkeFramework.Security

Public Class MyNewAPIController
    Inherits PageController

    Public Sub New()
        Me.AutoWireUp = True
    End Sub


    Public Function CreateHash(userRequest As UserRequest) As GenericResponse

    End Function

End Class
