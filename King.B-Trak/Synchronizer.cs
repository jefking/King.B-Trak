namespace King.BTrak
{
    using King.Azure.Data;
    using King.BTrak.Models;
    using King.Data.Sql.Reflection;
    using System;
    using System.Data.SqlClient;
    using System.Diagnostics;

    /// <summary>
    /// Data Synchronizer
    /// </summary>
    public class Synchronizer : ISynchronizer
    {
        #region Members
        /// <summary>
        /// Configuration Values
        /// </summary>
        protected readonly IConfigValues config = null;

        /// <summary>
        /// Table Storage
        /// </summary>
        protected readonly ITableStorage table = null;

        /// <summary>
        /// Schema Reader
        /// </summary>
        protected readonly ISchemaReader reader = null;

        /// <summary>
        /// Azure Table Storage Writer
        /// </summary>
        protected readonly ITableStorageWriter writer = null;

        /// <summary>
        /// Sql Data Loader
        /// </summary>
        protected readonly ISqlDataLoader loader = null;

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

            this.config = config;
            this.table = new TableStorage(config.StorageTableName, config.StorageAccountConnection);
            var database = new SqlConnection(config.SqlConenction);
            this.reader = new SchemaReader(this.config.SqlConenction);
            this.writer = new TableStorageWriter(this.table);
            this.loader = new SqlDataLoader(database);
            this.storageResources = new AzureStorageResources(config.StorageAccountConnection);
            this.tableStorageReader = new TableStorageReader(this.storageResources, config.StorageTableName);
            this.sqlDataWriter = new SqlDataWriter(config.SqlTableName, this.reader, config.SqlConenction);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Run Synchronization
        /// </summary>
        /// <returns>Synchronizer</returns>
        public virtual ISynchronizer Run()
        {
            var operation = "Table";//"SQL Server"
            Trace.TraceInformation("Loading {0} Schema.", operation);
            //var schemas = this.reader.Load(SchemaTypes.Table).Result;
            var schemas = this.tableStorageReader.Load();
            Trace.TraceInformation("Loaded {0} Schema.", operation);

            Trace.TraceInformation("Loading {0} Data.", operation);
            //var tables = this.loader.Retrieve(schemas);
            var data = this.tableStorageReader.Retrieve(schemas).Result;
            Trace.TraceInformation("Loaded {0} Data.", operation);

            Trace.TraceInformation("Storing {0} Data.", operation);
            //this.writer.Store(tables).Wait();
            this.sqlDataWriter.Store(data).Wait();
            Trace.TraceInformation("Stored {0} Data.", operation);

            return this;
        }
        #endregion
    }
}