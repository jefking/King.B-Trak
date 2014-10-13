namespace King.BTrak.Integration.Test
{
    using King.BTrak.Models;
    using NUnit.Framework;
    using System.Threading.Tasks;

    [TestFixture]
    public class SynchronizerTests
    {
        [Test]
        public async Task ConstructorTableToSql()
        {
            var c = new ConfigValues()
            {
                StorageAccountConnection = "UseDevelopmentStorage=true;",
                SqlConenction = "Server=server;Database=database;Trusted_Connection=True;",
                SqlTableName = "TableData",
                StorageTableName = "sqlserverdata",
                SyncDirection = Direction.TableToSql,
            };

            var s = new Synchronizer(c);
            await s.Run(c.SyncDirection);
        }

        [Test]
        public async Task ConstructorSqlToTable()
        {
            var c = new ConfigValues()
            {
                StorageAccountConnection = "UseDevelopmentStorage=true;",
                SqlConenction = "Server=server;Database=database;Trusted_Connection=True;",
                SqlTableName = "TableData",
                StorageTableName = "sqlserverdata",
                SyncDirection = Direction.SqlToTable,
            };

            var s = new Synchronizer(c);
            await s.Run(c.SyncDirection);
        }
    }
}