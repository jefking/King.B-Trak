namespace King.BTrak.Models
{
    using King.Mapper.Generated.Sql;

    public class SqlTable : dboSaveTableData
    {
        /// <summary>
        /// Gets Stored Proc name with Schema
        /// </summary>
        public override string FullyQualifiedName()
        {
            return string.Format("[{0}].[SaveTableData]", SqlStatements.Schema);
        }
    }
}