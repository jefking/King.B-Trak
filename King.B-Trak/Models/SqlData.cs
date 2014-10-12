namespace King.BTrak.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// SQL Data
    /// </summary>
    public class SqlData
    {
        #region Properties
        /// <summary>
        /// Table Name
        /// </summary>
        public string TableName
        {
            get;
            set;
        }

        /// <summary>
        /// Data Rows
        /// </summary>
        public IEnumerable<IDictionary<string, object>> Rows
        {
            get;
            set;
        }
        #endregion
    }
}