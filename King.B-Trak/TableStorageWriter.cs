namespace King.BTrak
{
    using King.Azure.Data;
    using King.BTrak.Models;
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Table Storage Writer
    /// </summary>
    public class TableStorageWriter
    {
        #region Members
        /// <summary>
        /// Table Storage
        /// </summary>
        protected readonly ITableStorage table = null;
        #endregion

        #region Constructors
        public TableStorageWriter(ITableStorage table)
        {
            this.table = table;
        }
        #endregion

        #region Methods
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
                            if (null == entity[col])
                            {
                                rowKey += "(null)";
                            }
                            else
                            {
                                rowKey += entity[col].ToString();
                            }
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