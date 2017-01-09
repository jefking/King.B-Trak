namespace King.BTrak
{
    #region Direction
    /// <summary>
    /// Syncronization Direction
    /// </summary>
    public enum Direction : byte
    {
        /// <summary>
        /// No direction
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// SQL Server to Table Storage
        /// </summary>
        SqlToTable = 1,

        /// <summary>
        /// Table Storage to SQL Server
        /// </summary>
        TableToSql = 2,
    }
    #endregion
}