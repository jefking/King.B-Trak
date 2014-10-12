namespace King.BTrak
{
    using King.Azure.Data;
    using King.BTrak.Models;
    using King.Data.Sql.Reflection;
    using System;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.Threading.Tasks;

    /// <summary>
    /// Data Synchronizer
    /// </summary>
    public class Synchronizer : ISynchronizer
    {
        #region Members
        /// <summary>
        /// Sql Data Loader
        /// </summary>
        protected readonly ISqlDataLoader sqlDataLoader = null;

        /// <summary>
        /// SQL Data Writer
        /// </summary>
        protected readonly ISqlDataWriter sqlDataWriter = null;

        /// <summary>
        /// Azure Table Storage Writer
        /// </summary>
        protected readonly ITableStorageWriter tableStorageWriter = null;

        /// <summary>
        /// Table Storage Reader
        /// </summary>
        protected readonly ITableStorageReader tableStorageReader = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="config">Configuration Values</param>
        public Synchronizer(IConfigValues config)
        {
            if (null == config)
            {
                throw new ArgumentNullException("config");
            }

            var table = new TableStorage(config.StorageTableName, config.StorageAccountConnection);
            var database = new SqlConnection(config.SqlConenction);
            var storageResources = new AzureStorageResources(config.StorageAccountConnection);
            var sqlSchemaReader = new SchemaReader(config.SqlConenction);

            this.tableStorageWriter = new TableStorageWriter(table);
            this.sqlDataLoader = new SqlDataLoader(database, sqlSchemaReader);
            this.tableStorageReader = new TableStorageReader(storageResources, config.StorageTableName);
            this.sqlDataWriter = new SqlDataWriter(config.SqlTableName, sqlSchemaReader, config.SqlConenction);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Run Synchronization
        /// </summary>
        /// <returns>Synchronizer</returns>
        public virtual async Task Run()
        {
            var operation = "Table";//"SQL Server"
            Trace.TraceInformation("Loading {0} Schema.", operation);
            var sqlSchema = await this.sqlDataLoader.Load();
            var tableSchema = this.tableStorageReader.Load();
            Trace.TraceInformation("Loaded {0} Schema.", operation);

            Trace.TraceInformation("Loading {0} Data.", operation);
            var sqlData = this.sqlDataLoader.Retrieve(sqlSchema);
            var tableData = await this.tableStorageReader.Retrieve(tableSchema);
            Trace.TraceInformation("Loaded {0} Data.", operation);

            Trace.TraceInformation("Storing {0} Data.", operation);
            this.tableStorageWriter.Store(sqlData).Wait();
            this.sqlDataWriter.Store(tableData).Wait();
            Trace.TraceInformation("Stored {0} Data.", operation);
        }
        #endregion
    }
}