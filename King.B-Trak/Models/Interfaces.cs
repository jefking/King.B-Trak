namespace King.BTrak.Models
{

    #region IConfigValues
    /// <summary>
    /// Configuration Values
    /// </summary>
    public interface IConfigValues
    {
        #region Properties
        /// <summary>
        /// 
        /// </summary>
        string SqlConenction
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        string StorageAccountConnection
        {
            get;
        }

        /// <summary>
        /// Storage Table Name
        /// </summary>
        string StorageTableName
        {
            get;
        }

        /// <summary>
        /// Storage Table Name
        /// </summary>
        string SqlTableName
        {
            get;
        }

        /// <summary>
        /// Sync Direction
        /// </summary>
        Direction SyncDirection
        {
            get;
        }
        #endregion
    }
    #endregion
}