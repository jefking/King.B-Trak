CREATE PROCEDURE [dbo].[SaveTableData]
	@Id UNIQUEIDENTIFIER = NULL
    , @TableName NVARCHAR(255) = NULL 
    , @PartitionKey NVARCHAR(255) = NULL
    , @RowKey NVARCHAR(255) = NULL
    , @ETag NCHAR(255) = NULL 
    , @Timestamp DATETIME = NULL
    , @Data XML = NULL
AS
BEGIN

	MERGE INTO [dbo].[TableData] [table]
	USING
	(
		SELECT @Id AS [Id]
		, @TableName AS [TableName]
		, @PartitionKey AS [PartitionKey]
		, @RowKey AS [RowKey]
		, @ETag AS [ETag]
		, @Timestamp AS [Timestamp]
		, @Data AS [Data]
		, GETUTCDATE() AS [SynchronizedOn]
	) AS [source]
	ON
	(
		[table].[TableName] = [source].[TableName]
		AND [table].[PartitionKey] = [source].[PartitionKey]
		AND [table].[RowKey] = [source].[RowKey]
	)
	WHEN MATCHED THEN
		UPDATE SET [table].[Id] = [source].[Id]
			, [table].[ETag] = [source].[ETag]
			, [table].[Timestamp] = [source].[Timestamp]
			, [table].[SynchronizedOn] = [source].[SynchronizedOn]
			, [table].[Data] = [source].[Data]
	WHEN NOT MATCHED THEN
		INSERT
		(
			[Id]
			, [TableName]
			, [PartitionKey]
			, [RowKey]
			, [ETag]
			, [Timestamp]
			, [SynchronizedOn]
			, [Data]
		)
		VALUES
		(
			[source].[Id]
			, [source].[TableName]
			, [source].[PartitionKey]
			, [source].[RowKey]
			, [source].[ETag]
			, [source].[Timestamp]
			, [source].[SynchronizedOn]
			, [source].[Data]
		);

END