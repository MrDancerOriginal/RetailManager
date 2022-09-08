CREATE PROCEDURE [dbo].[spUserLookup]
	@id nvarchar(128)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT Id,FirstName,LastName,EmailAdress,CreatedDate
	FROM [dbo].[User]
	WHERE Id = @id;
END
