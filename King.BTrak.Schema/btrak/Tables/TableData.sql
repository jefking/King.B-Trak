CREATE TABLE [dbo].[TableData]
(
	[Id] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(), 
    [TableName] NVARCHAR(255) NOT NULL, 
    [PartitionKey] NVARCHAR(255) NOT NULL, 
    [RowKey] NVARCHAR(255) NOT NULL, 
    [ETag] NCHAR(255) NOT NULL, 
    [Timestamp] DATETIME NOT NULL, 
    [SynchronizedOn] DATETIME NOT NULL DEFAULT GETUTCDATE(),
    [Data] XML NULL, 
    PRIMARY KEY ([TableName], [RowKey], [PartitionKey])
)
