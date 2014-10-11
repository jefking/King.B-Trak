namespace King.BTrak
{
    using King.BTrak.Models;
    using King.Data.Sql.Reflection;
    using King.Mapper;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;

    public class SqlDataWriter
    {
        public const string table = @"CREATE TABLE [btrak].[{0}]
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

        public SqlDataWriter(IConfigValues config)
        {
            this.tableName = config.SqlTableName;
        }

        public async Task<bool> CreateTable()
        {
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

            return false;
        }

        public async Task Store(IEnumerable<IDictionary<string, object>> datas)
        {
            var created = await this.CreateTable();
            if (created)
            {
                foreach (var data in datas)
                {
                    var tblMap = data.Map<SqlTable>();
                }
            }
            else
            {
                Trace.TraceError("Table is not created, no data loaded.");
            }
        }
    }
}