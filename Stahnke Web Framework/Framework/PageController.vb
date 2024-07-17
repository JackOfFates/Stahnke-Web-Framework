Imports System.Data.Entity
Imports System.Data.Entity.Core.Metadata.Edm
Imports System.Data.Entity.Infrastructure
Imports MicroSerializationLibrary.Serialization
Imports Microsoft.VisualBasic
Imports StahnkeFramework.Security
Imports StahnkeFramework
Imports System.Reflection

Public MustInherit Class PageController

    Public Property AutoWireUp As New List(Of AutoWireUpOptions) From {AutoWireUpOptions.Javascript}
    Public ReadOnly Property CommandModifiers As New Dictionary(Of String, Dictionary(Of String, cModKvp))
    Public Property ColumnPermissions As New Dictionary(Of String, ColumnSettings)
    Public Property ExcludedEdmProperties As New List(Of String)
    Public Property currentRequest As UserRequest
    Public ReadOnly Property EntityType As Type

    MustOverride Function GetDbContext() As DbContext
    Protected Friend Event OnRequest(ByRef userRequest As UserRequest)

    Friend Function GetCommand(CommandName As String) As ControllerCommand
        Return Commands(CommandName.ToUpper)
    End Function
    Private Commands As New SortedDictionary(Of String, ControllerCommand)

    ReadOnly Property CommandList As ControllerCommand()
        Get
            Return Commands.Values.ToArray
        End Get
    End Property

    Public Sub New(Permissions As ColumnSettings(), entityType As Type)
        Try
            Permissions.ForEach(
          Sub(cs As ColumnSettings)
              ColumnPermissions.Add(cs.ColumnName, cs)
          End Sub)
        Catch ex As Exception

        End Try
        Me._EntityType = entityType
    End Sub

    Public Sub New(entityType As Type)
        Me._EntityType = entityType
    End Sub

#Region "Public Methods"

    Public MustOverride Sub ProcessInputModifiers()

    Private metaListToType As New Dictionary(Of String, EntityType)
    Public Function GetMetaList() As EntityType()
        Dim MetaData = DirectCast(Me.GetDbContext, IObjectContextAdapter).ObjectContext.MetadataWorkspace
        Dim items As EntityType() = MetaData.GetItems(Of EntityType)(DataSpace.CSpace).ToArray
        If metaListToType.Count <> items.Count Then items.ForEach(Sub(ee As EntityType) metaListToType.Add(ee.Name, ee))
        Return items
    End Function

    Public Function TypeToEntityType(T As String) As EntityType
        If metaListToType.Count = 0 Then GetMetaList()
        Return metaListToType(T)
    End Function

    Protected Friend Function NewCommandModifier(MethodName As String, SqlFieldName As String, ValueModifier As Func(Of Object, Object)) As CommandModifier
        Dim c As New CommandModifier(SqlFieldName, ValueModifier)
        Dim n As String = MethodName.ToUpper

        If Not CommandModifiers.ContainsKey(n) Then
            CommandModifiers.Add(n, New Dictionary(Of String, cModKvp))
        End If
        CommandModifiers(n).Add(c.FieldName, New cModKvp(MethodName, c.FieldName, {c}))
        Return c
    End Function

    Protected Friend Sub RegisterCommand(CommandName As String, CommandFunction As CommandDelegate)
        RegisterCommand(New ControllerCommand(CommandName, CommandFunction))
    End Sub

    Private Sub RegisterCustomCommand(CommandName As String, CommandFunction As CommandDelegate, CustomVariables As CustomVariablesAttribute)
        Dim cmd As New ControllerCommand(CommandName, CommandFunction) With {.CustomVariableAttribute = CustomVariables}
        RegisterCommand(cmd)
    End Sub

    Protected Friend Sub RegisterCommand(Command As ControllerCommand)
        If Commands.ContainsKey(Command.CommandName) Then
            Commands(Command.CommandName) = Command
        Else
            Commands.Add(Command.CommandName, Command)
            TypeToEntityType(EntityType.Name).DeclaredMembers.ForEach(Sub(e As EdmMember) Command.dbParameters.Add(e.Name))
        End If
    End Sub

    Friend Sub RegisterMethod(methodReference As MethodReference)
        Dim cdlgt As New CommandDelegate(Function(u As UserRequest) methodReference.Info.Invoke(methodReference.Instance, {u}))
        Dim cVar As CustomVariablesAttribute = methodReference.Info.GetCustomAttribute(Of CustomVariablesAttribute)
        RegisterCustomCommand(methodReference.Info.Name, cdlgt, cVar)
    End Sub

    Protected Friend Sub RegisterCommands(Commands As IEnumerable(Of ControllerCommand))
        Commands.ForEach(Of ControllerCommand)(Sub(c) RegisterCommand(c))
    End Sub

    Friend Sub ProcessRequest(ByRef userRequest As UserRequest)
        ' Check if userRequest exists 
        If userRequest IsNot Nothing Then
            RaiseEvent OnRequest(userRequest)
            If Not userRequest.Handled Then
                ' Check if command is registed in the page controller
                If Commands.Keys.Contains(userRequest.RequestType.ToUpper) Then
                    ' Everything checks out. It's time to make the request.
                    Dim getCommandOutput As Object = GetCommand(userRequest.RequestType).ControllerFunction.Invoke(userRequest)
                    userRequest.HttpContext.WriteJson(getCommandOutput)
                    userRequest.Handled = True
                End If
            End If
        End If
    End Sub

#End Region

End Class

Public Enum AutoWireUpOptions
    GeneratedEdmTables
    CustomPublicMethods
    Javascript
End Enum