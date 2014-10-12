namespace King.BTrak.Unit.Test
{
    using King.BTrak.Models;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class SynchronizerTests
    {
        [Test]
        public void Constructor()
        {
            var c = new ConfigValues()
            {
                StorageAccountConnection = "UseDevelopmentStorage=true;",
                SqlConenction = "Server=server;Database=database;Trusted_Connection=True;",
                SqlTableName = "tabletable",
                StorageTableName = "tabletable",
                SyncDirection = Direction.TableToSql,
            };

            new Synchronizer(c);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorConfigNull()
        {
            new Synchronizer(null);
        }
    }
}