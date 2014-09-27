namespace King.BTrak
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

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
        public virtual object Process()
        {
            string sqlConnection = this.arguments.ElementAt(0);
            string storageAccount = this.arguments.ElementAt(1);

            return null;
        }
        #endregion
    }
}