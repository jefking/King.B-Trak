namespace King.BTrak.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class ConfigValues : IConfigValues
    {
        #region Properties
        /// <summary>
        /// SQL Conenction String
        /// </summary>
        public virtual string SQLConenction
        {
            get;
            set;
        }

        /// <summary>
        /// Storage Account Connection String
        /// </summary>
        public virtual string StorageAccountConnection
        {
            get;
            set;
        }

        /// <summary>
        /// Storage Table Name
        /// </summary>
        public virtual string StorageTableName
        {
            get;
            set;
        }
        #endregion
    }
}