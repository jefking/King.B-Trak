namespace King.BTrak
{
    using King.Azure.Data;
    using King.BTrak.Models;
    using King.Data.Sql.Reflection;
    using King.Mapper.Data;
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

            var sqlSchemaReader = new SchemaReader(config.SqlConenction);
            var executor = new Executor(new SqlConnection(config.SqlConenction));

            this.tableStorageWriter = new TableStorageWriter(new TableStorage(config.StorageTableName, config.StorageAccountConnection));
            this.sqlDataLoader = new SqlDataReader(executor, sqlSchemaReader, config.SqlTableName);
            this.tableStorageReader = new TableStorageReader(new AzureStorageResources(config.StorageAccountConnection), config.StorageTableName);
            this.sqlDataWriter = new SqlDataWriter(config.SqlTableName, sqlSchemaReader, executor);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Run Synchronization
        /// </summary>
        /// <param name="direction">Direction</param>
        /// <returns>Task</returns>
        public virtual async Task Run(Direction direction)
        {
            var from = string.Empty;
            var to = string.Empty;
            switch (direction)
            {
                case Direction.SqlToTable:
                    from = "SQL Server";
                    to = "Table Storage";
                    Trace.TraceInformation("Loading schema from {0}.", from);
                    var sqlSchema = await this.sqlDataLoader.Load();

                    Trace.TraceInformation("Loading data from {0}.", from);
                    var sqlData = await this.sqlDataLoader.Retrieve(sqlSchema);

                    Trace.TraceInformation("Storing data to {0}.", to);
                    await this.tableStorageWriter.Store(sqlData);
                    break;
                case Direction.TableToSql:
                    from = "Table Storage";
                    to = "SQL Server";
                    Trace.TraceInformation("Loading schema from {0}.", from);
                    var tableSchema = this.tableStorageReader.Load();

                    Trace.TraceInformation("Loading data {0}.", from);
                    var tableData = await this.tableStorageReader.Retrieve(tableSchema);

                    Trace.TraceInformation("Storing data to {0}.", to);
                    await this.sqlDataWriter.Store(tableData);
                    break;
                default:
                    throw new InvalidOperationException("Unknown sync direction.");
            }
        }
        #endregion
    }
}