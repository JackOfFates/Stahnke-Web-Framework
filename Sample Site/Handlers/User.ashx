<%@ WebHandler Language="VB" Class="User" %>

Imports System
Imports System.Web
Imports Database
Imports System.Net
Imports MicroLibrary.Serialization
Imports StahnkeFramework

Public Class User : Inherits PageHandler

    Sub New()
        MyBase.New(New LoginController)
    End Sub

End Class
