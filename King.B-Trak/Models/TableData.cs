namespace King.BTrak.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Table Data
    /// </summary>
    public class TableData
    {
        #region Properties
        public virtual IEnumerable<IDictionary<string, object>> Data
        {
            get;
            set;
        }
        public virtual string Name
        {
            get;
            set;
        }
        #endregion
    }
}