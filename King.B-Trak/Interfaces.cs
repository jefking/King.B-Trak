namespace King.BTrak
{
    using King.BTrak.Models;
    using King.Data.Sql.Reflection.Models;
    using System.Collections.Generic;
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
        #region Methods
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
        #endregion
    }
    #endregion

    #region ITableStorageWriter
    public interface ITableStorageWriter
    {
        #region Methods
        Task Store(IList<TableData> tables);
        #endregion
    }
    #endregion

    #region ISqlDataLoader
    public interface ISqlDataLoader
    {
        #region Methods
        IList<TableData> Retrieve(IEnumerable<IDefinition> schemas);
        #endregion
    }
    #endregion
}