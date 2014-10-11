namespace King.BTrak
{
    using King.BTrak.Models;
    using King.Data.Sql.Reflection;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
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
                var tableStatement = string.Format(table, this.tableName);
                return true;
            }

            return false;
        }

        public void Store(IDictionary<string, object> data)
        {

        }
    }
}