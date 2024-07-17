Imports System.Data.Entity.Core.Metadata.Edm
Imports MicroSerializationLibrary.Serialization
Imports Microsoft.VisualBasic

Public Class ControllerCommand

    Public Property CommandName As String
    Public Property ControllerFunction As CommandDelegate
    Public Property dbParameters As New List(Of String)
    Public Property CustomVariableAttribute As CustomVariablesAttribute

    Public Sub New(CommandName As String, ControllerFunction As CommandDelegate)
        Me.CommandName = CommandName.ToUpper
        _ControllerFunction = ControllerFunction
    End Sub

End Class
