namespace King.BTrak.Sql
{
    /// <summary>
    /// SQL Statements
    /// </summary>
    public struct SqlStatements
    {
        #region Members
        /// <summary>
        /// Select Data Format String
        /// </summary>
        public const string SelectDataFormat = "SELECT * FROM [{0}].[{1}] WITH(NOLOCK);";

        /// <summary>
        /// Schema
        /// </summary>
        public const string Schema = "dbo";

        /// <summary>
        /// Create Table Statement
        /// </summary>
        public const string CreateTable = @"CREATE TABLE [{0}].[{1}]
                                    (
	                                    [Id] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(), 
                                        [TableName] NVARCHAR(255) NOT NULL, 
                                        [PartitionKey] NVARCHAR(255) NOT NULL, 
                                        [RowKey] NVARCHAR(255) NOT NULL, 
                                        [ETag] NVARCHAR(255) NOT NULL, 
                                        [Timestamp] DATETIME NOT NULL, 
                                        [SynchronizedOn] DATETIME NOT NULL DEFAULT GETUTCDATE(),
                                        [Data] XML NULL, 
                                        PRIMARY KEY ([TableName], [RowKey], [PartitionKey])
                                    );";

        /// <summary>
        /// Create Stored Procedure Statement
        /// </summary>
        public const string CreateStoredProcedure = @"CREATE PROCEDURE [{0}].[SaveTableData]
	                                    @Id UNIQUEIDENTIFIER = NULL
                                        , @TableName NVARCHAR(255) = NULL 
                                        , @PartitionKey NVARCHAR(255) = NULL
                                        , @RowKey NVARCHAR(255) = NULL
                                        , @ETag NVARCHAR(255) = NULL 
                                        , @Timestamp DATETIME = NULL
                                        , @Data XML = NULL
                                    AS
                                    BEGIN

	                                    MERGE INTO [{0}].[{1}] [table]
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

                                    END";
        #endregion
    }
}