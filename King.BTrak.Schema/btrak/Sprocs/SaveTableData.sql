CREATE PROCEDURE [dbo].[SaveTableData]
	@Id UNIQUEIDENTIFIER = NULL
    , @TableName NVARCHAR(255) = NULL 
    , @PartitionKey NVARCHAR(255) = NULL
    , @RowKey NVARCHAR(255) = NULL
    , @ETag NCHAR(255) = NULL 
    , @Timestamp DATETIME = NULL 
    , @SynchronizedOn DATETIME = NULL
    , @Data XML = NULL
AS
BEGIN

	RETURN 0;

END