namespace King.BTrak.Unit.Test
{
    using King.Azure.Data;
    using NSubstitute;
    using NUnit.Framework;
    using System;

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
    }
}