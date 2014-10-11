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

    public interface ITableStorageReader
    {
        #region Methods
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerable<ITableStorage> Load();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tables"></param>
        /// <returns></returns>
        Task<IEnumerable<SqlData>> Retrieve(IEnumerable<ITableStorage> tables);
        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public interface ISqlDataWriter
    {
        #region Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        Task Store(IEnumerable<SqlData> datas);
        #endregion
    }
}