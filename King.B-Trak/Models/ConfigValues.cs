namespace King.BTrak.Models
{
    /// <summary>
    /// Configuration Values
    /// </summary>
    public class ConfigValues : IConfigValues
    {
        #region Properties
        /// <summary>
        /// SQL Conenction String
        /// </summary>
        public virtual string SqlConnection
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

        /// <summary>
        /// SQL Table Name
        /// </summary>
        public string SqlTableName
        {
            get;
            set;
        }

        /// <summary>
        /// Sync Direction
        /// </summary>
        public Direction SyncDirection
        {
            get;
            set;
        }
        #endregion
    }
}