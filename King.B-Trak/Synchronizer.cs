namespace King.BTrak
{
    using King.Azure.Data;
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

            Trace.TraceInformation("Ran.");

            return this;
        }
        #endregion
    }
}