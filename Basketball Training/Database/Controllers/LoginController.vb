Imports System.Data.Entity
Imports System.Data.Entity.Core.Metadata.Edm
Imports MicroSerializationLibrary.Serialization
Imports StahnkeFramework
Imports StahnkeFramework.Security

Class LoginController
    Inherits PageController

#Region "Required"
    ' DO NOT EDIT, REQUIRED CODE. 
    Public db As New basketballEntities11()
    Overrides Function GetDbContext() As DbContext
        Return db
    End Function
    Public Sub New(Permissions As ColumnSettings())
        MyBase.New(Permissions, GetType(Account))
    End Sub
    Public Sub New()
        MyBase.New(GetType(Account))
    End Sub

    Public Function currentSession() As Session
        Try
            Return Newtonsoft.Json.JsonConvert.DeserializeObject(Of Session)(currentRequest.HttpContext.Request.Cookies("SFT").Value)
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

#End Region

    Public Overrides Sub ProcessInputModifiers()
        NewCommandModifier("Do_Login", "Password", AddressOf EncryptPlainPassword)
        NewCommandModifier("Do_Register", "Password", AddressOf EncryptPlainPassword)
        NewCommandModifier("Do_Register", "ProfileType", Function(Value As Object) CByte(Value))
    End Sub

    Private Const Salt As String = "gfhj34tu9sergjerptsau9"
    Public Function EncryptPlainPassword(pw As String) As String
        Return Encryption.Encrypt(pw, Salt)
    End Function

#Region "SQL"

    Public Enum ProfileType
        Player
        PlayerGuardian
        Trainer
    End Enum

#End Region

    <CustomVariables({"Username", "Password"})>
    Public Function Do_Login(userRequest As UserRequest) As GenericResponse
        Dim S As New SessionLoader(userRequest)
        Dim defaultResponse As New GenericResponse(False, "Did not pass initialization.")
        If Not S.Success Then
            Dim a As GenericResponse = S.NewSession(Me, userRequest)
            Dim Username As String = userRequest.UserData.PostCollection.Item("Username")
            Dim Password As String = userRequest.UserData.PostCollection.Item("Password")
            Try
                Dim acct As Account = db.Accounts.Find({Username}), StoredPW As String = acct.Password
                If Password.Compare(StoredPW) Then : Return a
                Else : defaultResponse.Data = "Invalid Username or Passsword."
                End If
            Catch ex As Exception
                defaultResponse.Data = "Could not find user."
            End Try
        End If
        Return defaultResponse
    End Function

    <CustomVariables({"Username", "Password", "Email", "ProfileType"})>
    Public Function Do_Register(userRequest As UserRequest) As GenericResponse
        Dim S As New SessionLoader(userRequest)
        Dim defaultResponse As New GenericResponse(False, "Failed to create account.")
        If Not S.Success Then
            Dim a As GenericResponse = S.NewSession(Me, userRequest)
            If Not a.Success Then
                defaultResponse.Data = ""
                Return defaultResponse
            End If
            Dim acct As Account = db.Accounts.Create()
            DeserializationWrapper.GetPropertyReferences(acct).ForEach(
                Sub(p)
                    Dim n As String = p.Info.Name.Capitalize
                    Dim v As Object = userRequest.UserData.PostCollection.Item(n)
                    p.Info.SetValue(acct, v)
                End Sub)
            Try
                db.Accounts.Add(acct)
                db.SaveChanges()
                Return a
            Catch ex As Exception
                defaultResponse.Data = "Username already exists in our DB."
            End Try
        End If
        Return defaultResponse
    End Function

    Public Function Get_Session(userRequest As UserRequest) As GenericResponse
        Dim isActive As Boolean = (userRequest.HttpContext.Request.Cookies("SFT") IsNot Nothing)
        Dim defaultResponse As New GenericResponse(False, "No active session found.")
        If isActive Then
            defaultResponse.Success = True
            defaultResponse.Data = userRequest.HttpContext.Request.Cookies("SFT").Value
        End If
        Return defaultResponse
    End Function

End Class
