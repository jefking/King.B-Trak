namespace King.BTrak
{
    using King.Azure.Data;
    using System.Collections.Generic;

    public class TableStorageReader
    {
        protected readonly IAzureStorageResources resources = null;

        public TableStorageReader(IAzureStorageResources resources)
        {
            this.resources = resources;
        }

        public IEnumerable<string> Load()
        {
            return this.resources.TableNames();
        }
    }
}