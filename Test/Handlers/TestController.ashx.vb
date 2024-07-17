Imports System.Web
Imports System.Web.Services
Imports StahnkeFramework

Public Class TestController
    Inherits PageHandler

    Sub New()
        MyBase.New(New TestControllerBase)
    End Sub

End Class