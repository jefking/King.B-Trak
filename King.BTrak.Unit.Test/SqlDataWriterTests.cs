namespace King.BTrak.Unit.Test
{
    using King.Data.Sql.Reflection;
    using King.Mapper.Data;
    using NSubstitute;
    using NUnit.Framework;
    using System;

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
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorReaderNull()
        {
            var executor = Substitute.For<IExecutor>();
            var tableName = Guid.NewGuid().ToString();
            new SqlDataWriter(null, executor, tableName);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorExecutorNull()
        {
            var reader = Substitute.For<ISchemaReader>();
            var tableName = Guid.NewGuid().ToString();
            new SqlDataWriter(reader, null, tableName);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorTableNameNull()
        {
            var reader = Substitute.For<ISchemaReader>();
            var executor = Substitute.For<IExecutor>();
            new SqlDataWriter(reader, executor, null);
        }
    }
}