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
        /// SQL Connection
        /// </summary>
        protected readonly SqlConnection database = null;

        /// <summary>
        /// Table Storage
        /// </summary>
        protected readonly ITableStorage table = null;

        /// <summary>
        /// Schema Reader
        /// </summary>
        protected readonly SchemaReader reader = null;

        /// <summary>
        /// Azure Table Storage Writer
        /// </summary>
        protected readonly TableStorageWriter writer = null;

        /// <summary>
        /// Sql Data Loader
        /// </summary>
        protected readonly SqlDataLoader loader = null;
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
            this.database = new SqlConnection(config.SqlConenction);
            this.reader = new SchemaReader(this.config.SqlConenction);
            this.writer = new TableStorageWriter(this.table);
            this.loader = new SqlDataLoader(this.database);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Innitialize Members
        /// </summary>
        /// <returns>Synchronizer</returns>
        public virtual ISynchronizer Initialize()
        {
            Trace.TraceInformation("Initializing...");

            Trace.TraceInformation("Opening connection to Database.");
            this.database.Open();

            Trace.TraceInformation("Creating Storage Table.");
            this.table.CreateIfNotExists().Wait();

            Trace.TraceInformation("Initialized.");

            return this;
        }

        /// <summary>
        /// Run Synchronization
        /// </summary>
        /// <returns>Synchronizer</returns>
        public virtual ISynchronizer Run()
        {
            var operation = "Table";//"SQL Server"
            Trace.TraceInformation("Loading {0} Schema.", operation);
            //var schemas = this.reader.Load(SchemaTypes.Table).Result;
            var tableStorageReader = new TableStorageReader(new AzureStorageResources(""));
            var schemas = tableStorageReader.Load();
            Trace.TraceInformation("Loaded {0} Schema.", operation);

            Trace.TraceInformation("Loading {0} Data.", operation);
            //var tables = this.loader.Retrieve(schemas);
            var data = tableStorageReader.Retrieve(schemas).Result;
            Trace.TraceInformation("Loaded {0} Data.", operation);

            Trace.TraceInformation("Storing {0} Data.", operation);
            //this.writer.Store(tables).Wait();
            Trace.TraceInformation("Stored {0} Data.", operation);

            return this;
        }
        #endregion
    }
}