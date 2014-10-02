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
        /// Mockable Constructor
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
        /// Store Table Data
        /// </summary>
        /// <param name="tables">Tables</param>
        /// <returns>Task</returns>
        public virtual async Task Store(IList<TableData> tables)
        {
            foreach (var table in tables)
            {
                foreach (var entity in table.Data)
                {
                    entity.Add(TableStorage.PartitionKey, table.Name);

                    string rowKey = string.Empty;
                    if (null != table.PrimaryKeyColumns && table.PrimaryKeyColumns.Any())
                    {
                        foreach (var col in table.PrimaryKeyColumns)
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