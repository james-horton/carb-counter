CREATE PROCEDURE [dbo].[spFood_Insert]
	@Id				int,
	@UserId			nvarchar(128),
	@Name			nvarchar(255),
	@ServingSize	nvarchar(25), 
	@CarbQty		decimal,
	@CalorieQty		decimal, 
	@DateAdded		datetime2
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO dbo.Food (UserId, [Name], ServingSize, CarbQty, CalorieQty, DateAdded)
	OUTPUT INSERTED.Id
	VALUES (@UserId, @Name, @ServingSize, @CarbQty, @CalorieQty, @DateAdded)
END
