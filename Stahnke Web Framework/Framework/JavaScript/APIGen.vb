Imports System.Data.Entity.Core.Metadata.Edm
Imports System.Reflection
Imports MicroSerializationLibrary.Serialization

Namespace JavaScript

    ' TODO: Need to generate this from the class 'LoginController'
    '---------------------------------------------------------------------
    '    Function autoGen_logincontroller() {
    '    //[START:Variables]
    '    var logincontrollerObj = New Object();
    '    var handlerBase = '../Handlers/User.ashx'
    '    var loginQuery = combineQuery(handlerBase, { 't': 'Login' });
    '    //[END:Variables]
    '    //[START:Functions]
    '    Function Login(u, p) {
    '        executeQuery(loginQuery,
    '            {
    '                'username': u,
    '                'password': p
    '            });
    '    }
    '    //[END:Functions]
    '    UserObj.Login = Login;
    '    Return UserObj;
    '}
    '---------------------------------------------------------------------


    '   Function Apple(type) {
    '   this.type = type;
    '   this.getInfo = Function() {
    '       return this.type + ' apple';
    '   };
    '}

    Public Class APIGen
        Public QueryVariables As New Dictionary(Of String, Object)

        Public Property Controller As PageController

        Public Sub New(pageController As PageController)
            Me.Controller = pageController
            pageController.CommandList.ForEach(Sub(cc As ControllerCommand) QueryVariables.Add(cc.CommandName.Capitalize, cc.ControllerFunction.Method))
        End Sub

        Private Function StartJS(t As Type) As String
            StartJS = ("/// <reference path=""../jquery-1.11.2.min.js"" />" & vbNewLine &
                       "/// <reference path=""../QueryBuilder.js"" />" & vbNewLine &
                       "/// <reference path=""../Main.js"" />" & vbNewLine & vbNewLine &
                       "" &
                       "function autoGen_" & t.Name & "() [" & vbNewLine &
                       "    var " & t.Name & "Obj = new Object();" & vbNewLine &
                       "    var Callback = function (data, type) { }" & vbNewLine &
                       "    var handlerBase = '../Handlers/" & t.Name & ".ashx';" & vbNewLine).Replace("[", "{") & vbNewLine
        End Function

        Private Function DictToJS(c As PageController, sourceType As Type, d As Dictionary(Of String, Object), OutputType As jsOutputType) As String
            Dim output As String = ""
            If d.Count > 0 Then

                Select Case OutputType
                    Case jsOutputType.Var
                        ' Define all query variables
                        For i As Integer = 0 To d.Count - 1
                            Dim varName As String = d.Keys(i)
                            Dim isProperty As Boolean = varName.StartsWith("SET_") Or varName.StartsWith("GET_")
                            Dim isFunction As Boolean = varName.ToLower.StartsWith("do")
                            Dim varJS As String = String.Format("    var {0}Query = combineQuery(handlerBase, [ 't': '{0}' ]);", {varName}).Replace("[", "{").Replace("]", "}") & vbNewLine
                            output = output & varJS
                        Next
                    Case jsOutputType.Method
                        ' Create js Function header/w params    
                        '       Function login(u, p) {
                        output = output & vbNewLine
                        Dim MetaList As EntityType() = c.GetMetaList
                        For i As Integer = 0 To d.Count - 1
                            Dim mName As String = d.Keys(i)
                            Dim isProperty As Boolean = mName.StartsWith("SET_") Or mName.StartsWith("GET_") Or mName.StartsWith("REMOVE_") Or mName.StartsWith("ADD_")
                            Dim isCustom As Boolean = False
                            Dim nStr As String, nStr2 As String
                            Try
                                If Not isProperty Then isCustom = True
                                nStr2 = mName.Split("_")(0)
                                nStr = mName.Split("_")(1)
                            Catch ex As Exception
                            End Try
                            Dim mFunctionType As String = nStr2.Capitalize
                            Dim mNamePretty As String = nStr.Capitalize
                            Dim mType As Type = Nothing
                            Dim a As Assembly = Assembly.GetAssembly(c.GetType)
                            Dim ServiceTypes As New Dictionary(Of Type, MethodReference())
                            For Each t As Type In a.GetExportedTypes
                                Try
                                    ServiceTypes.Add(t, DeserializationWrapper.GetMethodReferences(t, DeserializationWrapper.ReflectionFlags, True, True).ToArray)
                                Catch ex As Exception
                                    Exit For
                                End Try
                            Next

                            If isCustom Then
                                mType = c.EntityType
                            Else
                                ServiceTypes.ForEach(Sub(kvp As KeyValuePair(Of Type, MethodReference()))
                                                         Dim ktype As Type = kvp.Key
                                                         Dim methods As MethodReference() = kvp.Value
                                                         methods.ForEach(Sub(m As MethodReference)
                                                                             If mName.Capitalize = m.Info.Name.Capitalize Then
                                                                                 mType = ktype
                                                                             End If
                                                                         End Sub)
                                                     End Sub)
                            End If
                            Dim mInfo As MethodInfo = d(mName)

                            Dim js As String = String.Format("    function {0}([PARAMS]) ", {mName}) & "{" & vbNewLine
                            Dim parameters As New List(Of String)
                            ' Get Table's attribute for required Parameters.
                            If isProperty Or isCustom Then
                                Dim Command As ControllerCommand
                                Dim props As PropertyInfo() = mType.GetProperties
                                Dim Keys As String() = System.Data.Entity.DbContextExtensions.GetKeyNames(c.GetDbContext, mType)
                                Dim Key As String = Keys.First

                                If mFunctionType = "Set" Then parameters.Add(Key)
                                If isCustom Then
                                    For Each cmd As ControllerCommand In c.CommandList
                                        If cmd.CommandName = mName.ToUpper Then
                                            Command = cmd
                                            Exit For
                                        End If
                                    Next

                                    For Each param As String In Command.dbParameters
                                        If Command.CustomVariableAttribute IsNot Nothing Then
                                            If Command.CustomVariableAttribute.Variables.Contains(param) Then parameters.Add(param)
                                        End If
                                    Next
                                End If
                                If isProperty Then
                                    For Each p As PropertyInfo In props

                                        If parameters.Count > 0 AndAlso mFunctionType = "Get" Then Exit For

                                        For Each e As EntityType In MetaList
                                            If e.Name = mNamePretty Then
                                                If GetType(String) Is p.PropertyType Then
                                                    If mFunctionType = "Set" Then
                                                        If Keys(0) <> p.Name Then parameters.Add("New" & p.Name)
                                                    ElseIf mFunctionType = "Add" Then
                                                        For x As Integer = 0 To props.Count - 1
                                                            parameters.Add(props(x).Name)
                                                        Next
                                                    ElseIf mFunctionType = "Remove" Then

                                                    ElseIf mFunctionType = "Get" Then
                                                        parameters.Add(p.Name)
                                                        If parameters.Count > 0 Then Exit For
                                                    End If
                                                End If
                                            End If
                                        Next
                                    Next
                                End If

                            End If

                            js = js.Replace("[PARAMS]", String.Join(",", parameters.ToArray))

                            ' Function Body
                            '        executeQuery(loginQuery,
                            '            {
                            '                'username': u,
                            '                'password': p
                            '            });


                            js = js & String.Format("        return executeQuery({0}Query, ", {mName})

                            If parameters.Count > 0 Then
                                js = js & vbNewLine
                                ' Iterate Parameters
                                '   "'username': u," & vbNewLine & etc....
                                js = js & "            {" & vbNewLine
                                Dim ii As Integer = 0
                                parameters.ForEach(Sub(s As String)
                                                       If ii > 0 Then js = js & "," & vbNewLine
                                                       js = js & String.Format("                '{0}': {0}", {s})
                                                       ii += 1
                                                   End Sub)
                                js = js & vbNewLine & "            }, Callback);"
                            Else
                                js = js & " null);"
                            End If

                            ' Function Footer
                            '       }

                            js = js & vbNewLine & "    }" & vbNewLine
                            'js = js & sourceType.Name & "Obj.login = login;"
                            output = output & js & vbNewLine
                        Next

                        For i As Integer = 0 To d.Count - 1
                            Dim mName As String = d.Keys(i)
                            If mName.StartsWith("get_") Or mName.StartsWith("set_") Then Continue For
                            output = output & String.Format("    {0}Obj.{1} = {1};", {sourceType.Name, mName}) & vbNewLine
                        Next
                        output = output & String.Format("    {0}Obj", {sourceType.Name}) & ".Callback = Callback" & vbNewLine
                        output = output & "    return " & String.Format("{0}Obj;", {sourceType.Name})
                        If Not output.EndsWith(vbNewLine & "}") Then
                            output = output & vbNewLine & "}"
                        End If
                        If Not output.EndsWith("}") Then
                            output = output & "}"
                        End If
                        output = output & vbNewLine & vbNewLine & String.Format("    var {1} = new autoGen_{0}();", {sourceType.Name, sourceType.Name})
                End Select
            End If
            Return output
        End Function

        Private Function TruncateJs(c As PageController, Output As String, sourceType As Type, d As Dictionary(Of String, Object)) As String
            Dim JS_d1 As String = DictToJS(c, sourceType, d, jsOutputType.Var)
            Dim JS_d2 As String = DictToJS(c, sourceType, d, jsOutputType.Method)

            Dim NewOutput As String = String.Empty
            NewOutput = Output & JS_d1
            NewOutput = NewOutput & JS_d2
            Return NewOutput
        End Function

        Public Function Generate() As String

            ' Create the class
            Dim t As Type = Controller.GetType
            Dim output As String = StartJS(t)
            Dim pageController As PageController = DirectCast(Controller, PageController)
            DeserializationWrapper.GetMethodReferences(t, DeserializationWrapper.ReflectionFlags, True).ForEach(
                Sub(r As MethodReference)
                    If r.Info.ReturnType = GetType(GenericResponse) Then
                        Dim n As String = r.Info.Name.Capitalize
                        If QueryVariables.ContainsKey(n) Then
                            QueryVariables(n) = r.Info
                        Else
                            ' Create a query variable for the method
                            QueryVariables.Add(r.Info.Name.Capitalize, r.Info)
                        End If
                    End If
                End Sub)

            output = TruncateJs(Controller, output, t, QueryVariables)

            Return output
        End Function

        Public Enum jsOutputType As Byte
            Var
            Method
        End Enum

    End Class

End Namespace