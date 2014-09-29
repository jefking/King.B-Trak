namespace King.BTrak
{
    using King.Azure.Data;
    using King.BTrak.Models;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class TableStorageWriter
    {
        #region Members
        /// <summary>
        /// Table Storage
        /// </summary>
        protected readonly ITableStorage table = null;
        #endregion

        public TableStorageWriter(ITableStorage table)
        {
            this.table = table;
        }

        public virtual async Task Store(IList<TableData> tables)
        {
            foreach (var table in tables)
            {
                foreach (var entity in table.Data)
                {
                    entity.Add(TableStorage.PartitionKey, table.Name);
                    entity.Add(TableStorage.RowKey, Guid.NewGuid());//Temporary, needs to be based off of Primary Key
                    await this.table.InsertOrReplace(entity);
                }
            }
        }
    }
}