namespace King.BTrak
{
    #region IParameters
    public interface IParameters
    {
        #region Methods
        IConfigValues Process();
        #endregion
    }
    #endregion

    #region IConfigValues
    public interface IConfigValues
    {
        #region Properties
        string SQLConenctionString
        {
            get;
        }

        string StorageAccountConnectionString
        {
            get;
        }
        string StorageTableName
        {
            get;
        }
        #endregion
    }
    #endregion
}