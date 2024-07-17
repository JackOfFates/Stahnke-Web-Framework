Imports Microsoft.VisualBasic

Imports StahnkeFramework

Public Module Globals

#Region "Cached Properties"

    Public Property CurrentRegistrationModel As RegistrationModel
        Get
            Return GetCachedProperty(Of RegistrationModel)()
        End Get
        Set(value As RegistrationModel)
            SetCachedProperty(Of RegistrationModel)(value)
        End Set
    End Property

    Public Property CurrentSellerModel As SellerModel
        Get
            Return GetCachedProperty(Of SellerModel)()
        End Get
        Set(value As SellerModel)
            SetCachedProperty(Of SellerModel)(value)
        End Set
    End Property

    Public Property CurrentProductModel As ProductModel
        Get
            Return GetCachedProperty(Of ProductModel)()
        End Get
        Set(value As ProductModel)
            SetCachedProperty(Of ProductModel)(value)
        End Set
    End Property

#End Region

End Module
