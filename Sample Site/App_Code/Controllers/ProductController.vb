Imports System
Imports System.Linq
Imports MicroLibrary.Serialization
Imports Microsoft.VisualBasic
Imports StahnkeFramework

Public Class ProductController
	Inherits PageController

	Public Sub New()
		Me.AutoWireUp = True
	End Sub

	Public Sub Post(userRequest As UserRequest)
        Dim CanCreate As Boolean = True

        Select Case userRequest.RequestType
			Case "ADD"
				Dim Result As New GenericResponse
				Dim newProduct As New Database.Product With {.owner = userRequest.HttpContext.Account.Username, .published = "false"}
				CurrentProductModel.Properties.ForEach(
					   Sub(p As PropertyReference)
						   Dim PropertyName As String = p.Info.Name.ToLower
                           Dim UserValue As Object = userRequest.UserData.PostCollection(PropertyName)
                           If Not UserValue = Nothing Then
                               ' Check if values are valid
                               Select Case PropertyName
								   Case "category"
									   Dim r As Database.ProductCategories = Database.ProductCategories.Recreational
									   Dim success As Boolean = [Enum].TryParse(Of Database.ProductCategories)(UserValue, r)
									   If Not success Then
										   CanCreate = False
										   Result.Set(False, "Please specify a valid category.")
									   End If
								   Case "tags"
									   Dim r As Database.ProductTags = Database.ProductTags.Indoor
									   Dim success As Boolean = [Enum].TryParse(Of Database.ProductTags)(UserValue, r)
									   If Not success Then
										   CanCreate = False
										   Result.Set(False, "Please specify a valid tag.")
									   End If
								   Case "pricetype"
									   Dim r As Database.PriceTypes = Database.PriceTypes.OneTime
									   Dim success As Boolean = [Enum].TryParse(Of Database.PriceTypes)(UserValue, r)
									   If Not success Then
										   CanCreate = False
										   Result.Set(False, "Please specify a valid price type.")
									   End If
								   Case "price"
									   Dim price As Double = 0.0
									   If Not Double.TryParse(UserValue, price) Then
										   CanCreate = False
										   Result.Set(False, "Please specify a valid price.")
									   End If
								   Case "published"
									   Dim AllowedValues As String() = {"true", "false"}
									   Dim v As String = UserValue.ToLower
									   If Not AllowedValues.Contains(v) Then
										   UserValue = "false"
									   End If
							   End Select
							   p.Info.SetValue(newProduct, CTypeDynamic(UserValue, p.Info.PropertyType), Nothing)
						   End If
					   End Sub)
				If CanCreate Then Result.Set(Database.Product.PostProduct(newProduct))
				userRequest.HttpContext.WriteJson(Result)
			Case "REMOVE"

			Case "UPDATE"

		End Select
	End Sub

	Public Sub GetPost(userRequest As UserRequest)
		Select Case userRequest.RequestType
			Case "PRODUCTS"
				userRequest.HttpContext.WriteJson(Database.Seller.GetProducts(userRequest.HttpContext.Request.QueryString("seller")))
		End Select
	End Sub

End Class
