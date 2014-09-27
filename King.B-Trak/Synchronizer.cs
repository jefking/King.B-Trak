namespace King.BTrak
{
    using King.Azure.Data;
    using System;
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
        /// 
        /// </summary>
        protected readonly IConfigValues config;
        #endregion

        #region Constructors
        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        public Synchronizer(IConfigValues config)
        {
            if (null == config)
            {
                throw new ArgumentNullException("config");
            }

            this.config = config;
        }
        #endregion

        #region Methods
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ISynchronizer Initialize()
        {
            Trace.TraceInformation("Initializing...");

            var database = new SqlConnection(config.SQLConenction);
            var table = new TableStorage(config.StorageTableName, config.StorageAccountConnection);

            Task.WaitAll(database.OpenAsync(), table.CreateIfNotExists());

            Trace.TraceInformation("Initialized.");

            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ISynchronizer Run()
        {
            Trace.TraceInformation("Running...");

            Trace.TraceInformation("Loading Database Schema.");
            Trace.TraceInformation("Loaded Database Schema.");

            Trace.TraceInformation("Loading SQL Server Data.");
            Trace.TraceInformation("Loaded SQL Server Data.");

            Trace.TraceInformation("Storing SQL Server Data.");
            Trace.TraceInformation("Stored SQL Server Data.");

            Trace.TraceInformation("Ran.");

            return this;
        }
        #endregion
    }
}