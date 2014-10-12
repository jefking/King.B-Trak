namespace King.BTrak.Unit.Test
{
    using King.Data.Sql.Reflection;
    using King.Mapper.Data;
    using NSubstitute;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class SqlDataReaderTests
    {
        [Test]
        public void Constructor()
        {
            var executor = Substitute.For<IExecutor>();
            var reader = Substitute.For<ISchemaReader>();
            var tableName = Guid.NewGuid().ToString();
            new SqlDataReader(executor, reader, tableName);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorReaderNull()
        {
            var executor = Substitute.For<IExecutor>();
            var tableName = Guid.NewGuid().ToString();
            new SqlDataReader(executor, null, tableName);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorExecutorNull()
        {
            var reader = Substitute.For<ISchemaReader>();
            var tableName = Guid.NewGuid().ToString();
            new SqlDataReader(null, reader, tableName);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorTableNameNull()
        {
            var reader = Substitute.For<ISchemaReader>();
            var executor = Substitute.For<IExecutor>();
            new SqlDataReader(executor, reader, null);
        }
    }
}