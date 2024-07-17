Imports System.Reflection
Imports MicroLibrary.Serialization
Imports Microsoft.VisualBasic
Imports Database
Imports StahnkeFramework
Imports System.Collections.Generic

Public Class ProductModel
    Inherits RequestModel(Of Product)

    Public Shadows Property SpecialFields As New Dictionary(Of String, RequestField) From {
    {"id", New RequestField("id", New ReflectionFlags(True))},
    {"category", New RequestField("category", New ReflectionFlags(False, True))},
    {"tags", New RequestField("tags", New ReflectionFlags(False, True))},
    {"title", New RequestField("title", New ReflectionFlags(False, True))},
    {"description", New RequestField("description", New ReflectionFlags(False, True))},
    {"pricetype", New RequestField("pricetype", New ReflectionFlags(False, True))},
    {"price", New RequestField("price", New ReflectionFlags(False, True))},
    {"owner", New RequestField("owner", New ReflectionFlags(True))},
    {"published", New RequestField("published", New ReflectionFlags(False, False))}}

End Class
