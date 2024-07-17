Public Class MissingSqlFieldException
    Public ReadOnly Property Message As String

    Public ReadOnly Property TableName As String
    Public ReadOnly Property FieldKey As String

    Public Sub New(TableName As String, FieldKey As String)
        _TableName = TableName
        _FieldKey = FieldKey
        _Message = String.Format("Missing field '{0}' under table name '{1}'.", {FieldKey, TableName})
    End Sub

End Class
