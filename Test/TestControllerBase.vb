Imports System.Data.Entity
Imports StahnkeFramework

Friend Class TestControllerBase
    Inherits PageController

    Public Overrides Function GetDbContext() As DbContext
        Throw New NotImplementedException()
    End Function

    Public Overrides Sub ProcessInputModifiers()
        Throw New NotImplementedException()
    End Sub

    ' Public Property TestProperty As String = "ABC"
    Public Function returnTest(userrequest As UserRequest) As GenericResponse
        Dim StringToReturn As String = "hello"
        Return New GenericResponse(True, StringToReturn)
    End Function
    'Public Sub returNull(userrequest As UserRequest)

    'End Sub

    Public Sub New()
        MyBase.New(GetType(String))
    End Sub

End Class
