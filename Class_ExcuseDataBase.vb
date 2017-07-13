Option Strict On
Option Explicit On
Imports System
Imports System.Data
Imports System.Collections.Generic
Imports BusinessObject.Collections
Imports System.Data.SqlClient


Public Class transentryForeignEquity
    Inherits BusinessObjectBase
    
 'Example Field not full field    
#Region "Field"

Private _trader_code As String
Private _o_succ As Boolean
Private _gross_value As Decimal
Private _update_date As Date

#End Region

 'Example Property not full Property    
#Region "Property"
    Public Property traderCode() As String
        Get
            Return _traderCode
        End Get
        Set(ByVal value As String)
            _traderCode = value
        End Set
    End Property
    Public Property o_succ() As Boolean
        Get
            Return _o_succ
        End Get
        Set(ByVal value As Boolean)
            _o_succ = value
        End Set
    End Property
        Public Property gross_value() As Decimal
        Get
            Return _gross_value
        End Get
        Set(ByVal value As Decimal)
            _gross_value = value
        End Set
    End Property
        Public Property update_date() As Date
        Get
            Return _update_date
        End Get
        Set(ByVal value As Date)
            _update_date = value
        End Set
    End Property
#End Region   

Public Shared Function Update_Data(ByRef obj As transentryForeignEquity) As Boolean

        Dim dt As New transEntrycollections
        Dim ws As New WS_TRANS.WS_TRANS
        Dim opts As New WS_TRANS.transEntryOptions
        Try
            opts.ws_trans_no = obj.ws_trans_no
            opts.ws_response_code = obj.ws_response_code
            opts.ws_response_desc = obj.ws_response_desc
            opts.ws_status_flag = obj.ws_status_flag
            opts.UserID = obj.userId
            ws.UpdateWsCBS(opts)

            Return True
        Catch ex As Exception
            Return False
        End Try

    End Function
End Class    
    
