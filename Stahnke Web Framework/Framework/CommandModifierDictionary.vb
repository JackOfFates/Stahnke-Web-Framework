Public Class cModKvp

    Public ReadOnly Property MethodName As String
    Public ReadOnly Property FieldName As String

    Public ReadOnly Property Modifiers As New Dictionary(Of String, CommandModifier)

    Public Sub New(MethodName As String, FieldName As String, Modifiers As IEnumerable(Of CommandModifier))
        _MethodName = MethodName.ToUpper
        _FieldName = FieldName
        Modifiers.ForEach(Sub(cm As CommandModifier) AddModifier(cm))
    End Sub

    Public Sub AddModifier(cm As CommandModifier)
        Modifiers.Add(cm.FieldName, cm)
    End Sub

    Public Sub UpdateModifier(cm As CommandModifier)
        Modifiers(cm.FieldName) = cm
    End Sub
End Class

Public Class CommandModifier

    Public Property FieldName As String
    Public Property ModifierFunction As Func(Of Object, Object)

    Public Sub New(FieldName As String, ModifierFunction As Func(Of Object, Object))
        Me.FieldName = FieldName
        Me.ModifierFunction = ModifierFunction
    End Sub

End Class
