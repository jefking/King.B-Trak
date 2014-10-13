namespace King.BTrak.Integration.Test
{
    using King.Azure.Data;
    using King.BTrak.Models;
    using King.Mapper.Data;
    using Microsoft.WindowsAzure.Storage.Table;
    using NUnit.Framework;
    using System;
    using System.Data.SqlClient;
    using System.Threading.Tasks;

    [TestFixture]
    public class SynchronizerTests
    {
        private class DataRow
        {
            public bool Exists
            {
                get;
                set;
            }
        }

        [Test]
        public async Task ConstructorTableToSql()
        {
            var c = new ConfigValues()
            {
                StorageAccountConnection = "UseDevelopmentStorage=true;",
                SqlConenction = "Server=localhost;Database=King.BTrak.Data;Trusted_Connection=True;",
                SqlTableName = "TableData",
                StorageTableName = "sqlserverdata",
                SyncDirection = Direction.TableToSql,
            };

            var tableName = "testsynctabletosql";
            var table = new TableStorage(tableName, c.StorageAccountConnection);
            await table.CreateIfNotExists();
            var entity = new TableEntity
            {
                PartitionKey = Guid.NewGuid().ToString(),
                RowKey = Guid.NewGuid().ToString(),
            };
            await table.InsertOrReplace(entity);

            var s = new Synchronizer(c);
            await s.Run(c.SyncDirection);

            await table.Delete();

            var statement = string.Format("SELECT 1 AS [Exists] FROM [dbo].[TableData] WHERE TableName='{0}' AND PartitionKey = '{1}' AND RowKey = '{2}';", tableName, entity.PartitionKey, entity.RowKey);
            var executor = new Executor(new SqlConnection(c.SqlConenction));
            var ds = await executor.Query(statement);
            var r = ds.Model<DataRow>();
            Assert.IsNotNull(r);
            Assert.IsTrue(r.Exists);
        }

        [Test]
        public async Task ConstructorSqlToTable()
        {
            var c = new ConfigValues()
            {
                StorageAccountConnection = "UseDevelopmentStorage=true;",
                SqlConenction = "Server=localhost;Database=King.BTrak.Data;Trusted_Connection=True;",
                SqlTableName = "TableData",
                StorageTableName = "sqlserverdata",
                SyncDirection = Direction.SqlToTable,
            };

            var s = new Synchronizer(c);
            await s.Run(c.SyncDirection);
        }
    }
}