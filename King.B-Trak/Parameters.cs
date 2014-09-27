namespace King.BTrak
{
    using King.BTrak.Models;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;

    public class Parameters : IParameters
    {
        #region Members
        /// <summary>
        /// Arguments
        /// </summary>
        protected readonly IEnumerable<string> arguments;
        #endregion

        #region Constructors
        /// <summary>
        /// Parameters
        /// </summary>
        /// <param name="arguments">Arguments</param>
        public Parameters(IEnumerable<string> arguments)
        {
            this.arguments = arguments;
        }
        #endregion

        #region Methods
        public virtual IConfigValues Process()
        {
            var config = new ConfigValues
            {
                SQLConenctionString = this.arguments.ElementAt(0),
                StorageAccountConnectionString = this.arguments.ElementAt(1),
                StorageTableName = ConfigurationManager.AppSettings["StorageTable"],
            };

            return config;
        }
        #endregion
    }
}