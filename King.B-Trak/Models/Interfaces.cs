namespace King.BTrak.Models
{

    #region IConfigValues
    /// <summary>
    /// 
    /// </summary>
    public interface IConfigValues
    {
        #region Properties
        /// <summary>
        /// 
        /// </summary>
        string SQLConenction
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
        /// 
        /// </summary>
        string StorageTableName
        {
            get;
        }
        #endregion
    }
    #endregion
}