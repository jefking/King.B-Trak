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
        protected readonly ISqlDataLoader sqlReader = null;

        /// <summary>
        /// SQL Data Writer
        /// </summary>
        protected readonly ISqlDataWriter sqlWriter = null;

        /// <summary>
        /// Azure Table Storage Writer
        /// </summary>
        protected readonly ITableStorageWriter tableWriter = null;

        /// <summary>
        /// Table Storage Reader
        /// </summary>
        protected readonly ITableStorageReader tableReader = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Default Constructor
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

            switch (config.SyncDirection)
            {
                case Direction.TableToSql:
                    this.tableReader = new TableStorageReader(new AzureStorageResources(config.StorageAccountConnection), config.StorageTableName);
                    this.sqlWriter = new SqlDataWriter(sqlSchemaReader, executor, config.SqlTableName);
                    break;
                case Direction.SqlToTable:
                    this.sqlReader = new SqlDataReader(executor, sqlSchemaReader, new DynamicLoader(), config.SqlTableName);
                    this.tableWriter = new TableStorageWriter(new TableStorage(config.StorageTableName, config.StorageAccountConnection));
                    break;
                default:
                    throw new ArgumentException("Invalid Direction.");
            }
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
                    var sqlSchema = await this.sqlReader.Load();

                    Trace.TraceInformation("Loading data from {0}.", from);
                    var sqlData = await this.sqlReader.Retrieve(sqlSchema);

                    Trace.TraceInformation("Storing data to {0}.", to);
                    await this.tableWriter.Store(sqlData);
                    break;
                case Direction.TableToSql:
                    from = "Table Storage";
                    to = "SQL Server";
                    Trace.TraceInformation("Loading schema from {0}.", from);
                    var tableSchema = this.tableReader.Load();

                    Trace.TraceInformation("Loading data {0}.", from);
                    var tableData = await this.tableReader.Retrieve(tableSchema);

                    Trace.TraceInformation("Storing data to {0}.", to);
                    await this.sqlWriter.Store(tableData);
                    break;
                default:
                    throw new InvalidOperationException("Unknown sync direction.");
            }
        }
        #endregion
    }
}