Imports System.Web
Imports Microsoft.VisualBasic

Public Module Globals

    Public Delegate Function CommandDelegate(userRequest As UserRequest) As GenericResponse

    Public Delegate Function AsyncCommandDelegate(userRequest As UserRequest) As Task(Of GenericResponse)


#Region "Methods"

    Public Function GetCachedProperty(Of T)() As T
        Dim PropertyName As String = GetType(T).Name
        Dim CachedModel As T = HttpRuntime.Cache(PropertyName)
        If CachedModel Is Nothing Then
            Dim NewModel As T = Activator.CreateInstance(Of T)
            HttpRuntime.Cache(PropertyName) = NewModel
            Return NewModel
        Else
            Return CachedModel
        End If
    End Function

    Public Sub SetCachedProperty(Of T)(value As Object)
        Dim PropertyName As String = GetType(T).Name
        HttpRuntime.Cache(PropertyName) = value
    End Sub

#End Region

#Region "Functions"

    'Public Sub TryInvoke(Of T, T2)(f As Func(Of T, T2))
    '    Try
    '        f.Invoke(v)
    '    Catch ex As Exception

    '    End Try
    'End Sub

#End Region


End Module
