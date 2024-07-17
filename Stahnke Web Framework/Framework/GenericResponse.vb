Imports Microsoft.VisualBasic

Public Class GenericResponse

    Public Overloads Sub [Set](Success As Boolean, Data As Object)
        Me.Success = Success
        Me.Data = Data
    End Sub

    Public Overloads Sub [Set](Response As GenericResponse)
        [Set](Response.Success, Response.Data)
    End Sub

    Public Sub New()

    End Sub

    Public Sub New(Success As Boolean, Data As Object)
        [Set](Success, Data)
    End Sub

    Public Property Data As Object

    Public Property Success As Boolean

End Class