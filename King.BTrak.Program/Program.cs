namespace King.BTrak.Program
{
    using System;
    using System.Diagnostics;

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