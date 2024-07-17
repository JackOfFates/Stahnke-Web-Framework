Imports System.Reflection
Imports MicroLibrary.Serialization
Imports Microsoft.VisualBasic
Imports Database
Imports StahnkeFramework
Imports System.Collections.Generic

Public Class SellerModel
	Inherits RequestModel(Of Seller)

	Public Shadows Property SpecialFields As New Dictionary(Of String, RequestField) From {
							   {"sellerid", New RequestField("sellerid", New ReflectionFlags(True))},
							   {"username", New RequestField("username", New ReflectionFlags(True))},
							   {"businessname", New RequestField("businessname", New ReflectionFlags(True))},
							   {"businesswebsite", New RequestField("businesswebsite", New ReflectionFlags(False, False))},
							   {"businessbiography", New RequestField("businessbiography", New ReflectionFlags(False, False))},
							   {"businesslocation", New RequestField("businesslocation", New ReflectionFlags(False, False))}}

End Class