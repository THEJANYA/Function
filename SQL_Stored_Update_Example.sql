USE [NameOfDataBase]
GO
/****** Object:  StoredProcedure [dbo].[NameOfStored]    Script Date: 06/15/17 9:49:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Janya.b
-- Create date: 14/06/2017
-- =============================================
ALTER PROCEDURE [dbo].[NameOfStored]
	(
   @ReturnCode int output,      
   @Msg varchar(255) output,      
   @Serverity varchar(15) output,      
   @userid varchar(20),

  @trans_no varchar (20) output
 ,@response_code varchar(20)
 ,@response_desc varchar(255)
 ,@status_flag varchar (1)
)  

AS
--set @trans_no  = '170127000007'
--set	 @response_code  = '755'
--set	 @response_desc ='Date must not be in the past Test'
BEGIN


	declare @r_code int,
			@operation varchar(20),
			@webservice_name varchar(20),
			@store_name varchar(50),
			@screen_name varchar(50),
			@table_name varchar(50),
			@start_msg varchar(100),
			@end_msg varchar(100),
			@filter_sql_statement varchar(1000),
			@sys_msg varchar(4000),
			@wavGroup int,
			@wavSeqNo int,
			@cntpurpose int

	select  @r_code = 0,
			@operation = 'insert',
			@webservice_name = 'WS_TRANS_ENTRY_FOREIGN',
			@store_name = 'sp_ws_cbs_update',
			@screen_name = 'Release Webservice',
			@table_name = 'EQ_trans_transfer_for_ws_cbs',
			@start_msg = 'Start ==> @' + @store_name,
			@end_msg = 'End ==> @' + @store_name,
			@filter_sql_statement = ''

exec EQ_Log_Proc @operation, @webservice_name, @store_name, null, @screen_name, @table_name, null, @r_code, @start_msg, null, @userid 

  /*************************************************************************************************************/      
 /*  check parameter         */      
 /*************************************************************************************************************/      
 if @trans_no IS NULL OR      
    @response_code IS NULL OR 
    @response_desc    IS NULL   
 
  begin      
   set @r_code = -1      
   exec EQ_ErrorMsg_Search_Proc @r_code, @store_name, @ReturnCode output, @Msg output, @Serverity output      
   exec EQ_Log_Proc @operation, @webservice_name, @store_name, null, @screen_name, @table_name, null, @r_code, @Msg, null, @UserID      
   return      
  end   

 /*************************************************************************************************************/      
 /*  update data         */      
 /*************************************************************************************************************/    
BEGIN TRY          
			UPDATE NameOfTable 
			SET
					response_code   = @response_code,
					response_desc   = @response_desc,
					status_flag     = @status_flag
			
			WHERE trans_no = @trans_no
END TRY
BEGIN CATCH
			SET @sys_msg = ERROR_MESSAGE()
			SET @r_code = -2
			
--Write log--
			EXEC EQ_ErrorMsg_Search_Proc @r_code, @store_name, @ReturnCode output, @Msg output, @Serverity output      
			EXEC EQ_Log_Proc @operation, @webservice_name, @store_name, null, @screen_name, @table_name, @filter_sql_statement, @sys_msg, @userid
return
END CATCH
	EXEC EQ_ErrorMsg_Search_Proc @r_code, @store_name, @ReturnCode output, @Msg output, @Serverity output      
	EXEC EQ_Log_Proc @operation, @webservice_name, @store_name, null, @screen_name, @table_name, @filter_sql_statement, @sys_msg, @userid

END
