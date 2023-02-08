CREATE PROCEDURE [dbo].[spUser_Insert]
	@id nvarchar(128),
	@FirstName nvarchar(50),
	@LastName nvarchar(50),
	@EmailAdress nvarchar(256)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO dbo.[User](Id,FirstName,LastName,EmailAdress)
	VALUES (@Id,@FirstName,@LastName,@EmailAdress);
END

