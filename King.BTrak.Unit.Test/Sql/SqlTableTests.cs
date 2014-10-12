namespace King.BTrak.Unit.Test.Sql
{
    using King.BTrak.Sql;
    using King.Mapper.Generated.Sql;
    using NUnit.Framework;

    [TestFixture]
    public class SqlTableTests
    {
        [Test]
        public void Constructor()
        {
            new SaveData();
        }

        [Test]
        public void IsdboSaveTableData()
        {
            Assert.IsNotNull(new SaveData() as dboSaveTableData);
        }

        [Test]
        public void FullyQualifiedName()
        {
            var st = new SaveData();
            Assert.AreEqual("[dbo].[SaveTableData]", st.FullyQualifiedName());
        }
    }
}