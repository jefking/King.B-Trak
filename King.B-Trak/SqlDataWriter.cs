namespace King.BTrak
{
    using King.Azure.Data;
using King.BTrak.Models;
using King.Data.Sql.Reflection;
using King.Mapper;
using King.Mapper.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    public class SqlDataWriter
    {
        public const string Schema = "dbo";
        public const string table = @"CREATE TABLE [{0}].[{1}]
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
        public const string sproc = @"CREATE PROCEDURE [{0}].[SaveTableData]
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

        protected readonly string tableName = null;

        /// <summary>
        /// Schema Reader
        /// </summary>
        protected readonly SchemaReader reader = null;

        /// <summary>
        /// SQL Connection
        /// </summary>
        protected readonly SqlConnection database = null;

        protected readonly IExecutor executor = null;

        public SqlDataWriter(string tableName, SchemaReader reader, string connectionString)
        {
            this.tableName = tableName;
            this.reader = reader;
            this.database = new SqlConnection(connectionString);
            this.executor = new Executor(this.database);
            this.database.Open();
        }

        public virtual async Task<bool> CreateTable()
        {
            var exists = (from t in await this.reader.Load(SchemaTypes.Table)
                          where t.Name == this.tableName
                          && t.Preface == Schema
                          select true).FirstOrDefault();
            if (!exists)
            {
                Trace.TraceInformation("Creating table to load data into: '{0}'.", this.tableName);

                var statement = string.Format(table, Schema, this.tableName);
                var cmd = this.database.CreateCommand();
                cmd.CommandText = statement;
                return await cmd.ExecuteNonQueryAsync() == -1;
            }

            return true;
        }

        public virtual async Task<bool> CreateSproc()
        {
            var exists = (from t in await this.reader.Load(SchemaTypes.StoredProcedure)
                          where t.Name == "SaveTableData"
                           && t.Preface == Schema
                          select true).FirstOrDefault();
            if (!exists)
            {
                Trace.TraceInformation("Creating stored procedure to load data into table: '{0}'.", this.tableName);

                var statement = string.Format(sproc, Schema, this.tableName);
                var cmd = this.database.CreateCommand();
                cmd.CommandText = statement;
                return await cmd.ExecuteNonQueryAsync() == -1;
            }

            return true;
        }

        public virtual async Task Store(IEnumerable<SqlData> datas)
        {
            var created = await this.CreateTable();
            if (created)
            {
                created = await this.CreateSproc();
                if (created)
                {
                    foreach (var data in datas)
                    {
                        foreach (var row in data.Rows)
                        {
                            var sproc = row.Map<SqlTable>();
                            sproc.TableName = data.TableName;
                            sproc.Id = Guid.NewGuid();
                            var keys = from k in row.Keys
                                       where k != TableStorage.ETag
                                           && k != TableStorage.PartitionKey
                                           && k != TableStorage.RowKey
                                           && k != TableStorage.Timestamp
                                       select k;
                            var values = new StringBuilder();
                            foreach (var k in keys)
                            {
                                values.AppendFormat("<{0}>{1}</{0}>", k, row[k]);
                            }

                            sproc.Data = string.Format("<data>{0}</data>", values);

                            await this.executor.NonQuery(sproc);
                        }
                    }
                }
                else
                {
                    Trace.TraceError("Stored Procedure is not created, no data can be loaded.");
                }
            }
            else
            {
                Trace.TraceError("Table is not created, no data can be loaded.");
            }
        }
    }
}