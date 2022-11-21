CREATE TABLE [dbo].[Food]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[UserId] NVARCHAR(128) NOT NULL, 
    [Name] NVARCHAR(255) NOT NULL, 
    [ServingSize] NVARCHAR(25) NOT NULL, 
    [CarbQty] DECIMAL NOT NULL, 
    [CalorieQty] DECIMAL NOT NULL, 
    [DateAdded] DATETIME2 NOT NULL DEFAULT getutcdate(),
	
	

)
