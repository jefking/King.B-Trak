namespace King.BTrak
{
    using King.BTrak.Models;
    using System.Threading.Tasks;

    #region IParameters
    /// <summary>
    /// Command Line Parameters
    /// </summary>
    public interface IParameters
    {
        #region Methods
        /// <summary>
        /// Process Configuration
        /// </summary>
        /// <returns>Configuration Values</returns>
        IConfigValues Process();
        #endregion
    }
    #endregion

    #region ISynchronizer
    /// <summary>
    /// Data Synchronizer
    /// </summary>
    public interface ISynchronizer
    {
        /// <summary>
        /// Initialize
        /// </summary>
        /// <returns>Self</returns>
        ISynchronizer Initialize();

        /// <summary>
        /// Run
        /// </summary>
        /// <returns>Self</returns>
        ISynchronizer Run();
    }
    #endregion

    #region ITableStorageWriter
    public interface ITableStorageWriter
    {

    }
    #endregion

    #region ISqlDataLoader
    public interface ISqlDataLoader
    {

    }
    #endregion
}