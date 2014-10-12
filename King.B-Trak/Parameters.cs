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
        /// Process Configuration
        /// </summary>
        /// <returns>Configuration Values</returns>
        public virtual IConfigValues Process()
        {
            var first = this.arguments.ElementAt(0);
            var second = this.arguments.ElementAt(1);
            var direction = first.ToLowerInvariant().Contains("server") && first.ToLowerInvariant().Contains("database")
                ? Direction.SqlToTable : Direction.TableToSql;
            return new ConfigValues
            {
                SyncDirection = direction,
                SqlConenction = direction == Direction.SqlToTable ? first : second,
                StorageAccountConnection = direction == Direction.TableToSql ? first : second,
                StorageTableName = ConfigurationManager.AppSettings["StorageTable"],
                SqlTableName = ConfigurationManager.AppSettings["SqlTable"],
            };
        }
        #endregion
    }
}