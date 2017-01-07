namespace King.BTrak
{
    using King.Azure.Data;
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
        /// Run Synchronization
        /// </summary>
        /// <returns>Task</returns>
        Task Run();
        #endregion
    }
    #endregion

    #region IInitializer
    /// <summary>
    /// Initializer
    /// </summary>
    public interface IInitializer
    {
        /// <summary>
        /// Initialize
        /// </summary>
        /// <returns>Success</returns>
        Task<bool> Initialize();
    }
    #endregion

    #region ITableStorageWriter
    /// <summary>
    /// Table Storage Writer Interface
    /// </summary>
    public interface ITableStorageWriter : IInitializer
    {
        #region Methods
        /// <summary>
        /// Store Data
        /// </summary>
        /// <param name="tables">Tables</param>
        /// <returns>Task</returns>
        Task Store(IEnumerable<TableSqlData> tables);
        #endregion
    }
    #endregion

    #region ISqlDataLoader
    /// <summary>
    /// SQL Data Loader
    /// </summary>
    public interface ISqlDataLoader
    {
        #region Methods
        /// <summary>
        /// Retrieve Table Data
        /// </summary>
        /// <param name="schemas">Schemas</param>
        /// <returns>Table Data</returns>
        Task<IEnumerable<TableSqlData>> Retrieve(IEnumerable<IDefinition> schemas);

        /// <summary>
        /// Load Definitions
        /// </summary>
        /// <returns>Schema</returns>
        Task<IEnumerable<IDefinition>> Load();
        #endregion
    }
    #endregion

    #region ITableStorageReader
    /// <summary>
    /// Table Storage Reader Interface
    /// </summary>
    public interface ITableStorageReader
    {
        #region Methods
        /// <summary>
        /// Load Tables
        /// </summary>
        /// <returns>Tables</returns>
        Task<IEnumerable<ITableStorage>> Load();

        /// <summary>
        /// Retrieve Data
        /// </summary>
        /// <param name="tables">Tables</param>
        /// <returns>SQL Data</returns>
        Task<IEnumerable<TableData>> Retrieve(IEnumerable<ITableStorage> tables);
        #endregion
    }
    #endregion

    #region ISqlDataWriter
    /// <summary>
    /// SQL Data Writer Interface
    /// </summary>
    public interface ISqlDataWriter : IInitializer
    {
        #region Methods
        /// <summary>
        /// Stores Data
        /// </summary>
        /// <param name="dataSet">Data Sets</param>
        /// <returns>Task</returns>
        Task Store(IEnumerable<TableData> datas);
        #endregion
    }
    #endregion
}