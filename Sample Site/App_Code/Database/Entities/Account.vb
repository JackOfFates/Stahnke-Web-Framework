Imports System
Imports System.Linq
Imports System.Net
Imports MicroLibrary.Serialization
Imports Microsoft.VisualBasic
Imports StahnkeFramework
Imports StahnkeFramework.Security

'Namespace Database

'	Partial Public Class account

'#Region "Shared"

'		Public Shared Function Update(Account As account, newAccount As account) As GenericResponse
'			Dim Result As New GenericResponse
'			Try
'                Dim db As New stahnkeEntities2
'                Dim acct As account = db.accounts.Where(Function(x) Account.Username = x.Username).FirstOrDefault
'				CurrentRegistrationModel.Properties.ForEach(
'					Sub(p As PropertyReference)
'                        Dim UserValue As Object = p.Info.GetValue(newAccount, Nothing)
'                        Select Case p.Info.Name.ToLower
'                            Case "id", "username" 'SKIP
'                            Case Else
'                                p.Info.SetValue(acct, UserValue, Nothing)
'                        End Select
'					End Sub)
'				db.SaveChanges()
'				Result.Set(True, "Account Updated.")
'				db.Dispose()
'			Catch ex As Exception
'				Result.Set(False, "An error has occured while attempting to modify the database.")
'			End Try
'			Return Result
'		End Function

'		Public Shared Function Register(a As account) As GenericResponse
'			Dim Result As New GenericResponse
'			Try
'                Dim db As New stahnkeEntities2
'                db.accounts.Add(a)
'				db.SaveChanges()
'				Result.Set(True, "Account created.")
'				db.Dispose()
'			Catch ex As Exception
'				Result.Set(False, "An error has occured while attempting to modify the database.")
'			End Try
'			Return Result
'		End Function

'		Public Shared Function Login(Credentials As NetworkCredential) As GenericResponse
'			Dim Result As New GenericResponse
'			Dim db As New stahnkeEntities2
'			Dim AccountsFound As IQueryable(Of account) = db.accounts.Where(Function(a) a.Username.Trim.ToLower = Credentials.UserName.Trim.ToLower)

'			If AccountsFound.Count > 0 Then
'				Dim UserAccount As account = AccountsFound.FirstOrDefault
'				Dim Salt As String = Encryption.DestroyString(UserAccount.Username)
'				Dim EncryptedPassword As String = Encryption.Encrypt(Credentials.Password, Salt)
'				If UserAccount.Password = EncryptedPassword Then
'					Dim newSession As New session With {
'						.SessionID = Guid.NewGuid.ToString,
'						.Username = Credentials.UserName,
'						.Expires = DateTime.Now.Add(session.DefaultSessionExpiration)}
'					db.sessions.Add(newSession)
'					db.SaveChanges()
'					Dim Exists As Boolean = newSession.ExistsInDatabase(db)
'					db.Dispose()

'					If Exists Then
'						Result.Set(True, newSession)
'					Else
'						Result.Set(False, "There was an error adding session to the database.")
'					End If
'				Else
'					Result.Set(False, "Invalid Password.")
'				End If
'			Else
'				Result.Set(False, "Invalid Username.")
'			End If
'			Return Result
'		End Function

'#Region "Exists"

'		Public Shared Function ExistsInDatabase(Username As String) As Boolean
'			Return ExistsInDatabase(New stahnkeEntities2, Username)
'		End Function

'		Public Shared Function ExistsInDatabase(Session As session) As Boolean
'			If Session IsNot Nothing Then
'				Return ExistsInDatabase(New stahnkeEntities2, Session.Username)
'			Else
'				Return False
'			End If
'		End Function

'		Public Shared Function ExistsInDatabase(db As stahnkeEntities2, Username As String) As Boolean
'			ExistsInDatabase = db.accounts.Where(Function(x) x.Username.ToLower = Username.ToLower).Count > 0
'		End Function

'#End Region

'#End Region

'	End Class

'End Namespace