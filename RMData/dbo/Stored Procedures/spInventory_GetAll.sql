﻿CREATE PROCEDURE [dbo].[spInventory_GetAll]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [Id], [ProductID], [Quantity], [PurchasePrice], [PurchaseDate]
	FROM dbo.Inventory;
END