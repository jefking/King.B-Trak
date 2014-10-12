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
        /// Run
        /// </summary>
        /// <returns>Self</returns>
        ISynchronizer Run();
        #endregion
    }
    #endregion

    #region ITableStorageWriter
    /// <summary>
    /// 
    /// </summary>
    public interface ITableStorageWriter
    {
        #region Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tables"></param>
        /// <returns></returns>
        Task Store(IEnumerable<TableData> tables);
        #endregion
    }
    #endregion

    #region ISqlDataLoader
    /// <summary>
    /// 
    /// </summary>
    public interface ISqlDataLoader
    {
        #region Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="schemas"></param>
        /// <returns></returns>
        IEnumerable<TableData> Retrieve(IEnumerable<IDefinition> schemas);
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
        IEnumerable<ITableStorage> Load();

        /// <summary>
        /// Retrieve Data
        /// </summary>
        /// <param name="tables">Tables</param>
        /// <returns>SQL Data</returns>
        Task<IEnumerable<SqlData>> Retrieve(IEnumerable<ITableStorage> tables);
        #endregion
    }
    #endregion

    #region ISqlDataWriter
    /// <summary>
    /// SQL Data Writer Interface
    /// </summary>
    public interface ISqlDataWriter
    {
        #region Methods
        /// <summary>
        /// Stores Data
        /// </summary>
        /// <param name="dataSet">Data Sets</param>
        /// <returns>Task</returns>
        Task Store(IEnumerable<SqlData> datas);
        #endregion
    }
    #endregion
}