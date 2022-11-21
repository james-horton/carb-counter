CREATE PROCEDURE [dbo].[spFood_GetAll]
	@UserId	nvarchar(128)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT	[UserId], [Name], [ServingSize], [CarbQty], [CalorieQty], [DateAdded]
	FROM	dbo.Food
	WHERE	UserId = @UserId;
END
