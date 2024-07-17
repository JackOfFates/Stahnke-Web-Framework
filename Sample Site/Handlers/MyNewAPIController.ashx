<%@ WebHandler Language="VB" Class="MyNewAPI" %>

Imports System
Imports System.Web
Imports MicroLibrary.Serialization
Imports StahnkeFramework

Public Class MyNewAPI : Inherits PageHandler

    Sub New()
        MyBase.New(New MyNewAPIController)
    End Sub

End Class