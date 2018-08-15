CREATE TABLE [dbo].[Conversations]
(
	[Id] NVARCHAR(128) NOT NULL PRIMARY KEY, 
    [StartTime] DATETIME NULL, 
    [Name] NVARCHAR(MAX) NULL, 
    [CreatedBy] NVARCHAR(128) NULL, 
    [ConCode] NVARCHAR(128) NULL, 
)
