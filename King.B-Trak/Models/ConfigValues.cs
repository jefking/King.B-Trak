namespace King.BTrak.Models
{
    public class ConfigValues : IConfigValues
    {
        public virtual string SQLConenctionString
        {
            get;
            set;
        }
        public virtual string StorageAccountConnectionString
        {
            get;
            set;
        }
        public virtual string StorageTableName
        {
            get;
            set;
        }
    }
}