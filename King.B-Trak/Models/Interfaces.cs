namespace King.BTrak.Models
{
    using System.Collections.Generic;
    
    #region IConfigValues
    /// <summary>
    /// Configuration Values
    /// </summary>
    public interface IConfigValues
    {
        #region Properties
        /// <summary>
        /// SQL Conenction String
        /// </summary>
        string SqlConnection
        {
            get;
        }

        /// <summary>
        /// Storage Account Connection String
        /// </summary>
        string StorageAccountConnection
        {
            get;
        }

        /// <summary>
        /// Storage Table Name
        /// </summary>
        string StorageTableName
        {
            get;
        }

        /// <summary>
        /// Storage Table Name
        /// </summary>
        string SqlTableName
        {
            get;
        }

        /// <summary>
        /// Sync Direction
        /// </summary>
        Direction Direction
        {
            get;
        }
        #endregion
    }
    #endregion

    #region ITableData
    /// <summary>
    /// Table Data Interface
    /// </summary>
    public interface ITableData
    {
        #region Properties
        /// <summary>
        /// Row Data
        /// </summary>
        IEnumerable<IDictionary<string, object>> Rows
        {
            get;
            set;
        }

        /// <summary>
        /// Table Name
        /// </summary>
        string TableName
        {
            get;
            set;
        }
        #endregion
    }
    #endregion

    #region ITableSqlData
    /// <summary>
    /// Table Data Interface
    /// </summary>
    public interface ITableSqlData : ITableData
    {
        #region Properties
        /// <summary>
        /// Primary Key Columns
        /// </summary>
        IEnumerable<string> PrimaryKeyColumns
        {
            get;
            set;
        }
        #endregion
    }
    #endregion
}