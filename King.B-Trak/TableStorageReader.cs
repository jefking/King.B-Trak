﻿namespace King.BTrak
{
    using King.Azure.Data;
    using King.BTrak.Models;
    using Microsoft.WindowsAzure.Storage.Table;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
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
        /// Default Constructor
        /// </summary>
        /// <param name="resources">Azure Storage Resources</param>
        /// <param name="tableName">Table Name</param>
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
        public async Task<IEnumerable<ITableStorage>> Load()
        {
            return from t in await this.resources.Tables()
                   where t.Name != this.tableName
                   select t;
        }

        /// <summary>
        /// Retrieve Data
        /// </summary>
        /// <param name="tables">Tables</param>
        /// <returns>Table Data</returns>
        public async Task<IEnumerable<ITableData>> Retrieve(IEnumerable<ITableStorage> tables)
        {
            var datas = new List<ITableData>();
            foreach (var table in tables)
            {
                Trace.TraceInformation("Reading from table: {0}.", table.Name);

                var data = new TableData
                {
                    TableName = table.Name,
                    Rows = await table.Query(new TableQuery()),
                };

                datas.Add(data);

                Trace.TraceInformation("Rows Read: {0}", data.Rows.Count());
            }

            return datas;
        }
        #endregion
    }
}