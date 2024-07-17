Imports MicroLibrary.Serialization
Imports Microsoft.VisualBasic
Imports StahnkeFramework

Public Class SellerController
    Inherits PageController

    Public Sub New()
        Me.AutoWireUp = True
    End Sub

    Public Sub Update(userRequest As UserRequest)
        Dim newSellerAccount As Database.seller = Extensions.Seller(userRequest.HttpContext)
        CurrentSellerModel.Properties.ForEach(
       Sub(p As PropertyReference)
           Dim PropertyName As String = p.Info.Name
           Dim UserValue As String = userRequest.UserData.PostCollection(PropertyName.ToLower)
           p.Info.SetValue(newSellerAccount, UserValue, Nothing)
	   End Sub)

        Dim Result As GenericResponse = Database.seller.Update(userRequest.HttpContext.Seller, newSellerAccount)
        userRequest.HttpContext.WriteJson(Result)
    End Sub
End Class
