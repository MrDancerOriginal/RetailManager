﻿CREATE PROCEDURE [dbo].[spSale_SaleReport]

AS
BEGIN
	SET NOCOUNT ON;

	SELECT [s].[CashierId], [s].[SaleDate], [s].[SubTotal], [s].[Tax], [s].[Total], u.FirstName, u.LastName, u.EmailAdress
	FROM dbo.Sale s
	INNER JOIN dbo.[User] u ON s.CashierId = u.Id
END