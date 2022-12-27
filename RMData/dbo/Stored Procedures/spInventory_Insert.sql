CREATE PROCEDURE [dbo].[spInventory_Insert]
	--[ProductID], [Quantity], [PurchasePrice], [PurchaseDate]
	@ProductId int,
	@Quantity int,
	@PurchasePrice money,
	@PurchaseDate datetime2
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO dbo.Inventory(ProductID,Quantity,PurchasePrice,PurchaseDate)
	VALUES (@ProductId, @Quantity, @PurchasePrice, @PurchaseDate);
END