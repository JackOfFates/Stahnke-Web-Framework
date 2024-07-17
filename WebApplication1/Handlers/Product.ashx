<%@ WebHandler Language="VB" Class="Product" %>

Imports System
Imports System.Web
Imports MicroLibrary.Serialization
Imports StahnkeFramework

Public Class Product : Inherits PageHandler

    Sub New()
        MyBase.New(New ProductController)
    End Sub

End Class