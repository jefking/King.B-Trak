namespace King.BTrak.Unit.Test
{
    using King.Azure.Data;
    using King.BTrak.Models;
    using King.BTrak.Sql;
    using King.Data.Sql.Reflection;
    using King.Data.Sql.Reflection.Models;
    using King.Mapper.Data;
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [TestFixture]
    public class SqlDataWriterTests
    {
        [Test]
        public void Constructor()
        {
            var reader = Substitute.For<ISchemaReader>();
            var executor = Substitute.For<IExecutor>();
            var tableName = Guid.NewGuid().ToString();
            new SqlDataWriter(reader, executor, tableName);
        }

        [Test]
        public void ConstructorReaderNull()
        {
            var executor = Substitute.For<IExecutor>();
            var tableName = Guid.NewGuid().ToString();
            
            Assert.That(() => new SqlDataWriter(null, executor, tableName), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void ConstructorExecutorNull()
        {
            var reader = Substitute.For<ISchemaReader>();
            var tableName = Guid.NewGuid().ToString();
            
            Assert.That(() => new SqlDataWriter(reader, null, tableName), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void ConstructorTableNameNull()
        {
            var reader = Substitute.For<ISchemaReader>();
            var executor = Substitute.For<IExecutor>();
            
            Assert.That(() => new SqlDataWriter(reader, executor, null), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void IsISqlDataWriter()
        {
            var reader = Substitute.For<ISchemaReader>();
            var executor = Substitute.For<IExecutor>();
            var tableName = Guid.NewGuid().ToString();
            Assert.IsNotNull(new SqlDataWriter(reader, executor, tableName) as ISqlDataWriter);
        }

        [Test]
        public async Task Create()
        {
            var type = SchemaTypes.Table;
            var name = Guid.NewGuid().ToString();
            var statement = Guid.NewGuid().ToString();

            var reader = Substitute.For<ISchemaReader>();
            reader.Load(type).Returns(Task.FromResult<IEnumerable<IDefinition>>(new List<IDefinition>()));
            var executor = Substitute.For<IExecutor>();
            executor.NonQuery(statement).Returns(Task.FromResult(-1));

            var writer = new SqlDataWriter(reader, executor, Guid.NewGuid().ToString());
            var result = await writer.Create(type, name, statement);

            Assert.IsTrue(result);

            reader.Received().Load(type);
            executor.Received().NonQuery(statement);
        }

        [Test]
        public async Task CreateDoesntExist()
        {
            var type = SchemaTypes.Table;
            var name = Guid.NewGuid().ToString();
            var statement = Guid.NewGuid().ToString();

            var definitions = new List<IDefinition>();
            var def = new Definition()
            {
                Name = name,
                Preface = SqlStatements.Schema,
            };
            definitions.Add(def);
            var reader = Substitute.For<ISchemaReader>();
            reader.Load(type).Returns(Task.FromResult<IEnumerable<IDefinition>>(definitions));
            var executor = Substitute.For<IExecutor>();
            executor.NonQuery(statement).Returns(Task.FromResult(-1));

            var writer = new SqlDataWriter(reader, executor, Guid.NewGuid().ToString());
            var result = await writer.Create(type, name, statement);

            Assert.IsTrue(result);

            reader.Received().Load(type);
            executor.Received(0).NonQuery(statement);
        }

        [Test]
        public async Task Initialize()
        {
            var tableName = Guid.NewGuid().ToString();
            var name = Guid.NewGuid().ToString();
            var tableStatement = string.Format(SqlStatements.CreateTable, SqlStatements.Schema, tableName);
            var sprocStatement = string.Format(SqlStatements.CreateStoredProcedure, SqlStatements.Schema, tableName);
            
            var reader = Substitute.For<ISchemaReader>();
            reader.Load(SchemaTypes.Table).Returns(Task.FromResult<IEnumerable<IDefinition>>(new List<IDefinition>()));
            reader.Load(SchemaTypes.StoredProcedure).Returns(Task.FromResult<IEnumerable<IDefinition>>(new List<IDefinition>()));
            var executor = Substitute.For<IExecutor>();
            executor.NonQuery(tableStatement).Returns(Task.FromResult(-1));
            executor.NonQuery(sprocStatement).Returns(Task.FromResult(-1));

            var writer = new SqlDataWriter(reader, executor, tableName);
            var result = await writer.Initialize();

            Assert.IsTrue(result);

            reader.Received().Load(SchemaTypes.Table);
            reader.Received().Load(SchemaTypes.StoredProcedure);
            executor.Received().NonQuery(tableStatement);
            executor.Received().NonQuery(sprocStatement);
        }

        [Test]
        public async Task Store()
        {
            var tableName = Guid.NewGuid().ToString();
            var tableStatement = string.Format(SqlStatements.CreateTable, SqlStatements.Schema, tableName);
            var sprocStatement = string.Format(SqlStatements.CreateStoredProcedure, SqlStatements.Schema, tableName);
            var row = new Dictionary<string, object>();
            row.Add(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
            row.Add(TableStorage.PartitionKey, Guid.NewGuid().ToString());
            row.Add(TableStorage.RowKey, Guid.NewGuid().ToString());
            row.Add(TableStorage.ETag, Guid.NewGuid().ToString());
            var rows = new List<IDictionary<string, object>>();
            rows.Add(row);
            var data = new TableData()
            {
                TableName = Guid.NewGuid().ToString(),
                Rows = rows,
            };
            var dataSets = new List<TableData>();
            dataSets.Add(data);
            var reader = Substitute.For<ISchemaReader>();
            reader.Load(SchemaTypes.Table).Returns(Task.FromResult<IEnumerable<IDefinition>>(new List<IDefinition>()));
            reader.Load(SchemaTypes.StoredProcedure).Returns(Task.FromResult<IEnumerable<IDefinition>>(new List<IDefinition>()));
            var executor = Substitute.For<IExecutor>();
            executor.NonQuery(tableStatement).Returns(Task.FromResult(-1));
            executor.NonQuery(sprocStatement).Returns(Task.FromResult(-1));
            executor.NonQuery(Arg.Any<SaveData>());

            var writer = new SqlDataWriter(reader, executor, tableName);
            await writer.Store(dataSets);

            reader.Received().Load(SchemaTypes.Table);
            reader.Received().Load(SchemaTypes.StoredProcedure);
            executor.Received().NonQuery(tableStatement);
            executor.Received().NonQuery(sprocStatement);
            executor.Received().NonQuery(Arg.Any<SaveData>());
        }

        [Test]
        public async Task StoreTableNotCreated()
        {
            var tableName = Guid.NewGuid().ToString();
            var tableStatement = string.Format(SqlStatements.CreateTable, SqlStatements.Schema, tableName);
            var row = new Dictionary<string, object>();
            row.Add(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
            row.Add(TableStorage.PartitionKey, Guid.NewGuid().ToString());
            row.Add(TableStorage.RowKey, Guid.NewGuid().ToString());
            row.Add(TableStorage.ETag, Guid.NewGuid().ToString());
            var rows = new List<IDictionary<string, object>>();
            rows.Add(row);
            var data = new TableData()
            {
                TableName = Guid.NewGuid().ToString(),
                Rows = rows,
            };
            var dataSets = new List<TableData>();
            dataSets.Add(data);
            var reader = Substitute.For<ISchemaReader>();
            reader.Load(SchemaTypes.Table).Returns(Task.FromResult<IEnumerable<IDefinition>>(new List<IDefinition>()));
            var executor = Substitute.For<IExecutor>();
            executor.NonQuery(tableStatement).Returns(Task.FromResult(0));
            executor.NonQuery(Arg.Any<SaveData>());

            var writer = new SqlDataWriter(reader, executor, tableName);
            await writer.Store(dataSets);

            reader.Received().Load(SchemaTypes.Table);
            executor.Received().NonQuery(tableStatement);
            executor.Received(0).NonQuery(Arg.Any<SaveData>());
        }
    }
}