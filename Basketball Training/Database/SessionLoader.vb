Imports MicroSerializationLibrary.Serialization
Imports Newtonsoft.Json
Imports StahnkeFramework

Public Class SessionLoader

    Private userRequest As UserRequest
    Public Const sessionKey As String = "SFT"
    Public Const usernameKey As String = "Username"
    Public Const passwordKey As String = "Password"

    Public Property isValidRequest As Boolean = False
    Public Property Success As Boolean = False
    Public Property Ticket As Guid
    Public Property AutoRenewTicket As Guid

    Public Sub New(userRequest As UserRequest)
        Me.userRequest = userRequest
        If userRequest IsNot Nothing Then
            processRequest()
        End If
    End Sub

    Private Sub processRequest()
        isValidRequest = userRequest.UserData.PostCollection IsNot Nothing AndAlso userRequest.UserData.PostCollection.AllKeys.Contains(sessionKey)
        If isValidRequest Then
            Dim csplit As String() = userRequest.HttpContext.Request.Cookies.Item(sessionKey).Value.Split(",")

            Dim TicketValue As String = Nothing : Try : TicketValue = csplit(0) : Catch ex As Exception : End Try
            Dim RenewTicketValue As String = Nothing : Try : RenewTicketValue = csplit(1) : Catch ex As Exception : End Try
            Success = (TicketValue <> Nothing)

            If Success Then
                ' Already Logged In, Reload Login
                Ticket = Guid.Parse(TicketValue)
                AutoRenewTicket = Guid.Parse(RenewTicketValue)
            End If

        End If
    End Sub

    Public Function NewSession(controller As PageController, userRequest As UserRequest) As GenericResponse
        isValidRequest = userRequest.UserData.PostCollection IsNot Nothing AndAlso (userRequest.UserData.PostCollection.AllKeys.Contains(usernameKey))
        If isValidRequest Then
            Dim c As HttpCookie = userRequest.HttpContext.Request.Cookies(sessionKey)
            Dim Username As String = userRequest.UserData.PostCollection.Item(usernameKey)

            If userRequest.HttpContext.Request.Cookies.AllKeys.Contains(sessionKey) Then
                'Check for session
                Dim ExpiresOn As DateTime = DateTime.Now.AddDays(3)
                Dim cookieToObj As Session = JsonConvert.DeserializeObject(Of Session)(c.Value)

                c.Expires = DateTime.Now.AddDays(12)
                Dim lcontroller As LoginController = DirectCast(controller, LoginController)
                Try
                    Dim sObj As Session = lcontroller.db.Sessions.Find({cookieToObj.SessionID})
                    sObj.RemoteHost = userRequest.HttpContext.Request.UserHostAddress
                    sObj.ExpiresOn = cookieToObj.ExpiresOn
                    cookieToObj = sObj
                Catch ex As Exception
                    cookieToObj.RemoteHost = userRequest.HttpContext.Request.UserHostAddress
                    lcontroller.db.Sessions.Add(cookieToObj)
                End Try
                Try
                    lcontroller.db.SaveChanges()
                Catch ex As Exception

                End Try
                Return New GenericResponse(True, cookieToObj)
            Else
                Dim Password As String = userRequest.UserData.PostCollection.Item(passwordKey)
                Dim ExpiresOn As DateTime = DateTime.Now.AddDays(3)
                Dim s As New Session With {.SessionID = Guid.NewGuid.GUIDToString,
                                             .ExpiresOn = ExpiresOn,
                                             .AutoRenewID = Guid.NewGuid.GUIDToString,
                                             .SessionUser = Username,
                                             .RemoteHost = userRequest.HttpContext.Request.UserHostAddress}
                Dim cNew As HttpCookie = New HttpCookie(sessionKey, s.Serialize)
                Dim lcontroller As LoginController = DirectCast(controller, LoginController)

                lcontroller.db.Sessions.Add(s)
                lcontroller.db.SaveChanges()
                userRequest.HttpContext.Response.Cookies.Add(cNew)
                Return New GenericResponse(True, s)
            End If

        Else
            Return New GenericResponse(False, "Invalid parameter format.")
        End If
    End Function

End Class