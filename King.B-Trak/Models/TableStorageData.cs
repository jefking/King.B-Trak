namespace King.BTrak.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Table Data
    /// </summary>
    public class TableSqlData : TableData, ITableSqlData
    {
        #region Properties
        /// <summary>
        /// Primary Key Columns
        /// </summary>
        public virtual IEnumerable<string> PrimaryKeyColumns
        {
            get;
            set;
        }
        #endregion
    }
}