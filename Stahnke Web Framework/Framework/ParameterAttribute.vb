Public Class ParameterAttribute
    Inherits Attribute

    Public Property Parameters As String()

    Public Sub New(Parameters As String())
        Me.Parameters = Parameters
    End Sub

End Class
