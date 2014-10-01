CREATE TABLE [dbo].[MultipleKeys]
(
	[Id] INT NOT NULL IDENTITY(1,1) , 
    [Unique] UNIQUEIDENTIFIER NOT NULL DEFAULT newid(), 
    [Comment] NVARCHAR(50) NOT NULL DEFAULT '', 
    PRIMARY KEY ([Unique], [Id])
)
