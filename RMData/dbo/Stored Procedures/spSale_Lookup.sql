﻿CREATE PROCEDURE [dbo].[spSale_Lookup]
	@CashierId nvarchar(128),
	@SaleDate dateTime2
AS
BEGIN
	SET NOCOUNT ON;
	SELECT Id
	FROM dbo.Sale
	WHERE CashierId = @CashierId and SaleDate = @SaleDate
END
