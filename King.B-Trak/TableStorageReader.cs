namespace King.BTrak
{
    using King.Azure.Data;
    using King.BTrak.Models;
    using Microsoft.WindowsAzure.Storage.Table;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Table Storage Reader
    /// </summary>
    public class TableStorageReader : ITableStorageReader
    {
        #region Members
        /// <summary>
        /// Azure Storage Resources
        /// </summary>
        protected readonly IAzureStorageResources resources = null;

        /// <summary>
        /// Table Name
        /// </summary>
        protected readonly string tableName = null;
        #endregion

        #region Constructors
        /// <summary>
        /// 
        /// </summary>
        /// <param name="resources"></param>
        /// <param name="tableName"></param>
        public TableStorageReader(IAzureStorageResources resources, string tableName)
        {
            if (null == resources)
            {
                throw new ArgumentNullException("resources");
            }
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new ArgumentException("tableName");
            }

            this.resources = resources;
            this.tableName = tableName;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Load Tables
        /// </summary>
        /// <returns>Tables</returns>
        public IEnumerable<ITableStorage> Load()
        {
            return from t in this.resources.Tables()
                   where t.Name != this.tableName
                   select t;
        }

        /// <summary>
        /// Retrieve Data
        /// </summary>
        /// <param name="tables">Tables</param>
        /// <returns>SQL Data</returns>
        public async Task<IEnumerable<TableData>> Retrieve(IEnumerable<ITableStorage> tables)
        {
            var datas = new List<TableData>();
            foreach (var table in tables)
            {
                var data = new TableData
                {
                    TableName = table.Name,
                    Rows = await table.Query(new TableQuery()),
                };

                datas.Add(data);
            }

            return datas;
        }
        #endregion
    }
}