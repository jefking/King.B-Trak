namespace King.BTrak
{
    using King.Azure.Data;
    using King.BTrak.Models;
    using Microsoft.WindowsAzure.Storage.Table;
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
        public TableStorageReader(IAzureStorageResources resources, string tableName)
        {
            this.resources = resources;
            this.tableName = tableName;
        }
        #endregion

        #region Methods
        public IEnumerable<ITableStorage> Load()
        {
            return from t in this.resources.Tables()
                   where t.Name != this.tableName
                   select t;
        }

        public async Task<IEnumerable<SqlData>> Retrieve(IEnumerable<ITableStorage> tables)
        {
            var datas = new List<SqlData>();
            foreach (var table in tables)
            {
                var data = new SqlData
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