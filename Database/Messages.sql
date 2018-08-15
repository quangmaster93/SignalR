CREATE TABLE [dbo].[Messages]
(
	[Id] NVARCHAR(128) NOT NULL PRIMARY KEY, 
    [Content] NVARCHAR(MAX) NULL, 
    [ConversationId] NVARCHAR(128) NULL, 
    [SentUserId] NVARCHAR(128) NULL, 
    [SendTime] DATETIME NULL, 
    CONSTRAINT [FK_Message_ToTable] FOREIGN KEY ([ConversationId]) REFERENCES [Conversations]([Id])
)
