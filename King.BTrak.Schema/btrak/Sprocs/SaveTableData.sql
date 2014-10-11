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
		SELECT @Id
		, @TableName
		, @PartitionKey
		, @RowKey
		, @ETag
		, @Timestamp
		, @Data
		, GETUTCDATE()
	) AS [Data]
	(
		[Id]
		, [TableName]
		, [PartitionKey]
		, [RowKey]
		, [ETag]
		, [Timestamp]
		, [Data]
		, [SynchronizedOn]
	)
	ON
	(
		[table].[TableName] = [data].[TableName]
		AND [table].[PartitionKey] = [data].[PartitionKey]
		AND [table].[RowKey] = [data].[RowKey]
	)
	WHEN MATCHED THEN
		UPDATE SET [table].[Id] = [data].[Id]
			, [table].[ETag] = [data].[ETag]
			, [table].[Timestamp] = [data].[Timestamp]
			, [table].[SynchronizedOn] = [data].[SynchronizedOn]
			, [table].[Data] = [data].[Data]
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
			[data].[Id]
			, [data].[TableName]
			, [data].[PartitionKey]
			, [data].[RowKey]
			, [data].[ETag]
			, [data].[Timestamp]
			, [data].[SynchronizedOn]
			, [data].[Data]
		);

END