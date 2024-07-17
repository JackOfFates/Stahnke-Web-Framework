Imports System.Net.Http
Imports System.Web
Imports System.Web.Http.Hosting

Namespace Controllers
    Public MustInherit Class DataController

        Public Property Parent As UserRequest
        Public Property Context As HttpContext

        Public Sub New(Parent As UserRequest, Context As HttpContext)
            Me.Parent = Parent
            Me.Context = Context
        End Sub

    End Class
End Namespace
