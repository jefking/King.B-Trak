namespace King.BTrak
{
    using King.Azure.Data;
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
            this.database = new SqlConnection(config.SQLConenction);
            this.reader = new SchemaReader(this.config.SQLConenction);
            this.writer = new TableStorageWriter(this.table);
            this.loader = new SqlDataLoader(this.database);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Innitialize Members
        /// </summary>
        /// <returns></returns>
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
        /// <returns></returns>
        public virtual ISynchronizer Run()
        {
            Trace.TraceInformation("Loading Database Schema.");
            var schemas = reader.Load(SchemaTypes.Table).Result;
            Trace.TraceInformation("Loaded Database Schema.");

            Trace.TraceInformation("Loading SQL Server Data.");
            var tables = loader.Retrieve(schemas);
            Trace.TraceInformation("Loaded SQL Server Data.");

            Trace.TraceInformation("Storing SQL Server Data.");
            writer.Store(tables).Wait();
            Trace.TraceInformation("Stored SQL Server Data.");

            return this;
        }
        #endregion
    }
}