<%@ WebHandler Language="VB" Class="Seller" %>

Imports System
Imports System.Web
Imports MicroLibrary.Serialization
Imports StahnkeFramework

Public Class Seller : Inherits PageHandler

    Sub New()
        MyBase.New(New SellerController)
    End Sub

End Class