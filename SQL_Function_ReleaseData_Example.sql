USE NameOfDataBase
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Janya.B
-- Create date: 02/05/2017
-- Description:	Return CostCenter HO 
-- =============================================


ALTER FUNCTION [dbo].[fnGetAC-CostCenterHO]
(
)
RETURNS varchar(6)
AS
BEGIN
	 
	DECLARE @CC_HO varchar(6) = '108307'
	
	RETURN @CC_HO
END
