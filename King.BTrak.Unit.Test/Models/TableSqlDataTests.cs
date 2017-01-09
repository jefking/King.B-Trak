namespace King.BTrak.Unit.Test.Models
{
    using King.BTrak.Models;
    using NUnit.Framework;
    using System.Collections.Generic;

    [TestFixture]
    public class TableSqlDataTests
    {
        [Test]
        public void Constructor()
        {
            new TableSqlData();
        }

        [Test]
        public void IsITableSqlData()
        {
            Assert.IsNotNull(new TableSqlData() as ITableSqlData);
        }

        [Test]
        public void IsTableData()
        {
            Assert.IsNotNull(new TableSqlData() as TableData);
        }

        [Test]
        public void PrimaryKeyColumns()
        {
            var expected = new List<string>();
            var c = new TableSqlData
            {
                PrimaryKeyColumns = expected,
            };

            Assert.AreEqual(expected, c.PrimaryKeyColumns);
        }
    }
}