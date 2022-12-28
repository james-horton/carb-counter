CREATE PROCEDURE [dbo].[spFood_Delete]
	@Id		int,
	@UserId	nvarchar(128)
AS
BEGIN
	DELETE FROM dbo.Food
	WHERE Id = @Id
	AND UserId = @UserId;
END
