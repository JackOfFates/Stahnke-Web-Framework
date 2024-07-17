Imports MicroSerializationLibrary.Serialization
Imports Microsoft.VisualBasic

Public Class RequestModel(Of T)

    Public Property SpecialFields As New Dictionary(Of String, RequestField)

    Public Property Properties As New List(Of PropertyReference)

    Public Function GetMetaData(db As Entity.Database) As ReflectionFlags

        Return Nothing
    End Function

    Public Function GetFlags(Prop As PropertyReference) As ReflectionFlags
        Return GetFlags(Prop, Nothing)
    End Function

    Public Function GetFlags(Prop As PropertyReference, db As Entity.Database) As ReflectionFlags
        If SpecialFields.ContainsKey(Prop.Info.Name) Then
            SpecialFields(Prop.Info.Name).Flags.parent = Prop
            Return SpecialFields(Prop.Info.Name).Flags
        ElseIf db IsNot Nothing Then ' Get from Table metadata
            Return GetMetaData(db)
        End If
        Return RequestField.Default
    End Function

    Public Sub New()
        Dim AllProperties As List(Of PropertyReference) = DeserializationWrapper.GetPropertyReferences(GetType(T))
        For Each P As PropertyReference In AllProperties
            Dim Flags As ReflectionFlags = GetFlags(P)
            If Flags.GetQualifierResult Or (Flags.Require OrElse Not Flags.Ignore) Then
                Properties.Add(P)
            End If
        Next
    End Sub

End Class

Public Class ReflectionFlags
    Inherits Tuple(Of Boolean, Boolean)

    Public Property parent As PropertyReference

    Public Sub New(Ignore As Boolean)
        MyBase.New(Ignore, False)
    End Sub

    Public Sub New(Ignore As Boolean, Require As Boolean)
        MyBase.New(Ignore, Require)
    End Sub

    Public ReadOnly Property Ignore As Boolean
        Get
            Return Item1
        End Get
    End Property

    Public ReadOnly Property Require As Boolean
        Get
            Return Item2
        End Get
    End Property

    Public Property CustomQualifier As Func(Of String, Boolean) = Nothing

    Public Function GetQualifierResult() As Boolean
        If HasCustomQualifier() Then
            Return CustomQualifier.Invoke(parent.Info.Name)
        Else
            Return False
        End If
    End Function

    Private Function HasCustomQualifier() As Boolean
        Return CustomQualifier IsNot Nothing
    End Function

End Class

Public Class RequestField

    Public Shared Property [Default] As New ReflectionFlags(False, True)

    Public Property Flags As ReflectionFlags = [Default]

    Public Sub New(FieldName As String)
        Me.New(FieldName, [Default])
    End Sub

    Public Sub New(FieldName As String, Flags As ReflectionFlags)
        Me.FieldName = FieldName
        Me.Flags = Flags
    End Sub

    Public Property FieldName As String

End Class
