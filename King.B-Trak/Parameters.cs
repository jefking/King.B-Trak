namespace King.BTrak
{
    using King.BTrak.Models;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;

    /// <summary>
    /// Command Line Parameters
    /// </summary>
    public class Parameters : IParameters
    {
        #region Members
        /// <summary>
        /// Arguments
        /// </summary>
        protected readonly IReadOnlyList<string> arguments;
        #endregion

        #region Constructors
        /// <summary>
        /// Parameters
        /// </summary>
        /// <param name="arguments">Arguments</param>
        public Parameters(IReadOnlyList<string> arguments)
        {
            this.arguments = arguments;
        }
        #endregion

        #region Methods
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual IConfigValues Process()
        {
            return new ConfigValues
            {
                SQLConenction = this.arguments.ElementAt(0),
                StorageAccountConnection = this.arguments.ElementAt(1),
                StorageTableName = ConfigurationManager.AppSettings["StorageTable"],
            };
        }
        #endregion
    }
}