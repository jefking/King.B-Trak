namespace King.BTrak
{
    using King.Azure.Data;
    using King.Data.Sql.Reflection;
    using King.Mapper.Data;
    using System;
    using System.Linq;
    using System.Data;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using King.BTrak.Models;

    /// <summary>
    /// 
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
        protected SqlConnection database = null;

        /// <summary>
        /// Table Storage
        /// </summary>
        protected ITableStorage table = null;
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
            this.database = new SqlConnection(config.SQLConenction);
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

            this.database.Open();

            this.table = new TableStorage(config.StorageTableName, config.StorageAccountConnection);
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
            Trace.TraceInformation("Running...");
            //Injectable
            var loader = new Loader<object>();
            var reader = new SchemaReader(this.config.SQLConenction);

            Trace.TraceInformation("Loading Database Schema.");
            var schemas = reader.Load(SchemaTypes.Table).Result;
            Trace.TraceInformation("Loaded Database Schema.");

            Trace.TraceInformation("Loading SQL Server Data.");
            var tables = new List<TableData>();
            foreach (var schema in schemas.Values)
            {
                var sql = string.Format("SELECT * FROM [{0}].[{1}] WITH(NOLOCK)", schema.Preface, schema.Name);
                Trace.TraceInformation(sql);

                var cmd = this.database.CreateCommand();
                cmd.CommandText = sql;
                var adapter = new SqlDataAdapter(cmd);

                var ds = new DataSet();
                adapter.Fill(ds);
                var table = ds.Tables[0];

                var data = new TableData
                {
                    Data = loader.Dictionaries(table),
                    Name = string.Format("[{0}].[{1}]", schema.Preface, schema.Name),
                };
                tables.Add(data);
                Trace.TraceInformation("Rows Read: {0}", data.Data.Count());
            }
            this.database.Close();
            Trace.TraceInformation("Loaded SQL Server Data.");

            Trace.TraceInformation("Storing SQL Server Data.");
            foreach (var table in tables)
            {
                foreach (var entity in table.Data)
                {
                    entity.Add(TableStorage.PartitionKey, table.Name);
                    entity.Add(TableStorage.RowKey, Guid.NewGuid());
                    this.table.InsertOrReplace(entity).Wait();
                }
            }
            Trace.TraceInformation("Stored SQL Server Data.");

            Trace.TraceInformation("Ran.");

            return this;
        }
        #endregion
    }
}