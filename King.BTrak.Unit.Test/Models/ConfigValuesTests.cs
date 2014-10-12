namespace King.BTrak.Unit.Test.Models
{
    using King.BTrak.Models;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class ConfigValuesTests
    {
        [Test]
        public void Constructor()
        {
            new ConfigValues();
        }

        [Test]
        public void IsIConfigValues()
        {
            Assert.IsNotNull(new ConfigValues() as IConfigValues);
        }

        [Test]
        public void SqlConnection()
        {
            var expected = Guid.NewGuid().ToString();
            var c = new ConfigValues
            {
                SqlConenction = expected,
            };

            Assert.AreEqual(expected, c.SqlConenction);
        }

        [Test]
        public void StorageAccountConnection()
        {
            var expected = Guid.NewGuid().ToString();
            var c = new ConfigValues
            {
                StorageAccountConnection = expected,
            };

            Assert.AreEqual(expected, c.StorageAccountConnection);
        }

        [Test]
        public void StorageTableName()
        {
            var expected = Guid.NewGuid().ToString();
            var c = new ConfigValues
            {
                StorageTableName = expected,
            };

            Assert.AreEqual(expected, c.StorageTableName);
        }

        [Test]
        public void SqlTableName()
        {
            var expected = Guid.NewGuid().ToString();
            var c = new ConfigValues
            {
                SqlTableName = expected,
            };

            Assert.AreEqual(expected, c.SqlTableName);
        }

        [Test]
        public void SyncDirection()
        {
            var expected = Direction.SqlToTable;
            var c = new ConfigValues
            {
                SyncDirection = expected,
            };

            Assert.AreEqual(expected, c.SyncDirection);
        }
    }
}