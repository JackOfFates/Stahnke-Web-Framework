'Imports System
'Imports System.Linq
'Imports System.Web
'Imports Microsoft.VisualBasic

'Namespace Database

'	Partial Public Class session

'#Region "Shared"

'		Public Shared DefaultSessionExpiration As TimeSpan = TimeSpan.FromDays(7)

'#Region "Exists"

'		Public Shared Function ResolveSession(Context As HttpContext) As session
'			Dim CookieExists As Boolean = Context.Request.Cookies.Item("SessionID") IsNot Nothing
'			If CookieExists Then
'				Dim SessionID_Cookie As String = Context.Request.Cookies.Item("SessionID").Value

'				Return getSessionFromID(Context.Request.Cookies.Item("SessionID").Value)
'			Else
'				Return Nothing
'			End If
'		End Function

'		Public Shared Function ExistsInDatabase(Session As session) As Boolean
'			Return ExistsInDatabase(Session.SessionID)
'		End Function

'		Public Shared Function ExistsInDatabase(SessionID As String) As Boolean
'			Using db As New stahnkeEntities2
'				ExistsInDatabase = db.sessions.Where(Function(x) x.SessionID = SessionID).Count > 0
'			End Using
'		End Function

'		Public Shared Function ExistsInDatabase(db As stahnkeEntities2, SessionID As String) As Boolean
'			Return db.sessions.Where(Function(x) x.SessionID = SessionID).Count > 0
'		End Function

'		Public Shared Function ExistsInDatabase(SessionID As Guid) As Boolean
'			Return ExistsInDatabase(SessionID.ToString)
'		End Function

'#End Region

'#Region "Get"

'		Public Shared Function getSessionFromID(SessionID As Guid) As session
'			Return getSessionFromID(SessionID.ToString)
'		End Function

'		Public Shared Function getSessionFromID(SessionID As String) As session
'			Using db As New stahnkeEntities2
'				getSessionFromID = db.sessions.Where(Function(x) x.SessionID = SessionID).FirstOrDefault
'			End Using
'		End Function

'		Public Shared Function getSessionFromID(db As stahnkeEntities2, SessionID As String) As session
'			Return db.sessions.Where(Function(x) x.SessionID = SessionID).FirstOrDefault
'		End Function

'		Public Shared Function getAccount(db As stahnkeEntities2, session As session) As account
'			Return db.accounts.Where(Function(x) x.Username.Trim.ToLower = session.Username.Trim.ToLower).FirstOrDefault
'		End Function

'		Public Shared Function getAccount(session As session) As account
'			Using db As New stahnkeEntities2
'				getAccount = getAccount(db, session)
'			End Using
'		End Function

'		Public Shared Function getSeller(db As stahnkeEntities2, session As session) As Seller
'			Return db.sellers.Where(Function(x) x.username.Trim.ToLower = session.Username.Trim.ToLower).FirstOrDefault
'		End Function

'		Public Shared Function getSeller(session As session) As Seller
'			Using db As New stahnkeEntities2
'				Try
'					getSeller = getSeller(db, session)
'				Catch ex As Exception
'					getSeller = Nothing
'					Console.WriteLine(ex.Message)
'				End Try
'			End Using
'		End Function

'#End Region

'#Region "isValid"

'		Public Shared Function isValid(db As stahnkeEntities2, SessionID As String) As Boolean
'			Dim SqlSession As session = db.sessions.Where(Function(x) x.SessionID = SessionID).FirstOrDefault
'			Return SqlSession.Expires > DateTime.Now
'		End Function

'		Public Shared Function isValid(SessionID As String) As Boolean
'			Using db As New stahnkeEntities2
'				isValid = isValid(db, SessionID)
'			End Using
'		End Function

'		Public Shared Function isValid(Session As session) As Boolean
'			Using db As New stahnkeEntities2
'				isValid = isValid(db, Session.SessionID)
'			End Using
'		End Function

'		Public Shared Function isValid(ValidUntil As Date) As Boolean
'			Return ValidUntil > DateTime.Now
'		End Function

'#End Region

'#End Region

'		Public Function ExistsInDatabase() As Boolean
'			Return ExistsInDatabase(Me)
'		End Function

'		Public Function ExistsInDatabase(db As stahnkeEntities2) As Boolean
'			Return ExistsInDatabase(db, SessionID)
'		End Function

'		Public Function isValid() As Boolean
'			Return isValid(Expires)
'		End Function

'	End Class

'End Namespace