Imports System
Imports System.Collections.Generic
Imports System.Data.Entity
Imports System.Diagnostics
Imports System.Runtime.CompilerServices
Imports System.Web
Imports Database
Imports Microsoft.VisualBasic
Imports StahnkeFramework

Public Module Extensions


    <Extension()>
    Public Function ToArray(array As DbSet(Of account)) As account()
        Dim newList As New List(Of account)
        For Each acc As account In array
			newList.Add(acc)
		Next
        Return newList.ToArray
    End Function

#Region "ForEach"

    <Extension()>
    Public Sub ForEach(array As DbSet(Of account), a As Action(Of account))
        array.ToArray.ForEach(a)
    End Sub

    <Extension()>
    Public Sub ForEach(Of T)(array As DbSet(Of account), a As Func(Of account, T), Callback As Action(Of account, T))
        array.ToArray.ForEach(a, Callback)
    End Sub

#End Region

    <Extension()>
    Public Function SqlSession(Context As HttpContext) As session
        Try
            Dim SessionID_Cookie As HttpCookie = Context.Request.Cookies("SessionID")
            If SessionID_Cookie IsNot Nothing Then
                Dim SessionID As String = SessionID_Cookie.Value
                If Context.Session("SessionID") Is Nothing Then
                    Dim session As session = session.getSessionFromID(SessionID)
                    Context.Session.Add("SessionID", session)
                End If
                Return Context.Session("SessionID")
            End If
        Catch ex As Exception
        End Try
        Return New session
    End Function

    <Extension()>
    Public Function Account(Context As HttpContext) As account
        Try
            If Context.Session("Account") Is Nothing Then
                Dim acct As account = session.getAccount(SqlSession(Context))
                Context.Session.Add("Account", acct)
                If acct IsNot Nothing Then Return acct
            Else
                Return Context.Session("Account")
            End If
        Catch ex As Exception
        End Try
        Return New account
    End Function

    <Extension()>
    Public Function Seller(Context As HttpContext) As Seller
        Try
            If Context.Session("Seller") Is Nothing Then
                Dim s As Seller = session.getSeller(SqlSession(Context))
                Context.Session.Add("Seller", s)
                If s IsNot Nothing Then Return s
            Else
                Return Context.Session("Seller")
            End If
        Catch ex As Exception
        End Try
        Return New Seller
    End Function

End Module