namespace King.BTrak.Models
{
    using King.Mapper.Generated.Sql;

    /// <summary>
    /// SQL Table Sproc
    /// </summary>
    public class SqlTable : dboSaveTableData
    {
        #region Methods
        /// <summary>
        /// Gets Stored Proc name with Schema
        /// </summary>
        public override string FullyQualifiedName()
        {
            return string.Format("[{0}].[SaveTableData]", SqlStatements.Schema);
        }
        #endregion
    }
}