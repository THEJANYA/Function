#Region "Imports"
Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Data.SqlClient
Imports BusinessObject
Imports System.Net.Security
Imports System.Net
Imports System.Security.Cryptography.X509Certificates
Imports CrystalDecisions
Imports CrystalDecisions.CrystalReports.Engine
#End Region 
Partial Class Transfer_ToWebservice
    Inherits FIPage
Private Function CallWsbservice() As Boolean
        Try

            Dim user As String = CStr(System.Configuration.ConfigurationManager.AppSettings("User"))
            Dim password As String = CStr(System.Configuration.ConfigurationManager.AppSettings("Password"))

            Dim ws As New WS_TRANS.WS_TRANS
            Dim obj As New transentryForeignEquity

            ServicePointManager.ServerCertificateValidationCallback = New RemoteCertificateValidationCallback(AddressOf RemoteCertValidate)
            Dim WsService As WS_Service.Services = New WS_Service.Services()
            WsService.Credentials = New System.Net.NetworkCredential(user, password)
            WsService.PreAuthenticate = True

            Dim DepositRequest As WS_Service.RequestType = New WS_Service.RequestType()  'request
            Dim DepositResponse As WS_Service.ResponseType = New WS_Service.ResponseType()  'respons
      
            Dim strTransNo As String = ""

            gvMain.Rows.Count.ToString()
            For Each row As GridViewRow In gvMain.Rows

                Dim chkAction As CheckBox = CType(row.FindControl("chkAction"), CheckBox)

                If chkAction IsNot Nothing AndAlso chkAction.Checked Then

                    Dim dr As DataRow = listData2.Rows(row.DataItemIndex)
                    Response.Clear()
                    Response.Buffer = True
                    Response.ContentType = "application/text; charset=windows-874"

                    '================== Match Filed Required for send request data to Web Service CBS >> Finance Service =================='
                    If Not dr.IsNull("ws_channel_id") AndAlso Not String.IsNullOrEmpty(CStr(dr("ws_channel_id"))) Then
                        DepositRequest.ChannelId = CStr(dr("ws_channel_id"))
                    Else
                        DepositRequest.ChannelId = "EQ" 
                    End If
                    
                    '================== End of Match Filed Required for send request data to Web Service CBS >> Finance Service =================='

                    '****** Start Send Request to Web Service >> Finance Service  ******'

                    DepositResponse = WsCBS.deposit(DepositRequest)

                    '****** End of Send Request to Web Service >> Finance Service  ******'

                    '================== Start Get Response form Web Service CBS >> Finance Service and match filed for update to database    =================='

                    If Not dr.IsNull("trans_no") AndAlso Not String.IsNullOrEmpty(CStr(dr("trans_no"))) Then

                        If DepositResponse.ResponseHeader.ResponseCode.ToString() = "0" Then
                         
                            obj.ws_status_flag = "S"  'Success
                        Else
                         
                            obj.ws_status_flag = "E" 'Error
                        End If

                        obj.ws_trans_no = CStr(dr("trans_no"))
                        obj.userId = CurrentUser.UserID

                        If DepositResponse.ResponseHeader.ResponseDesc.ToString().Length <> 0 Then
                            obj.ws_response_desc = DepositResponse.ResponseHeader.ResponseDesc.ToString()
                        Else
                            obj.ws_response_desc = "Success"
                        End If

                        If DepositResponse.ResponseHeader.ResponseCode.ToString().Length <> 0 Then
                            obj.ws_response_code = DepositResponse.ResponseHeader.ResponseCode.ToString()
                        Else
                            obj.ws_response_code = "Not Response Code"
                        End If

                        If DepositResponse.ResponseHeader.wsRefId.ToString().Length <> 0 Then

                        End If

                        '****** Start Update Rsponse to database  ******'

                        transentryForeignEquity.Update_WsCBS(obj)

                        '****** End of Update Rsponse to database  ******'

                       
                    Else
                        ' may be wanna do someing
                    End If
                    '================== End of Start Get Response form Web Service CBS >> Finance Service and match filed for update to database  =================='

                End If
            Next

            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function
    
     Private Function RemoteCertValidate(ByVal sender As Object, ByVal cert As X509Certificate, ByVal chain As X509Chain, ByVal [error] As System.Net.Security.SslPolicyErrors) As Boolean
        Return True
    End Function   
End Class
