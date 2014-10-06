namespace King.BTrak
{
    using King.Azure.Data;
    using Microsoft.WindowsAzure.Storage.Table;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class TableStorageReader
    {
        protected readonly IAzureStorageResources resources = null;

        public TableStorageReader(IAzureStorageResources resources)
        {
            this.resources = resources;
        }

        public IEnumerable<ITableStorage> Load()
        {
            return this.resources.Tables();
        }

        public async Task<IEnumerable<IDictionary<string, object>>> Retrieve(IEnumerable<ITableStorage> tables)
        {
            var data = new List<IDictionary<string, object>>();
            foreach (var table in tables)
            {
                var add = await table.Query(new TableQuery());
                data.AddRange(add);
            }

            return data;
        }
    }
}