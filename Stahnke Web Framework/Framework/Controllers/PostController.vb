Imports System.Collections.Specialized
Imports System.IO
Imports System.Text
Imports System.Web
Imports System
Imports MicroSerializationLibrary

Namespace Controllers

    Public Class PostController
        Inherits DataController

        Public Sub New(Parent As UserRequest)
            MyBase.New(Parent, Parent.HttpContext)
            PopulatePostData()
        End Sub

        Public Property PostCollection As NameValueCollection

        Public Function ReadPostData() As GenericResponse
            Dim Result As New GenericResponse
            Try

                Dim sw As New Stopwatch : sw.Start()
                Dim bufferSize As Integer = UserRequest.ClientBufferSize
                Dim buffer As [Byte]() = New [Byte](bufferSize - 1) {}

                Dim readBytes As Long = Context.Request.GetBufferlessInputStream().Read(buffer, 0, buffer.Length),
                    CurrentReadBytes As Long = readBytes,
                    CurrentReadBits As Long = readBytes * 8,
                    ms As New MemoryStream,
                    Timeout As Boolean = False,
                    timeoutcount As Integer = 0

                Do
                    Dim kbps As Long = CLng(Math.Max(CurrentReadBits, 1) / Math.Max(1, sw.ElapsedMilliseconds))
                    If Math.Floor(kbps) = 0 Then
                        timeoutcount += 1
                        If timeoutcount > 4000 Then
                            Timeout = True : Exit Do
                        End If
                    End If

                    If kbps <= UserRequest.SpeedLimit Then
                        ms.Write(buffer, 0, readBytes)
                        readBytes = Context.Request.GetBufferlessInputStream().Read(buffer, 0, buffer.Length)
                        CurrentReadBytes += readBytes
                        CurrentReadBits += readBytes * 8
                        If CurrentReadBytes > UserRequest.MaxRequestSize Then
                            Result.Set(False, "Timed out.")
                            Exit Do
                        End If
                    End If
                    ImprovedSpinWait.SpinFor(1.0)
                Loop While readBytes > 0
                sw.Stop()

                If Not Timeout Then
                    Result.Set(True, ms.ToArray)
                End If
                ms.Dispose()
            Catch ex As Exception
                Result.Set(False, ex.Message)
            End Try
            Return Result
        End Function

        Public Sub PopulatePostData()
            Dim PostResult As GenericResponse = ReadPostData()
            If PostResult.Success Then
                Dim Data As Byte() = PostResult.Data
                Dim DataToString As String = UTF8Encoding.UTF8.GetString(Data)
                If DataToString.Trim = String.Empty Then Return
                ' Update Current Request
                Me.Parent.Controller.currentRequest = Parent
                Try
                    PostCollection = HttpUtility.ParseQueryString(DataToString)
                Catch ex As Exception
                    If PostResult Is Nothing Then PostResult = New GenericResponse(False, "Empty POST.")
                End Try
                Parent.Controller.ProcessInputModifiers()
                ProcessFieldModifiers()
            Else
                Context.WriteJson(PostResult)
            End If

        End Sub

        Private Sub PreProcessInputModifiers()

        End Sub

        Public Sub ProcessFieldModifiers()
            Dim isModified As Boolean = Parent.Controller.CommandModifiers.ContainsKey(Parent.RequestType)

            If isModified Then
                Dim mods As Dictionary(Of String, cModKvp) = Parent.Controller.CommandModifiers(Parent.RequestType)
                For Each v As KeyValuePair(Of String, cModKvp) In mods
                    Dim defaultValue As String = PostCollection.Item(v.Key)
                    Dim modValue As String = defaultValue
                    For Each cm As CommandModifier In mods(v.Key).Modifiers.Values
                        modValue = cm.ModifierFunction.Invoke(modValue)
                    Next
                    PostCollection.Item(v.Key) = modValue 'mods(v.Key)
                Next
            End If

        End Sub
    End Class

End Namespace
