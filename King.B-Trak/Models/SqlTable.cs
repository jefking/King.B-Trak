namespace King.BTrak.Models
{
    using System;

    public class SqlTable
    {
        public Guid Id
        {
            get;
            set;
        }

        public string TableName
        {
            get;
            set;
        }

        public string PartitionKey
        {
            get;
            set;
        }

        public string RowKey
        {
            get;
            set;
        }

        public string ETag
        {
            get;
            set;
        }

        public DateTime Timestamp
        {
            get;
            set;
        }

        public string Data
        {
            get;
            set;
        }
    }
}