namespace King.BTrak.Unit.Test
{
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [TestFixture]
    public class ParametersTests
    {
        [Test]
        public void Constructor()
        {
            var args = new string[] { string.Empty, string.Empty };
            new Parameters(args);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorArgumentsNull()
        {
            new Parameters(null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorArgumentsEmpty()
        {
            new Parameters(new string[] { });
        }

        [Test]
        public void IsIParameters()
        {
            var args = new string[] { string.Empty, string.Empty };
            Assert.IsNotNull(new Parameters(args) as IParameters);
        }

        [Test]
        public void ProcessSqlToTable()
        {
            var args = new string[] { "Server=server;Database=database;Trusted_Connection=True;", "UseDevelopmentStorage=true;" };
            var p = new Parameters(args);
            var config = p.Process();

            Assert.AreEqual(Direction.SqlToTable, config.SyncDirection);
            Assert.AreEqual(args.ElementAt(1), config.StorageAccountConnection);
            Assert.AreEqual(args.ElementAt(0), config.SqlConenction);
            Assert.AreEqual("storagetable", config.StorageTableName);
            Assert.AreEqual("sqltable", config.SqlTableName);
        }

        [Test]
        public void ProcessTableToSql()
        {
            var args = new string[] { "UseDevelopmentStorage=true;", "Server=server;Database=database;Trusted_Connection=True;" };
            var p = new Parameters(args);
            var config = p.Process();

            Assert.AreEqual(Direction.TableToSql, config.SyncDirection);
            Assert.AreEqual(args.ElementAt(0), config.StorageAccountConnection);
            Assert.AreEqual(args.ElementAt(1), config.SqlConenction);
            Assert.AreEqual("storagetable", config.StorageTableName);
            Assert.AreEqual("sqltable", config.SqlTableName);
        }
    }
}