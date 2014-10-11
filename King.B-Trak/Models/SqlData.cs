namespace King.BTrak.Models
{
    using System.Collections.Generic;

    public class SqlData
    {
        public string TableName
        {
            get;
            set;
        }

        public IEnumerable<IDictionary<string, object>> Rows
        {
            get;
            set;
        }
    }
}