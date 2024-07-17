Imports System.Runtime.CompilerServices
Imports System.Web
Imports Newtonsoft.Json
Imports System.Data.Entity
Imports System.Data.Entity.Core.Metadata.Edm
Imports System.Data.Entity.Infrastructure
Imports System.Linq
Imports StahnkeFramework

Namespace System.Data.Entity
    Public NotInheritable Class DbContextExtensions
        Private Sub New()

        End Sub

        Public Shared Function GetKeyNames(Of TEntity As Class)(context As DbContext) As String()
            Return GetKeyNames(context, GetType(TEntity))
        End Function


        Public Shared Function GetKeyNames(context As DbContext, entityType As Type) As String()
            Dim metadata = DirectCast(context, IObjectContextAdapter).ObjectContext.MetadataWorkspace

            ' Get the mapping between CLR types and metadata OSpace
            Dim objectItemCollection = DirectCast(metadata.GetItemCollection(DataSpace.OSpace), ObjectItemCollection)

            ' Get metadata for given CLR type
            Dim entityMetadata = metadata.GetItems(Of EntityType)(DataSpace.OSpace).[Single](Function(e) objectItemCollection.GetClrType(e) = entityType)

            Return entityMetadata.KeyProperties.[Select](Function(p) p.Name).ToArray()
        End Function
    End Class
End Namespace

Public Module Extensions

	Public Delegate Function ResponseDelegate(Of T)(Arg As T) As GenericResponse

#Region "ForEach"

	<Extension()>
	Public Sub ForEach(Of T)(array As T(), a As Action(Of T))
		For Each obj As T In array
			a.Invoke(obj)
		Next
	End Sub

	<Extension()>
	Public Sub ForEach(Of T, T2)(array As T(), a As Func(Of T, T2), Callback As Action(Of T, T2))
		For Each obj As T In array
			Callback.Invoke(obj, a.Invoke(obj))
		Next
	End Sub

	<Extension()>
	Public Sub ForEach(Of T)(Collection As IEnumerable, a As Action(Of T))
		For Each obj As T In Collection
			a.Invoke(obj)
		Next
	End Sub

#End Region

#Region "Dictionary"

	<Extension()>
	Public Sub AddRange(Of TKey, TValue)(Dictionary As Dictionary(Of TKey, TValue), Collection As Array)
		Collection.ForEach(Of KeyValuePair(Of TKey, TValue))(Sub(kv) Dictionary.Add(kv.Key, kv.Value))
	End Sub

#End Region

#Region "WriteJson"

	<Extension()>
	Public Sub WriteJson(Context As HttpContext, SerializableObject As Object, clearResponse As Boolean)
		WriteJson(Context.Response, Serialize(SerializableObject), clearResponse)
	End Sub

	<Extension()>
	Public Sub WriteJson(Response As HttpResponse, SerializableObject As Object, clearResponse As Boolean)
		WriteJson(Response, Serialize(SerializableObject), clearResponse)
	End Sub

	<Extension()>
	Public Sub WriteJson(Context As HttpContext, SerializableObject As Object)
		WriteJson(Context.Response, Serialize(SerializableObject), True)
	End Sub

	<Extension()>
	Public Sub WriteJson(Response As HttpResponse, SerializableObject As Object)
		WriteJson(Response, Serialize(SerializableObject), True)
	End Sub

	<Extension()>
	Public Sub WriteJson(Context As HttpContext, Json As String, clearResponse As Boolean)
		WriteJson(Context.Response, Json, clearResponse)
	End Sub

	<Extension()>
	Public Sub WriteJson(Context As HttpContext, Json As String)
		WriteJson(Context.Response, Json, True)
	End Sub

	<Extension()>
	Public Sub WriteJson(Response As HttpResponse, Json As String)
		WriteJson(Response, Json, True)
	End Sub

	Public Sub WriteJson(Response As HttpResponse, Json As String, clearResponse As Boolean)
		If clearResponse Then Response.Clear()
		Response.ContentType = "application/json"
		Response.Write(Json & vbNewLine)
		Response.Flush()
	End Sub

#End Region

	<Extension()>
	Public Function Serialize(o As Object) As String
		Return Newtonsoft.Json.JsonConvert.SerializeObject(o)
	End Function

	<Extension()>
	Public Function isJSON(s As String) As Boolean
		Return s.Trim.StartsWith("{")
	End Function

    <Extension()>
    Public Function Compare(A As String, B As String) As Boolean
        Return A.Trim.ToLower = B.Trim.ToLower
    End Function

    <Extension()>
    Public Function MethodNameIsProperty(A As String) As Boolean
        Return A.StartsWith("SET_") Or A.StartsWith("GET_") Or A.StartsWith("ADD_") Or A.StartsWith("REMOVE_")
    End Function

    <Extension()>
    Public Function Capitalize(A As String) As String
        Try
            Return A.Remove(1).ToUpper & A.Remove(0, 1).ToLower
        Catch ex As Exception
            Return A
        End Try
    End Function

    <Extension()>
    Public Function GUIDToString(A As String) As String
        Return A.Replace("-", "")
    End Function

    <Extension()>
    Public Function GUIDToString(A As Guid) As String
        Return A.ToString.Replace("-", "")
    End Function

End Module