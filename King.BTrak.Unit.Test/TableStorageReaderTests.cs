namespace King.BTrak.Unit.Test
{
    using King.Azure.Data;
    using NSubstitute;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class TableStorageReaderTests
    {
        [Test]
        public void Constructor()
        {
            var resources = Substitute.For<IAzureStorageResources>();
            var tableName = Guid.NewGuid().ToString();
            new TableStorageReader(resources, tableName);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorStorageResourcesNull()
        {
            var tableName = Guid.NewGuid().ToString();
            new TableStorageReader(null, tableName);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorTableNameNull()
        {
            var resources = Substitute.For<IAzureStorageResources>();
            new TableStorageReader(resources, null);
        }

        [Test]
        public void IsITableStorageReader()
        {
            var resources = Substitute.For<IAzureStorageResources>();
            var tableName = Guid.NewGuid().ToString();
            Assert.IsNotNull(new TableStorageReader(resources, tableName) as ITableStorageReader);
        }
    }
}