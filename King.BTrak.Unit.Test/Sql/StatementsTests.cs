namespace King.BTrak.Unit.Test.Sql
{
    using King.BTrak.Sql;
    using NUnit.Framework;

    [TestFixture]
    public class SqlStatementsTests
    {
        [Test]
        public void Schema()
        {
            Assert.AreEqual("dbo", SqlStatements.Schema);
        }

        [Test]
        public void CreateTable()
        {
            Assert.IsTrue(SqlStatements.CreateTable.Contains("CREATE TABLE"));
        }

        [Test]
        public void CreateStoredProcedure()
        {
            Assert.IsTrue(SqlStatements.CreateStoredProcedure.Contains("CREATE PROCEDURE"));
        }

        [Test]
        public void SelectDataFormat()
        {
            Assert.AreEqual("SELECT * FROM [{0}].[{1}] WITH(NOLOCK);", SqlStatements.SelectDataFormat);
        }

        [Test]
        public void StoredProcedureName()
        {
            Assert.AreEqual("SaveTableData", SqlStatements.StoredProcedureName);
        }
    }
}