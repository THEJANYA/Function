Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.Data
Imports System.Data.SqlClient

<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class WS_TRANS
    Inherits System.Web.Services.WebService
    'Example Strucre not full field
  Public Structure transEntryOptions
      Public trader_code As String
      Public o_succ As Boolean
      Public gross_value As Decimal
      Public update_date As Date
      
      Public _succ As Boolean
      Public _msg As String
      Public _data As DataTable
      Public _userId As String
    End Structure
    
 <WebMethod()> _
       Public Function UpdateData(ByRef obj As transEntryOptions) As Boolean
        Dim tran As New Transaction
        tran.BeginOperation(SQLHelper.conStr)
        Dim param As SqlParameter() = New SqlParameter(7) {}

        Try
            param(0) = New SqlParameter("@ReturnCode", SqlDbType.Int, 0, ParameterDirection.Output)
            param(0).Direction = ParameterDirection.Output
            param(1) = New SqlParameter("@Msg", SqlDbType.VarChar, 255, ParameterDirection.Output)
            param(1).Direction = ParameterDirection.Output
            param(2) = New SqlParameter("@Serverity", SqlDbType.VarChar, 15, ParameterDirection.Output)
            param(2).Direction = ParameterDirection.Output
            param(3) = New SqlParameter("@userid", SQLHelper.ConvertStr(obj.UserID))

            param(4) = New SqlParameter("@trans_no", SQLHelper.ConvertStr(obj.ws_trans_no))
            param(5) = New SqlParameter("@response_code", SQLHelper.ConvertStr(obj.ws_response_code))
            param(6) = New SqlParameter("@response_desc", SQLHelper.ConvertStr(obj.ws_response_desc))
            param(7) = New SqlParameter("@status_flag", SQLHelper.ConvertStr(obj.ws_status_flag))

            SQLHelper.ExecuteNonQuery("NameOfStored", CommandType.StoredProcedure, param, tran.Connection, tran.Transaction)

            obj.ReturnCode = CInt(param(0).Value)
            obj.Msg = CStr(param(1).Value)
            obj.Serverity = getServerity(param(2))
            obj.storeStatus = "NameOfStored"
            If obj.ReturnCode <> 0 Then
                tran.RollbackAndClose()
                Exit Function
            End If
        Catch ex As Exception
            dbgLog.write("Error Update --> " & ex.ToString())
            obj.ReturnCode = CInt(param(0).Value)
            obj.Msg = CStr(param(1).Value)
            obj.Serverity = CStr(param(2).Value)
            tran.RollbackAndClose()
            Exit Function
        End Try
        tran.CommitAndClose()
        Return True
    End Function

    Public Class WSObjOptBase

        Private _succ As Boolean
        Private _msg As String
        Private _data As DataTable
        Private _userId As String

        Public Property Success() As Boolean
            Get
                Return _succ
            End Get
            Set(ByVal value As Boolean)
                _succ = value
            End Set
        End Property

        Public Property Message() As String
            Get
                Return _msg
            End Get
            Set(ByVal value As String)
                _msg = value
            End Set
        End Property

        Public Property ReturnData() As DataTable
            Get
                Return _data
            End Get
            Set(ByVal value As DataTable)
                _data = value
            End Set
        End Property

        Public Property UserId() As String
            Get
                Return _userId
            End Get
            Set(ByVal value As String)
                _userId = value
            End Set
        End Property

    End Class

End Class    
