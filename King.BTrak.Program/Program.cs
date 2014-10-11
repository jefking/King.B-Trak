namespace King.BTrak.Program
{
    using System;
    using System.Diagnostics;
    using System.Threading;

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
            Trace.TraceInformation("Starting...");

            try
            {
                var parameters = new Parameters(args);
                var config = parameters.Process();

                Trace.TraceInformation("SQL Server Connection String: '{0}'{3}{3}Storage Account: '{1}'{3}Table Name: '{2}'{3}"
                    , config.SqlConenction
                    , config.StorageAccountConnection
                    , config.StorageTableName
                    , Environment.NewLine);

                new Synchronizer(config)
                    .Initialize()
                    .Run();
            }
            catch (Exception ex)
            {
                Trace.Fail(ex.ToString());
            }

            Trace.TraceInformation("Completed.");

            Thread.Sleep(2000);
        }
        #endregion
    }
}