namespace King.BTrak.Program
{
    using System;
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

                var table = new King.Azure.Data.TableStorage(config.StorageTableName, config.StorageAccountConnectionString);
                Task.Run(() => table.CreateIfNotExists());
            }
            catch (Exception ex)
            {
                Trace.Fail(ex.Message, ex.StackTrace);
            }

            Trace.TraceInformation("Completed.");
        }
        #endregion
    }
}