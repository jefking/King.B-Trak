namespace King.BTrak.Unit.Test
{
    using King.Azure.Data;
    using Microsoft.WindowsAzure.Storage.Table;
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

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

        [Test]
        public void Load()
        {
            var tableName = Guid.NewGuid().ToString();

            var random = new Random();
            var count = random.Next(25);
            var tables = new List<ITableStorage>();

            for (var i = 0; i < count; i++)
            {
                var tbl = Substitute.For<ITableStorage>();
                tbl.Name.Returns(Guid.NewGuid().ToString());
                tables.Add(tbl);
            }

            var syncTable = Substitute.For<ITableStorage>();
            syncTable.Name.Returns(tableName);
            tables.Add(syncTable);

            var resources = Substitute.For<IAzureStorageResources>();
            resources.Tables().Returns(tables);

            var reader = new TableStorageReader(resources, tableName);
            var results = reader.Load();

            Assert.IsNotNull(results);
            Assert.AreEqual(count, results.Count());

            foreach (var tbl in tables)
            {
                var x = tbl.Received().Name;
            }

            resources.Received().Tables();
        }

        [Test]
        public async Task Retrieve()
        {
            var tableName = Guid.NewGuid().ToString();

            var random = new Random();
            var count = random.Next(25);
            var tables = new List<ITableStorage>();

            for (var i = 0; i < count; i++)
            {
                var tbl = Substitute.For<ITableStorage>();
                tbl.Name.Returns(Guid.NewGuid().ToString());
                tbl.Query(Arg.Any<TableQuery>()).Returns(Task.FromResult<IEnumerable<IDictionary<string, object>>>(null));

                tables.Add(tbl);
            }

            var resources = Substitute.For<IAzureStorageResources>();

            var reader = new TableStorageReader(resources, tableName);
            var results = await reader.Retrieve(tables);

            Assert.IsNotNull(results);
            Assert.AreEqual(count, results.Count());

            foreach (var tbl in tables)
            {
                var x = tbl.Received().Name;
                tbl.Received().Query(Arg.Any<TableQuery>());
            }
        }
    }
}