namespace King.BTrak
{
    using King.BTrak.Models;
    using System.Threading.Tasks;

    #region IParameters
    /// <summary>
    /// 
    /// </summary>
    public interface IParameters
    {
        #region Methods
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IConfigValues Process();
        #endregion
    }
    #endregion

    #region ISynchronizer
    /// <summary>
    /// Data Synchronizer
    /// </summary>
    public interface ISynchronizer
    {
        /// <summary>
        /// Initialize
        /// </summary>
        /// <returns>Self</returns>
        ISynchronizer Initialize();

        /// <summary>
        /// Run
        /// </summary>
        /// <returns>Self</returns>
        ISynchronizer Run();
    }
    #endregion
}