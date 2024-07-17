'Imports System
'Imports System.Collections.Generic
'Imports System.Data.Entity
'Imports System.Linq
'Imports MicroLibrary.Serialization
'Imports Microsoft.VisualBasic
'Imports StahnkeFramework

'Namespace Database

'	Partial Public Class Seller

'#Region "Exists"

'		Public Shared Function ExistsInDatabase(Seller As Seller) As Boolean
'			Return ExistsInDatabase(Seller.sellerid)
'		End Function

'		Public Shared Function ExistsInDatabase(Username As String) As Boolean
'			Using db As New stahnkeEntities2
'				ExistsInDatabase = ExistsInDatabase(db, Username)
'			End Using
'		End Function

'		Public Shared Function ExistsInDatabase(db As stahnkeEntities2, Username As String) As Boolean
'			Try
'				Return db.sellers.Where(Function(x) x.username = Username).Count > 0
'			Catch ex As Exception
'				Return False
'			End Try
'		End Function

'		Public Shared Function ExistsInDatabase(SellerID As Integer) As Boolean
'			Using db As New stahnkeEntities2
'				ExistsInDatabase = ExistsInDatabase(db, SellerID)
'			End Using
'		End Function

'		Public Shared Function ExistsInDatabase(db As stahnkeEntities2, SellerID As Integer) As Boolean
'			Try
'				Return db.sellers.Where(Function(x) x.sellerid = SellerID).Count > 0
'			Catch ex As Exception
'				Return False
'			End Try
'		End Function

'#End Region

'#Region "Shared"

'		Public Shared Function Register(s As Seller) As GenericResponse
'			Dim Result As New GenericResponse
'			Try
'				Dim db As New stahnkeEntities2
'				db.sellers.Add(s)
'				db.SaveChanges()
'				Result.Set(True, "Seller account created.")
'				db.Dispose()
'			Catch ex As Exception
'				Result.Set(False, "An error has occured while attempting to modify the database.")
'			End Try
'			Return Result
'		End Function

'		Public Shared Function Update(sellerAccount As Seller, newSellerAccount As Seller) As GenericResponse
'			Dim Result As New GenericResponse
'			Try
'				Dim db As New stahnkeEntities2
'				Dim sellerAcct As Seller = db.sellers.Where(Function(x) x.username = sellerAccount.username).FirstOrDefault
'				CurrentRegistrationModel.Properties.ForEach(
'					Sub(p As PropertyReference)
'						Dim UserValue As Object = p.Info.GetValue(newSellerAccount, Nothing)
'						Select Case p.Info.Name.ToLower
'							Case "id" 'SKIP
'                            Case Else
'								p.Info.SetValue(sellerAcct, UserValue, Nothing)
'						End Select
'					End Sub)
'				db.SaveChanges()
'				Result.Set(True, "Account Updated.")
'				db.Dispose()
'			Catch ex As Exception
'				Result.Set(False, "An error has occured while attempting to modify the database.")
'			End Try
'			Return Result
'		End Function

'		Public Shared Function GetProducts(username As String) As List(Of Product)
'			Return GetProducts(New stahnkeEntities2, username)
'		End Function

'		Public Shared Function GetProducts(db As stahnkeEntities2, username As String) As List(Of Product)
'			Dim foundProducts As IQueryable(Of Product) = db.products.Where(Function(x) x.owner.ToLower = username.ToLower)
'			Return foundProducts.Where(Function(p) p.published.ToLower = "true").ToList
'		End Function

'#End Region


'	End Class

'End Namespace