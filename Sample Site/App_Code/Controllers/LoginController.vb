Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Net
Imports System.Web
Imports Database
Imports MicroLibrary.Serialization
Imports Microsoft.VisualBasic
Imports StahnkeFramework
Imports StahnkeFramework.Controllers
Imports StahnkeFramework.Security

Public Class LoginController
    Inherits PageController

    Public Sub New()
        Me.AutoWireUp = True
    End Sub

    Public Function Update(userRequest As UserRequest) As GenericResponse


        Dim newAccount As account = Account(userRequest.HttpContext)
        Dim StaticUsername As String = newAccount.Username
        CurrentRegistrationModel.Properties.ForEach(
           Sub(p As PropertyReference)
               Dim PropertyName As String = p.Info.Name.ToLower

               Dim UserValue As String = userRequest.UserData.PostCollection(PropertyName)
               Select Case PropertyName.ToLower
                   Case "accounttype"
                       ' Confirm value is allowed
                       Dim AllowedValues As New List(Of String) From {"STANDARD", "SELLER"}
                       Dim v As String = UserValue.ToUpper
                       If Not AllowedValues.Contains(v) Then
                           UserValue = "STANDARD"
                       ElseIf v = "SELLER" Then
                           ' Check if seller account exists, if not then create it
                           If Not Database.Seller.ExistsInDatabase(userRequest.HttpContext.Account.Username) Then
                               RegisterSeller(userRequest)
                           End If
                       End If
                   Case "password"
                       If UserValue Is Nothing OrElse UserValue.Trim = "" Then
                           UserValue = newAccount.Password
                       Else
                           ' Set Encrypted Password
                           Dim PlainTextPassword As String = UserValue
                           Dim Salt As String = Encryption.DestroyString(StaticUsername)
                           Dim EncryptedPassword As String = Encryption.Encrypt(PlainTextPassword, Salt)
                           UserValue = EncryptedPassword
                       End If
                   Case "username"
                       UserValue = StaticUsername
               End Select

               p.Info.SetValue(newAccount, UserValue, Nothing)
           End Sub)
        Dim Result As GenericResponse = Database.account.Update(userRequest.HttpContext.Account, newAccount)
        Return Result
    End Function

    Public Function Logout(userRequest As UserRequest) As GenericResponse
        userRequest.HttpContext.Response.SetCookie(New HttpCookie("SessionID", Nothing) With {.Expires = DateTime.Now.AddYears(-1)})
        userRequest.HttpContext.Session.Remove("SessionID")
        userRequest.HttpContext.Session.Abandon()
        Return New GenericResponse(True, Nothing)
    End Function

    Public Function Login(userRequest As UserRequest) As GenericResponse
        Dim Result As New GenericResponse

        'Detect Current Session
        If userRequest.HttpContext.Request.Cookies("SessionID") IsNot Nothing Then
            ' Session Already Exists, Resume
            Dim Session As session = SqlSession(userRequest.HttpContext)
            If Session.isValid Then
                Dim NewCookie As New HttpCookie("SessionID", Session.SessionID) With {.Expires = Session.Expires}
                userRequest.HttpContext.Response.SetCookie(NewCookie)
                If userRequest.HttpContext.Session("SessionID") Is Nothing Then
                    userRequest.HttpContext.Session.Add("SessionID", Session)
                Else
                    userRequest.HttpContext.Session("SessionID") = Session
                End If
                Result.Set(True, Session)
            Else
                Result.Set(False, "Session Expired.")
                userRequest.HttpContext.Response.Cookies("SessionID").Expires = DateTime.Now.AddDays(-1)
                userRequest.HttpContext.Session.Abandon()
            End If
        Else
            ' Create new Login Session
            Dim Credentials As New NetworkCredential(userRequest.UserData.PostCollection("username"),
                                                             userRequest.UserData.PostCollection("password"),
                                                             userRequest.HttpContext.Request.Url.Host)
            Result.Set(Database.account.Login(Credentials))
            If Result.Success Then
                Dim Session As session = CType(Result.Data, session)
                userRequest.HttpContext.Response.Cookies.Add(New HttpCookie("SessionID", Session.SessionID) With
                                     {.Expires = Session.Expires})
                If userRequest.HttpContext.Session("SessionID") Is Nothing Then
                    userRequest.HttpContext.Session.Add("SessionID", Session)
                Else
                    userRequest.HttpContext.Session("SessionID") = Session
                End If
            End If
        End If
        Return Result
    End Function

    Public Function RegisterSeller(userRequest As UserRequest) As GenericResponse
        Dim newSellerAccount As New Database.Seller

        CurrentSellerModel.Properties.ForEach(
               Sub(p As PropertyReference)
                   Dim PropertyName As String = p.Info.Name.ToLower
                   Dim UserValue As String = userRequest.UserData.PostCollection(PropertyName)
                   Select Case PropertyName

                   End Select

                   p.Info.SetValue(newSellerAccount, UserValue, Nothing)
               End Sub)
        Return Database.Seller.Register(newSellerAccount)
    End Function

    Public Function Register(userRequest As UserRequest) As GenericResponse
        Dim Result As New GenericResponse
        Dim newAccount As New account

        CurrentRegistrationModel.Properties.ForEach(
           Sub(p As PropertyReference)
               Dim PropertyName As String = p.Info.Name.ToLower
               Dim UserValue As Object = userRequest.UserData.PostCollection(PropertyName)
               If Not UserValue = Nothing Then
                   Select Case PropertyName
                       Case "accounttype"
                           ' Confirm value is allowed
                           Dim AllowedValues As String() = {"STANDARD", "SELLER"}
                           Dim v As String = UserValue.ToUpper
                           If Not AllowedValues.Contains(v) Then
                               UserValue = "STANDARD"
                           ElseIf v = "SELLER" Then
                               ' Check if seller account exists, if not then create it
                               Try
                                   If Not Database.Seller.ExistsInDatabase(userRequest.UserData.PostCollection("username")) Then
                                       Dim r As GenericResponse = RegisterSeller(userRequest)
                                       If Not r.Success Then userRequest.HttpContext.WriteJson(r)
                                   End If
                               Catch ex As Exception
                               End Try
                           End If
                       Case "password"
                           ' Set Encrypted Password
                           Dim Username As String = userRequest.UserData.PostCollection("username")
                           Dim PlainTextPassword As String = UserValue
                           Dim Salt As String = Encryption.DestroyString(Username)
                           Dim EncryptedPassword As String = Encryption.Encrypt(PlainTextPassword, Salt)
                           UserValue = EncryptedPassword
                   End Select
                   p.Info.SetValue(newAccount, CTypeDynamic(UserValue, p.Info.PropertyType), Nothing)
               End If
           End Sub)
        Result.Set(Database.account.Register(newAccount))
        Return Result
    End Function

End Class
