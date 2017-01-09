namespace King.BTrak.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Table Data
    /// </summary>
    public class TableData : ITableData
    {
        #region Properties
        /// <summary>
        /// Row Data
        /// </summary>
        public virtual IEnumerable<IDictionary<string, object>> Rows
        {
            get;
            set;
        }

        /// <summary>
        /// Table Name
        /// </summary>
        public virtual string TableName
        {
            get;
            set;
        }
        #endregion
    }
}