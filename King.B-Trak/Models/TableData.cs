namespace King.BTrak.Models
{
    using System.Collections.Generic;

    public class TableData
    {
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
    }
}