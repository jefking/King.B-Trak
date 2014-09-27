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
        protected readonly string[] arguments;
        #endregion

        #region Constructors
        /// <summary>
        /// Parameters
        /// </summary>
        /// <param name="arguments">Arguments</param>
        public Parameters(string[] arguments)
        {
            this.arguments = arguments;
        }
        #endregion

        #region Methods
        public virtual object Process()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}