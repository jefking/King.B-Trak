namespace King.BTrak
{
    using King.Azure.Data;
    using System.Data.SqlClient;
    using System.Threading.Tasks;

    public class Synchronizer : ISynchronizer
    {
        #region Members
        protected readonly IConfigValues config;
        #endregion

        #region Constructors
        public Synchronizer(IConfigValues config)
        {
            this.config = config;
        }
        #endregion

        #region Methods
        public ISynchronizer Initialize()
        {
            var database = new SqlConnection(config.SQLConenctionString);
            var table = new TableStorage(config.StorageTableName, config.StorageAccountConnectionString);

            Task.WaitAll(database.OpenAsync(), table.CreateIfNotExists());

            return this;
        }

        public ISynchronizer Run()
        {

            return this;
        }
        #endregion
    }
}