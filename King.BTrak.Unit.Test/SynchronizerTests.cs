namespace King.BTrak.Unit.Test
{
    using King.BTrak.Models;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class SynchronizerTests
    {
        [Test]
        public void ConstructorTableToSql()
        {
            var c = new ConfigValues()
            {
                StorageAccountConnection = "UseDevelopmentStorage=true;",
                SqlConnection = "Server=server;Database=database;Trusted_Connection=True;",
                SqlTableName = "tabletable",
                StorageTableName = "tabletable",
                Direction = Direction.TableToSql,
            };

            new Synchronizer(c);
        }

        [Test]
        public void ConstructorSqlToTable()
        {
            var c = new ConfigValues()
            {
                StorageAccountConnection = "UseDevelopmentStorage=true;",
                SqlConnection = "Server=server;Database=database;Trusted_Connection=True;",
                SqlTableName = "tabletable",
                StorageTableName = "tabletable",
                Direction = Direction.SqlToTable,
            };

            new Synchronizer(c);
        }

        [Test]
        public void ConstructorConfigNull()
        {
            Assert.That(() => new Synchronizer(null), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void IsISynchronizer()
        {
            var c = new ConfigValues()
            {
                StorageAccountConnection = "UseDevelopmentStorage=true;",
                SqlConnection = "Server=server;Database=database;Trusted_Connection=True;",
                SqlTableName = "tabletable",
                StorageTableName = "tabletable",
                Direction = Direction.SqlToTable,
            };

            Assert.IsNotNull(new Synchronizer(c) as ISynchronizer);
        }
    }
}