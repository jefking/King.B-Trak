namespace King.BTrak.Unit.Test.Models
{
    using King.BTrak.Models;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;

    [TestFixture]
    public class SqlDataTests
    {
        [Test]
        public void Constructor()
        {
            new SqlData();
        }

        [Test]
        public void TableName()
        {
            var expected = Guid.NewGuid().ToString();
            var c = new SqlData
            {
                TableName = expected,
            };

            Assert.AreEqual(expected, c.TableName);
        }

        [Test]
        public void Rows()
        {
            var expected = new List<IDictionary<string, object>>();
            var c = new SqlData
            {
                Rows = expected,
            };

            Assert.AreEqual(expected, c.Rows);
        }
    }
}