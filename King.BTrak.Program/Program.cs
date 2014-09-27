namespace King.BTrak.Program
{
    using King.Azure.Data;
    using System;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.Threading.Tasks;

    /// <summary>
    /// B-Trak synchronizer, from SQL Server to Azure Table Storage
    /// </summary>
    public class Program
    {
        #region Methods
        /// <summary>
        /// Program Main Entry
        /// </summary>
        /// <param name="args">Program Arguments</param>
        public static void Main(string[] args)
        {
            try
            {
                var parameters = new Parameters(args);
                var config = parameters.Process();

                Trace.TraceInformation("{0}{3}{1}{3}{2}{3}", config.SQLConenctionString, config.StorageAccountConnectionString, config.StorageTableName, Environment.NewLine);

                var database = new SqlConnection(config.SQLConenctionString);
                var table = new TableStorage(config.StorageTableName, config.StorageAccountConnectionString);

                Task.WaitAll(table.CreateIfNotExists(), database.OpenAsync());
            }
            catch (Exception ex)
            {
                Trace.Fail(ex.ToString());
            }

            Trace.TraceInformation("Completed.");
        }
        #endregion
    }
}