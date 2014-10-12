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
        /// Schema Reader
        /// </summary>
        protected readonly ISchemaReader sqlSchemaReader = null;

        /// <summary>
        /// Azure Table Storage Writer
        /// </summary>
        protected readonly ITableStorageWriter tableStorageWriter = null;

        /// <summary>
        /// Sql Data Loader
        /// </summary>
        protected readonly ISqlDataLoader sqlDataLoader = null;

        /// <summary>
        /// Azure Storage Resources
        /// </summary>
        protected readonly IAzureStorageResources storageResources = null;

        /// <summary>
        /// Table Storage Reader
        /// </summary>
        protected readonly ITableStorageReader tableStorageReader = null;

        /// <summary>
        /// SQL Data Writer
        /// </summary>
        protected readonly ISqlDataWriter sqlDataWriter = null;
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

            this.sqlSchemaReader = new SchemaReader(config.SqlConenction);
            this.tableStorageWriter = new TableStorageWriter(table);
            this.sqlDataLoader = new SqlDataLoader(database);
            this.storageResources = new AzureStorageResources(config.StorageAccountConnection);
            this.tableStorageReader = new TableStorageReader(this.storageResources, config.StorageTableName);
            this.sqlDataWriter = new SqlDataWriter(config.SqlTableName, this.sqlSchemaReader, config.SqlConenction);
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
            var sqlSchema = await this.sqlSchemaReader.Load(SchemaTypes.Table);
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