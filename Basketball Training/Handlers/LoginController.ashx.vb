Imports System.Web
Imports System.Web.Services
Imports StahnkeFramework

Public Class LoginControllerHandler
    Inherits PageHandler

    Sub New()
        MyBase.New(New LoginController({New ColumnSettings()}) With {
                        .AutoWireUp = New List(Of AutoWireUpOptions) From
                                {AutoWireUpOptions.CustomPublicMethods, AutoWireUpOptions.Javascript}})
    End Sub

End Class