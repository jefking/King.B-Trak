namespace King.BTrak.Unit.Test
{
    using King.Azure.Data;
    using King.BTrak.Models;
    using Microsoft.WindowsAzure.Storage.Table;
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [TestFixture]
    public class TableStorageWriterTests
    {
        [Test]
        public void Constructor()
        {
            var table = Substitute.For<ITableStorage>();
            new TableStorageWriter(table);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorTableNull()
        {
            new TableStorageWriter(null);
        }

        [Test]
        public void IsITableStorageWriter()
        {
            var table = Substitute.For<ITableStorage>();
            Assert.IsNotNull(new TableStorageWriter(table) as ITableStorageWriter);
        }

        [Test]
        public async Task Initialize()
        {
            var table = Substitute.For<ITableStorage>();
            table.CreateIfNotExists().Returns(Task.FromResult(true));

            var writer = new TableStorageWriter(table);
            var result = await writer.Initialize();

            Assert.IsTrue(result);
            table.Received().CreateIfNotExists();
        }

        [Test]
        public async Task StoreNoPK()
        {
            var rows = new List<IDictionary<string, object>>();
            var row = new Dictionary<string, object>();
            rows.Add(row);
            var data = new TableSqlData()
            {
                PrimaryKeyColumns = null,
                Rows = rows,
                TableName = Guid.NewGuid().ToString(),
            };

            var tables = new List<TableSqlData>();
            tables.Add(data);

            var table = Substitute.For<ITableStorage>();
            table.InsertOrReplace(row);

            var writer = new TableStorageWriter(table);
            await writer.Store(tables);

            Assert.AreEqual(data.TableName, row[TableStorage.PartitionKey]);
            Assert.IsNotNull(row[TableStorage.RowKey]);
            Assert.AreNotEqual(Guid.Empty, Guid.Parse((string)row[TableStorage.RowKey]));

            table.Received().InsertOrReplace(row);
        }

        [Test]
        public async Task Store()
        {
            var pks = new List<string>();
            var pkName = Guid.NewGuid().ToString();
            pks.Add(pkName);
            var pk = Guid.NewGuid().ToString();

            var rows = new List<IDictionary<string, object>>();
            var row = new Dictionary<string, object>();
            row.Add(pkName, pk);
            rows.Add(row);
            var data = new TableSqlData()
            {
                PrimaryKeyColumns = pks,
                Rows = rows,
                TableName = Guid.NewGuid().ToString(),
            };

            var tables = new List<TableSqlData>();
            tables.Add(data);

            var table = Substitute.For<ITableStorage>();
            table.InsertOrReplace(row);

            var writer = new TableStorageWriter(table);
            await writer.Store(tables);

            Assert.AreEqual(data.TableName, row[TableStorage.PartitionKey]);
            Assert.AreEqual(pk, row[TableStorage.RowKey]);

            table.Received().InsertOrReplace(row);
        }

        [Test]
        public async Task StoreMultiplePks()
        {
            var pks = new List<string>();
            pks.Add(Guid.NewGuid().ToString());
            pks.Add(Guid.NewGuid().ToString());

            var rows = new List<IDictionary<string, object>>();
            var row = new Dictionary<string, object>();
            row.Add(pks.ElementAt(0), Guid.NewGuid());
            row.Add(pks.ElementAt(1), Guid.NewGuid());
            rows.Add(row);
            var data = new TableSqlData()
            {
                PrimaryKeyColumns = pks,
                Rows = rows,
                TableName = Guid.NewGuid().ToString(),
            };

            var tables = new List<TableSqlData>();
            tables.Add(data);

            var table = Substitute.For<ITableStorage>();
            table.InsertOrReplace(row);

            var writer = new TableStorageWriter(table);
            await writer.Store(tables);

            Assert.AreEqual(data.TableName, row[TableStorage.PartitionKey]);
            var pk = string.Format("{0}_{1}", row.Values.ElementAt(0), row.Values.ElementAt(1));
            var rowKey = row[TableStorage.RowKey];
            Assert.AreEqual(pk, rowKey);

            table.Received().InsertOrReplace(row);
        }

        [Test]
        public async Task StorePKNull()
        {
            var pks = new List<string>();
            var pkName = Guid.NewGuid().ToString();
            pks.Add(pkName);

            var rows = new List<IDictionary<string, object>>();
            var row = new Dictionary<string, object>();
            row.Add(pkName, null);
            rows.Add(row);
            var data = new TableSqlData()
            {
                PrimaryKeyColumns = pks,
                Rows = rows,
                TableName = Guid.NewGuid().ToString(),
            };

            var tables = new List<TableSqlData>();
            tables.Add(data);

            var table = Substitute.For<ITableStorage>();
            table.InsertOrReplace(row);

            var writer = new TableStorageWriter(table);
            await writer.Store(tables);

            Assert.AreEqual(data.TableName, row[TableStorage.PartitionKey]);
            Assert.AreEqual("(null)", (string)row[TableStorage.RowKey]);

            table.Received().InsertOrReplace(row);
        }
    }
}