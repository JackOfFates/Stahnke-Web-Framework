Imports System.Data.Entity
Imports System.Data.Entity.Core.Metadata.Edm
Imports System.Reflection
Imports System.Web
Imports System.Linq
Imports System.Web.SessionState
Imports MicroSerializationLibrary.Serialization
Imports Microsoft.VisualBasic
Imports StahnkeFramework.JavaScript
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.ComponentModel.DataAnnotations
Imports System.Collections.Specialized
Imports System.Data.Entity.Infrastructure
Imports System.Data.SqlClient

Public Class PageHandler : Implements IHttpHandler, IRequiresSessionState

    Public Property Controller As PageController
    Public Property CommandModifiers As New Dictionary(Of String, cModKvp)

    Public Event OnRequest(ByRef userRequest As UserRequest)

    Public Sub New(Controller As PageController)
        If Not DeserializationWrapper.ExcludedMethods.Contains("GetDbContext") Then DeserializationWrapper.ExcludedMethods.Add("GetDbContext")
        InitializePage(Controller)
    End Sub

    Private Sub InitializePage(Controller As PageController)
        Me.Controller = Controller
        If Controller.AutoWireUp.Contains(AutoWireUpOptions.CustomPublicMethods) Then AutoWireupCustomMethods(Controller)
        If Controller.AutoWireUp.Contains(AutoWireUpOptions.GeneratedEdmTables) Then AutoWireupGeneratedEdmFunctions(Controller)
        If Controller.AutoWireUp.Contains(AutoWireUpOptions.Javascript) Then AutoWireupJS(Controller)
    End Sub

    Private Sub AutoWireupCustomMethods(Controller As PageController)
        DeserializationWrapper.GetMethodReferences(Controller.GetType, DeserializationWrapper.ReflectionFlags, True).ForEach(
                   Sub(p As MethodReference) If p.Info.ReturnType = GetType(GenericResponse) Then Controller.RegisterMethod(p))
    End Sub

    Private Function AddFunction(e As EntityType, userRequest As UserRequest) As GenericResponse
        Dim db As DbContext = Controller.GetDbContext
        Dim a As Assembly = Assembly.GetAssembly(Controller.GetType)
        Dim FoundType As Type = Nothing
        ForEach(Of Type)(a.GetExportedTypes, Sub(t) If t.Name = e.Name Then FoundType = t)
        Dim DataSet As DbSet = db.Set(FoundType)
        Dim Keys As New List(Of String), Params As New List(Of String)
        Dim mName As String = userRequest.RequestType
        Dim isCustom As Boolean = False
        Dim nStr As String, nStr2 As String
        Dim TableName As String = ""
        Try
            If Not mName.Contains("_") Then
                isCustom = True
                Dim sname As String = mName & "_"
                nStr2 = sname.Split("_")(0)
                nStr = sname.Split("_")(1)
            Else
                nStr2 = mName.Split("_")(0)
                nStr = mName.Split("_")(1)
            End If
        Catch ex As Exception
        End Try
        Dim mFunctionType As String = nStr2.Capitalize
        Dim mNamePretty As String = nStr.Capitalize
        For Each P As PropertyReference In DeserializationWrapper.GetPropertyReferences(db)
            If TableName <> "" Then Exit For
            Dim obj As Object = P.Info.GetValue(db)
            Dim et As Type = obj.GetType
            If et.GenericTypeArguments(0) = FoundType Then
                TableName = P.Info.Name
                Exit For
            End If
        Next
        Dim KeyName As String, KeyValue As String
        Dim MissingResponse As GenericResponse = CheckForMissingParams(userRequest, FoundType, TableName)
        If MissingResponse.Success Then
            userRequest.UserData.PostCollection.Keys.ForEach(Of String)(
                         Sub(i As String)
                             Dim v As String = userRequest.UserData.PostCollection(i)
                             Keys.Add(v)
                         End Sub)
            KeyName = userRequest.UserData.PostCollection.Keys(0)
            KeyValue = userRequest.UserData.PostCollection(KeyName)
            Params.AddRange({TableName, KeyName, KeyName.Capitalize})
        Else
            Return MissingResponse
        End If

        Dim SqlString = String.Format("INSERT INTO {0} VALUES ('" & String.Join("' ,'", Keys) & "')", Params(0))
        Dim Instance As DbSqlQuery = DataSet.SqlQuery(SqlString, New SqlParameter(KeyName, KeyValue))
        Return New GenericResponse(True, Instance)
    End Function

    Private Function CheckForMissingParams(userRequest As UserRequest, FoundType As Type, TableName As String, Optional RequiredOnly As Boolean = False) As GenericResponse
        Dim MissingFields As List(Of MissingSqlFieldException) = _CheckForMissingParams(userRequest, FoundType, TableName)
        If MissingFields.Count > 0 Then
            Return New GenericResponse(False, MissingFields)
        Else
            Return New GenericResponse(True, Nothing)
        End If
    End Function

    Private Function _CheckForMissingParams(userRequest As UserRequest, FoundType As Type, TableName As String, Optional RequiredOnly As Boolean = False) As List(Of MissingSqlFieldException)
        Dim r As New List(Of MissingSqlFieldException)
        If userRequest.UserData.PostCollection Is Nothing Then
            For Each p As PropertyReference In DeserializationWrapper.GetPropertyReferences(FoundType)
                Dim pName As String = p.Info.Name
                r.Add(New MissingSqlFieldException(TableName, pName))
            Next
            Return r
        Else
            Return r
        End If
    End Function

    Private Function GetFunction(e As EntityType, userRequest As UserRequest) As GenericResponse
        ' Try
        Dim db As DbContext = Controller.GetDbContext
        Dim a As Assembly = Assembly.GetAssembly(Controller.GetType)
        Dim FoundType As Type = Nothing
        ForEach(Of Type)(a.GetExportedTypes, Sub(t) If t.Name = e.Name Then FoundType = t)
        Dim DataSet As DbSet = db.Set(FoundType)
        Dim mName As String = userRequest.RequestType, KeyName As String, KeyValue As String, nStr As String, nStr2 As String, isCustom As Boolean = False
        Dim TableName As String = ""
        Try
            If Not mName.Contains("_") Then
                isCustom = True
                Dim sname As String = mName & "_"
                nStr2 = sname.Split("_")(0)
                nStr = sname.Split("_")(1)
            Else
                nStr2 = mName.Split("_")(0)
                nStr = mName.Split("_")(1)
            End If
        Catch ex As Exception
        End Try
        Dim mFunctionType As String = nStr2.Capitalize
        Dim mNamePretty As String = nStr.Capitalize
        For Each P As PropertyReference In DeserializationWrapper.GetPropertyReferences(db)
            If TableName <> "" Then Exit For
            Dim obj As Object = P.Info.GetValue(db)
            Dim et As Type = obj.GetType
            If et.GenericTypeArguments(0) = FoundType Then
                TableName = P.Info.Name
                Exit For
            End If
        Next
        Dim MissingResponse As GenericResponse = CheckForMissingParams(userRequest, FoundType, TableName)
        If MissingResponse.Success Then
            KeyName = userRequest.UserData.PostCollection.Keys(0)
            KeyValue = userRequest.UserData.PostCollection(KeyName)
            'Params.AddRange({TableName, KeyName, KeyName.Capitalize})
            Dim Instance As Object = DataSet.Find({KeyValue})
            Dim ReturnObj As Object = Activator.CreateInstance(Instance.GetType)
            Dim propreferences As List(Of PropertyReference) = DeserializationWrapper.GetPropertyReferences(Instance)
            Dim DictPropReferences As New Dictionary(Of String, PropertyReference)
            propreferences.ForEach(Sub(i As PropertyReference) DictPropReferences.Add(i.Info.Name, i))
            propreferences.ForEach(
                                       Sub(propref As PropertyReference)
                                           If Controller.ColumnPermissions.ContainsKey(propref.Info.Name) Then
                                               Dim Permissions As ColumnSettings = Controller.ColumnPermissions(propref.Info.Name)
                                               If Permissions.CanRead Then propref.Info.SetValue(ReturnObj, DictPropReferences(propref.Info.Name).Info.GetValue(Instance))
                                           Else
                                               propref.Info.SetValue(ReturnObj, DictPropReferences(propref.Info.Name).Info.GetValue(Instance))
                                           End If
                                       End Sub)
            Return New GenericResponse(True, ReturnObj)

        Else
            Return MissingResponse
        End If

        'Catch ex As Exception
        '    Return New GenericResponse(False, ex.Message)
        'End Try
    End Function
    ' ---------------- PUT ON HOLD ---------------- 
    Private Sub AutoWireupGeneratedEdmFunctions(Controller As PageController)
        For Each e As EntityType In Controller.GetMetaList

            If Not Controller.ExcludedEdmProperties.Contains("GET_" & e.Name.ToUpper) Then
                Controller.RegisterCommand("Get_" & e.Name, New CommandDelegate(Function(u As UserRequest) GetFunction(e, u)))
            End If

            If Not Controller.ExcludedEdmProperties.Contains("SET_" & e.Name.ToUpper) Then
                Controller.RegisterCommand("Set_" & e.Name, New CommandDelegate(
                            Function(userRequest As UserRequest)

                                Dim db As DbContext = Controller.GetDbContext
                                Dim a As Assembly = Assembly.GetAssembly(Controller.GetType)
                                Dim FoundType As Type = Nothing
                                ForEach(Of Type)(a.GetExportedTypes, Sub(t) If t.Name = e.Name Then FoundType = t)
                                Dim DataSet As DbSet = db.Set(FoundType)
                                Dim KeyName As String = userRequest.UserData.PostCollection.Keys(0)
                                Dim KeyValue As String = userRequest.UserData.PostCollection(KeyName)
                                Dim NewKeyValues As New Dictionary(Of String, String)
                                Dim TableName As String = ""
                                DeserializationWrapper.GetPropertyReferences(db).ForEach(
                               Sub(p As PropertyReference)
                                   If TableName <> "" Then Return
                                   Dim obj As Object = p.Info.GetValue(db)
                                   Dim et As Type = obj.GetType
                                   If et.GenericTypeArguments(0) = FoundType Then
                                       TableName = p.Info.Name
                                       Return
                                   End If
                               End Sub)
                                userRequest.UserData.PostCollection.Keys.ForEach(Of String)(Sub(i As String) If i.StartsWith("New") Then NewKeyValues.Add(i.Remove(0, 3), userRequest.UserData.PostCollection(i)))
                                Dim Instance2 As DbSqlQuery = DataSet.SqlQuery(String.Format("SELECT * FROM {0} WHERE {1} = @{2}", {TableName, KeyName, KeyName.Capitalize}), New SqlParameter(KeyName, KeyValue))
                                Dim ReturnObj As Object = Nothing
                                Instance2.ForEach(Sub(aee As Object) If ReturnObj Is Nothing Then ReturnObj = aee)
                                Dim propreferences As List(Of PropertyReference) = DeserializationWrapper.GetPropertyReferences(ReturnObj)
                                Dim DictPropReferences As New Dictionary(Of String, PropertyReference)
                                propreferences.ForEach(Sub(i As PropertyReference) DictPropReferences.Add(i.Info.Name, i))

                                propreferences.ForEach(
                                   Sub(propref As PropertyReference)
                                       If NewKeyValues.ContainsKey(propref.Info.Name) Then
                                           Dim o As String = NewKeyValues.Item(propref.Info.Name)
                                           If Controller.ColumnPermissions.ContainsKey(propref.Info.Name) Then
                                               Dim Permissions As ColumnSettings = Controller.ColumnPermissions(propref.Info.Name)
                                               If Permissions.CanWrite Then propref.Info.SetValue(ReturnObj, o)
                                           Else
                                               propref.Info.SetValue(ReturnObj, o)
                                           End If
                                       End If
                                   End Sub)
                                Dim returnedint As Integer = db.SaveChanges()
                                Return New GenericResponse(returnedint > 0, returnedint)

                            End Function))
            End If

            If Not Controller.ExcludedEdmProperties.Contains("ADD_" & e.Name.ToUpper) Then
                Controller.RegisterCommand("Add_" & e.Name, New CommandDelegate(Function(u As UserRequest) AddFunction(e, u)))
            End If

            If Not Controller.ExcludedEdmProperties.Contains("REMOVE_" & e.Name.ToUpper) Then
                Controller.RegisterCommand("Remove_" & e.Name, New CommandDelegate(
                               Function(userRequest As UserRequest)
                                   ' Try
                                   Dim db As DbContext = Controller.GetDbContext
                                   Dim a As Assembly = Assembly.GetAssembly(Controller.GetType)
                                   Dim FoundType As Type = Nothing
                                   ForEach(Of Type)(a.GetExportedTypes, Sub(t) If t.Name = e.Name Then FoundType = t)
                                   Dim DataSet As DbSet = db.Set(FoundType)
                                   Dim KeyName As String = userRequest.UserData.PostCollection.Keys(0)
                                   Dim KeyValue As String = userRequest.UserData.PostCollection(KeyName)
                                   Dim Instance As Object = DataSet.Find({KeyValue})
                                   Dim ReturnObj As Object = Activator.CreateInstance(Instance.GetType)
                                   Dim propreferences As List(Of PropertyReference) = DeserializationWrapper.GetPropertyReferences(Instance)
                                   Dim DictPropReferences As New Dictionary(Of String, PropertyReference)
                                   propreferences.ForEach(Sub(i As PropertyReference) DictPropReferences.Add(i.Info.Name, i))
                                   propreferences.ForEach(
                                       Sub(propref As PropertyReference)
                                           If Controller.ColumnPermissions.ContainsKey(propref.Info.Name) Then
                                               Dim Permissions As ColumnSettings = Controller.ColumnPermissions(propref.Info.Name)
                                               If Permissions.CanRead Then propref.Info.SetValue(ReturnObj, DictPropReferences(propref.Info.Name).Info.GetValue(Instance))
                                           Else
                                               propref.Info.SetValue(ReturnObj, DictPropReferences(propref.Info.Name).Info.GetValue(Instance))
                                           End If
                                       End Sub)
                                   Return New GenericResponse(True, ReturnObj)
                                   'Catch ex As Exception
                                   '    Return New GenericResponse(False, ex.Message)
                                   'End Try
                End Function))
            End If

        Next

    End Sub

    Private Sub AutoWireupJS(Controller As PageController)
        Dim Commands As New List(Of ControllerCommand)
        Dim AG As New APIGen(Controller)
        Dim s As String = AG.Generate()
        Dim LocalPath As String = Assembly.GetExecutingAssembly().GetName().CodeBase.Replace("file:///", Nothing)
        LocalPath = LocalPath.Remove(LocalPath.LastIndexOf("/"))
        LocalPath = LocalPath.Remove(LocalPath.LastIndexOf("/"))
        Try
            If Not My.Computer.FileSystem.DirectoryExists(LocalPath & "/js") Then My.Computer.FileSystem.CreateDirectory(LocalPath & "/js")
        Catch ex As Exception
        End Try
        Try
            If Not My.Computer.FileSystem.DirectoryExists(LocalPath & "/js/Controllers") Then My.Computer.FileSystem.CreateDirectory(LocalPath & "/js/Controllers")
        Catch ex As Exception
        End Try
        Try
            If Not My.Computer.FileSystem.DirectoryExists(LocalPath & "/js/Main.js") Then My.Computer.FileSystem.WriteAllText(LocalPath & "/js/Main.js", My.Resources.Main, False)
        Catch ex As Exception
        End Try
        Try
            If Not My.Computer.FileSystem.DirectoryExists(LocalPath & "/js/jquery-1.11.2.min.js") Then My.Computer.FileSystem.WriteAllText(LocalPath & "/js/jquery-1.11.2.min.js", My.Resources.jquery_1_11_2_min, False)
        Catch ex As Exception
        End Try
        Try
            If Not My.Computer.FileSystem.DirectoryExists(LocalPath & "/js/QueryBuilder.js") Then My.Computer.FileSystem.WriteAllText(LocalPath & "/js/QueryBuilder.js", My.Resources.QueryBuilder, False)
        Catch ex As Exception
        End Try
        Try
            My.Computer.FileSystem.WriteAllText(String.Format("{0}/js/Controllers/{1}.js", {LocalPath, Controller.GetType.Name}), s, False)
        Catch ex As Exception
        End Try
        Controller.RegisterCommands(Commands)
    End Sub

    Public ReadOnly Property IsReusable As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        Dim userRequest As New UserRequest(context, Controller)
        RaiseEvent OnRequest(userRequest)
        If Not userRequest.Handled Then Controller.ProcessRequest(userRequest)
    End Sub

End Class
