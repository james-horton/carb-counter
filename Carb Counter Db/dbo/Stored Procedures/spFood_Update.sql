CREATE PROCEDURE [dbo].[spFood_Update]
	@Id				int,
	@UserId			nvarchar(128),
	@Name			nvarchar(255),
	@ServingSize	nvarchar(25), 
	@CarbQty		decimal,
	@CalorieQty		decimal
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE	dbo.Food
	SET		[Name] = @Name, 
			[ServingSize] = @ServingSize, 
			[CarbQty] = @CarbQty, 
			[CalorieQty] = @CalorieQty
	WHERE	Id = @Id
	AND		UserId = @UserId
END

