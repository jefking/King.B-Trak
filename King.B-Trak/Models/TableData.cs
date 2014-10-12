namespace King.BTrak.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Table Data
    /// </summary>
    public class TableData
    {
        #region Properties
        /// <summary>
        /// Row Data
        /// </summary>
        public virtual IEnumerable<IDictionary<string, object>> Data
        {
            get;
            set;
        }

        /// <summary>
        /// Name
        /// </summary>
        public virtual string Name
        {
            get;
            set;
        }

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