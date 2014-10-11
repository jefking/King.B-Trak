namespace King.BTrak
{
    using King.Azure.Data;
    using King.BTrak.Models;
    using King.Data.Sql.Reflection;
    using King.Mapper;
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class SqlDataWriter
    {
        public const string table = @"CREATE TABLE [dbo].[{0}]
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
                                    );";

        private readonly string tableName = null;

        /// <summary>
        /// Schema Reader
        /// </summary>
        protected readonly SchemaReader reader = null;

        /// <summary>
        /// SQL Connection
        /// </summary>
        protected readonly SqlConnection database = null;

        public SqlDataWriter(string tableName, SchemaReader reader, string connectionString)
        {
            this.tableName = tableName;
            this.reader = reader;
            this.database = new SqlConnection(connectionString);
        }

        public async Task<bool> CreateTable()
        {
            await this.database.OpenAsync();
            var exists = (from t in await this.reader.Load(SchemaTypes.Table)
                           where t.Name == this.tableName
                           select true).FirstOrDefault();
            if (!exists)
            {
                Trace.TraceInformation("Creating table to load data into: '{0}'.", this.tableName);

                var statement = string.Format(table, this.tableName);
                var cmd = this.database.CreateCommand();
                cmd.CommandText = statement;
                return 1 == await cmd.ExecuteNonQueryAsync();
            }

            return true;
        }

        public async Task Store(IEnumerable<SqlData> datas)
        {
            var created = await this.CreateTable();
            if (created)
            {
                foreach (var data in datas)
                {
                    foreach (var row in data.Rows)
                    {
                        var tblMap = row.Map<SqlTable>();
                        tblMap.TableName = data.TableName;
                        tblMap.Id = Guid.NewGuid();
                        var keys = from k in row.Keys
                                   where k != TableStorage.ETag
                                       && k != TableStorage.PartitionKey
                                       && k != TableStorage.RowKey
                                       && k != TableStorage.Timestamp
                                   select k;
                        var values = new StringBuilder();
                        foreach (var k in keys)
                        {
                            values.AppendFormat("<{0}>{1}<{0}>", k, row[k]);
                        }
                        const string xmlWrapper = "<data>{0}</data>";
                        tblMap.Data = string.Format(xmlWrapper, values);
                    }
                }
            }
            else
            {
                Trace.TraceError("Table is not created, no data loaded.");
            }
        }
    }
}