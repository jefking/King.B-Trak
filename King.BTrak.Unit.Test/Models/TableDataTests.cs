namespace King.BTrak.Unit.Test.Models
{
    using King.BTrak.Models;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;

    [TestFixture]
    public class TableDataTests
    {
        [Test]
        public void Constructor()
        {
            new TableData();
        }

        [Test]
        public void Name()
        {
            var expected = Guid.NewGuid().ToString();
            var c = new TableData
            {
                TableName = expected,
            };

            Assert.AreEqual(expected, c.TableName);
        }

        [Test]
        public void Rows()
        {
            var expected = new List<IDictionary<string, object>>();
            var c = new TableData
            {
                Rows = expected,
            };

            Assert.AreEqual(expected, c.Rows);
        }
    }
}