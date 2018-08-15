CREATE TABLE [dbo].[UserConversations]
(
	[UserId] NVARCHAR(128) NOT NULL , 
    [ConversationId] NVARCHAR(128) NOT NULL, 
    [JoinedTime] DATETIME NULL, 
    PRIMARY KEY ([UserId], [ConversationId]), 
    CONSTRAINT [FK_UserConversation_ToTable] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers]([Id]), 
    CONSTRAINT [FK_UserConversation_ToTable_1] FOREIGN KEY ([ConversationId]) REFERENCES [Conversations]([Id])
)
