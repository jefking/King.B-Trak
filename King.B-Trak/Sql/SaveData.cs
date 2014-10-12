namespace King.BTrak.Sql
{
    using King.Mapper.Generated.Sql;

    /// <summary>
    /// SQL Save Data to Table Sproc
    /// </summary>
    public class SaveData : dboSaveTableData
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