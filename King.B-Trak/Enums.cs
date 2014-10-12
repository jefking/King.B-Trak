namespace King.BTrak
{
    #region Direction
    /// <summary>
    /// Syncronization Direction
    /// </summary>
    public enum Direction : byte
    {
        Unknown = 0,
        SqlToTable = 1,
        TableToSql = 2,
    }
    #endregion
}