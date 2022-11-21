CREATE PROCEDURE [dbo].[spFood_Insert]
	@Id				int output,
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
	VALUES (@UserId, @Name, @ServingSize, @CarbQty, @CalorieQty, @DateAdded);

	SELECT @Id = Scope_Identity();
END
