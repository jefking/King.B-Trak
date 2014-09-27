namespace King.BTrak
{
    #region IParameters
    /// <summary>
    /// 
    /// </summary>
    public interface IParameters
    {
        #region Methods
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IConfigValues Process();
        #endregion
    }
    #endregion

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

    #region ISynchronizer
    /// <summary>
    /// 
    /// </summary>
    public interface ISynchronizer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ISynchronizer Initialize();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ISynchronizer Run();
    }
    #endregion
}