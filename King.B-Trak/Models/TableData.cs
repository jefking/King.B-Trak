namespace King.BTrak.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Table Data
    /// </summary>
    public class TableData
    {
        #region Properties
        public IEnumerable<IDictionary<string, object>> Data
        {
            get;
            set;
        }
        public string Name
        {
            get;
            set;
        }
        #endregion
    }
}