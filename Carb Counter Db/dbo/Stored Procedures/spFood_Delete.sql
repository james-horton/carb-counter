CREATE PROCEDURE [dbo].[spFood_Delete]
	@Id		int,
	@UserId	nvarchar(128)
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM dbo.Food
	WHERE Id = @Id
	AND UserId = @UserId;
END
