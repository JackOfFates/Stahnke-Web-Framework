'Imports System
'Imports Microsoft.VisualBasic
'Imports StahnkeFramework

'Namespace Database

'	Partial Public Class Product

'#Region "Shared"

'		Public Shared Function PostProduct(p As Product) As GenericResponse
'			Return PostProduct(New stahnkeEntities2, p)
'		End Function

'		Public Shared Function PostProduct(db As stahnkeEntities2, p As Product) As GenericResponse
'			Dim response As New GenericResponse
'			Try
'				db.products.Add(p)
'				db.SaveChanges()
'				response.Set(True, "Product Posted.")
'			Catch ex As Exception
'				response.Set(False, "Failed to modify the database.")
'			End Try
'			Return response
'		End Function

'		Public Shared Function RemoveProduct(p As Product) As GenericResponse
'			Return RemoveProduct(New stahnkeEntities2, p)
'		End Function

'		Public Shared Function RemoveProduct(db As stahnkeEntities2, p As Product) As GenericResponse
'			Dim response As New GenericResponse
'			Try
'				db.products.Remove(p)
'				db.SaveChanges()
'				response.Set(True, "Product removed.")
'			Catch ex As Exception
'				response.Set(False, "Failed to modify the database.")
'			End Try
'			Return response
'		End Function

'#End Region

'	End Class

'#Region "Enums"

'	Public Enum PriceTypes
'		OneTime
'		Daily
'		Weekly
'		Monthly
'		Quarterly
'		Yearly
'		Biannual
'	End Enum

'	Public Enum ProductCategories
'		Recreational
'		Business
'		Artistic
'	End Enum

'	Public Enum ProductTags
'		Indoor
'		Outdoor
'		Service
'		Digital
'	End Enum

'#End Region

'End Namespace