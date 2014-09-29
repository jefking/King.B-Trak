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

            Trace.TraceInformation("Loading Database Schema.");
            var reader = new SchemaReader(this.config.SQLConenction); // Pass in for injection
            var schemas = reader.Load(SchemaTypes.Table).Result;
            Trace.TraceInformation("Loaded Database Schema.");

            Trace.TraceInformation("Loading SQL Server Data.");

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

                var loader = new Loader<object>();
                var data = loader.Dictionaries(table);

                Trace.TraceInformation("Rows Read: {0}", data.Count());
            }
            Trace.TraceInformation("Loaded SQL Server Data.");

            Trace.TraceInformation("Storing SQL Server Data.");

            //OH YEA, Ready to STORE DATA

            //Store data to Table storage.
            //How to store dictionary into Entity...
            Trace.TraceInformation("Stored SQL Server Data.");

            Trace.TraceInformation("Ran.");

            return this;
        }
        #endregion
    }
}