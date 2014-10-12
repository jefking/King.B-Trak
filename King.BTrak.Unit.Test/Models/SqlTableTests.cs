namespace King.BTrak.Unit.Test.Models
{
    using King.BTrak.Models;
    using King.Mapper.Generated.Sql;
    using NUnit.Framework;

    [TestFixture]
    public class SqlTableTests
    {
        [Test]
        public void Constructor()
        {
            new SqlTable();
        }

        [Test]
        public void IsdboSaveTableData()
        {
            Assert.IsNotNull(new SqlTable() as dboSaveTableData);
        }

        [Test]
        public void FullyQualifiedName()
        {
            var st = new SqlTable();
            Assert.AreEqual("[dbo].[SaveTableData]", st.FullyQualifiedName());
        }
    }
}