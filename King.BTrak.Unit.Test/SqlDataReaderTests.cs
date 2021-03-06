﻿namespace King.BTrak.Unit.Test
{
    using King.BTrak.Sql;
    using King.Data.Sql.Reflection;
    using King.Data.Sql.Reflection.Models;
    using King.Mapper.Data;
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;

    [TestFixture]
    public class SqlDataReaderTests
    {
        [Test]
        public void Constructor()
        {
            var executor = Substitute.For<IExecutor>();
            var loader = Substitute.For<IDynamicLoader>();
            var reader = Substitute.For<ISchemaReader>();
            var tableName = Guid.NewGuid().ToString();
            new SqlDataReader(executor, reader, loader, tableName);
        }

        [Test]
        public void ConstructorReaderNull()
        {
            var executor = Substitute.For<IExecutor>();
            var loader = Substitute.For<IDynamicLoader>();
            var tableName = Guid.NewGuid().ToString();
            
            Assert.That(() => new SqlDataReader(executor, null, loader, tableName), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void ConstructorExecutorNull()
        {
            var reader = Substitute.For<ISchemaReader>();
            var loader = Substitute.For<IDynamicLoader>();
            var tableName = Guid.NewGuid().ToString();
            
            Assert.That(() => new SqlDataReader(null, reader, loader, tableName), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void ConstructorLoaderNull()
        {
            var executor = Substitute.For<IExecutor>();
            var reader = Substitute.For<ISchemaReader>();
            var tableName = Guid.NewGuid().ToString();
            
            Assert.That(() => new SqlDataReader(executor, reader, null, tableName), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void ConstructorTableNameNull()
        {
            var executor = Substitute.For<IExecutor>();
            var reader = Substitute.For<ISchemaReader>();
            var loader = Substitute.For<IDynamicLoader>();
            
            Assert.That(() => new SqlDataReader(executor, reader, loader, null), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void IsISqlDataLoader()
        {
            var executor = Substitute.For<IExecutor>();
            var reader = Substitute.For<ISchemaReader>();
            var loader = Substitute.For<IDynamicLoader>();
            var tableName = Guid.NewGuid().ToString();
            Assert.IsNotNull(new SqlDataReader(executor, reader, loader, tableName) as ISqlDataLoader);
        }

        [Test]
        public async Task Load()
        {
            var tableName = Guid.NewGuid().ToString();
            var random = new Random();
            var count = random.Next(1, 25);
            var definitions = new List<IDefinition>();
            for (var i = 0; i < count; i++)
            {
                var d = new Definition()
                {
                    Name = Guid.NewGuid().ToString(),
                    Preface = Guid.NewGuid().ToString(),
                };
                definitions.Add(d);
            }
            var def = new Definition()
            {
                Name = tableName,
                Preface = SqlStatements.Schema,
            };
            definitions.Add(def);

            var executor = Substitute.For<IExecutor>();
            var schemaReader = Substitute.For<ISchemaReader>();
            schemaReader.Load(SchemaTypes.Table).Returns(Task.FromResult<IEnumerable<IDefinition>>(definitions));

            var loader = Substitute.For<IDynamicLoader>();

            var reader = new SqlDataReader(executor, schemaReader, loader, tableName);
            var result = await reader.Load();

            Assert.IsNotNull(result);
            Assert.AreEqual(count, result.Count());

            schemaReader.Received().Load(SchemaTypes.Table);
        }

        [Test]
        public async Task Retrieve()
        {
            var tableName = Guid.NewGuid().ToString();
            var random = new Random();
            var definitions = new List<IDefinition>();
            var variables = new List<IVariable>();
            variables.Add(new Variable() { IsPrimaryKey = true, ParameterName = Guid.NewGuid().ToString() });
            var def = new Definition()
            {
                Name = Guid.NewGuid().ToString(),
                Preface = Guid.NewGuid().ToString(),
                Variables = variables,
            };
            definitions.Add(def);

            var ds = new DataSet();
            var sql = string.Format(SqlStatements.SelectDataFormat, def.Preface, def.Name);

            var executor = Substitute.For<IExecutor>();
            executor.Query(sql).Returns(Task.FromResult(ds));
            var schemaReader = Substitute.For<ISchemaReader>();
            var loader = Substitute.For<IDynamicLoader>();
            loader.Dictionaries(ds).Returns(new List<IDictionary<string, object>>());

            var reader = new SqlDataReader(executor, schemaReader, loader, tableName);
            var result = await reader.Retrieve(definitions);

            Assert.IsNotNull(result);
            Assert.AreEqual(definitions.Count(), result.Count());

            executor.Received().Query(sql);
            loader.Received().Dictionaries(ds);
        }
    }
}