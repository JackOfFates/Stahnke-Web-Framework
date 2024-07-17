<AttributeUsage(AttributeTargets.Method Or AttributeTargets.Class Or AttributeTargets.Delegate)>
Public Class CustomVariablesAttribute
    Inherits Attribute

    Public Property Variables As String()

    Public Sub New(Variables As String())
        Me.Variables = Variables
    End Sub
End Class
