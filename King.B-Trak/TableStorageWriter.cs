namespace King.BTrak
{
    using King.Azure.Data;
    using King.BTrak.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Table Storage Writer
    /// </summary>
    public class TableStorageWriter : ITableStorageWriter
    {
        #region Members
        /// <summary>
        /// Table Storage
        /// </summary>
        protected readonly ITableStorage table = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="table">Table Storage</param>
        public TableStorageWriter(ITableStorage table)
        {
            if (null == table)
            {
                throw new ArgumentNullException("table");
            }

            this.table = table;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Initialize
        /// </summary>
        /// <returns>Success</returns>
        public virtual async Task<bool> Initialize()
        {
            return await this.table.CreateIfNotExists();
        }

        /// <summary>
        /// Store Table Data
        /// </summary>
        /// <param name="tables">Tables</param>
        /// <returns>Task</returns>
        public virtual async Task Store(IEnumerable<ITableSqlData> tables)
        {
            foreach (var tableData in tables)
            {
                var hasPk = null != tableData.PrimaryKeyColumns && tableData.PrimaryKeyColumns.Any();
                foreach (var entity in tableData.Rows)
                {
                    entity.Add(TableStorage.PartitionKey, tableData.TableName);

                    var rowKey = string.Empty;
                    if (hasPk)
                    {
                        foreach (var col in tableData.PrimaryKeyColumns)
                        {
                            rowKey += string.IsNullOrWhiteSpace(rowKey) ? string.Empty : "_";
                            rowKey += null == entity[col] ? "(null)" : entity[col].ToString();
                        }
                    }
                    else
                    {
                        rowKey = Guid.NewGuid().ToString();
                    }

                    entity.Add(TableStorage.RowKey, rowKey);
                    await this.table.InsertOrReplace(entity);
                }
            }
        }
        #endregion
    }
}