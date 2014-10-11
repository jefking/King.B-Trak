namespace King.BTrak
{
    using King.BTrak.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class SqlDataWriter
    {
        public const string table = @"CREATE TABLE [btrak].[{0}]
                                    (
                                        id uniqueidentifier
                                        , data xml
                                    );";

        private readonly string tableName = null;

        public SqlDataWriter(IConfigValues config)
        {
            this.tableName = config.SqlTableName;
        }

        public void CreateTable()
        {
            var tableStatement = string.Format(table, this.tableName);
        }

        public void Store(IDictionary<string, object> data)
        {

        }
    }
}