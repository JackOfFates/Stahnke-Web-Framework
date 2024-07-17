Imports System.Collections.Specialized
Imports System.Diagnostics
Imports System.IO
Imports System.Text
Imports System.Threading
Imports System.Web
Imports Microsoft.VisualBasic
Imports Newtonsoft.Json
Imports StahnkeFramework.Controllers

Public Class UserRequest

    ''' <summary>
    ''' Maximum POST bandwidth. Default: 500 (500 kbps)
    ''' </summary>
    Public Shared SpeedLimit As Double = 500

    ''' <summary>
    ''' Total maximum POST size in bytes. Default: 128000 (128kb)
    ''' </summary>
    Public Shared MaxRequestSize As Long = 128000

    ''' <summary>
    ''' Buffer size for client download operations. Default: 32768 (32KB)
    ''' </summary>
    Public Shared ClientBufferSize As Long = 32768

    ''' <summary>
    ''' Buffer size for server upload operations. Default: 32768 (32KB)
    ''' </summary>
    'Public Shared ServerBufferSize As Long = 32768

    Private Context As HttpContext

    Public ReadOnly Controller As PageController

#Region "Properties"

    Public Property UserData As PostController

    Public Property Handled As Boolean = False

    Public ReadOnly Property HttpRequestType As String
        Get
            Return Context.Request.RequestType
        End Get
    End Property

    Public ReadOnly Property HttpContext As HttpContext
        Get
            Return Context
        End Get
    End Property

    Public ReadOnly Property RequestType As String
        Get
            Return _RequestType
        End Get
    End Property
    Private _RequestType As String = String.Empty

#End Region

    Public Function GetRequestType() As String
        Dim requestName As String = Context.Request.QueryString("t")
        If requestName IsNot Nothing Then
            Return requestName.ToUpper
        End If
        Return _RequestType
    End Function

    Public Sub New(Context As HttpContext, Controller As PageController)
        Me.Context = Context
        Me.Controller = Controller
        _RequestType = GetRequestType()
        UserData = New PostController(Me)
    End Sub


End Class