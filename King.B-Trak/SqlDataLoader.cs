namespace King.BTrak
{
    using King.BTrak.Models;
    using King.Data.Sql.Reflection.Models;
    using King.Mapper.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.Linq;

    public class SqlDataLoader : ISqlDataLoader
    {
        #region Members
        /// <summary>
        /// SQL Connection
        /// </summary>
        protected SqlConnection database = null;

        /// <summary>
        /// Dynamic Loader
        /// </summary>
        protected readonly IDynamicLoader loader = new DynamicLoader();
        #endregion

        #region Constructors
        public SqlDataLoader(SqlConnection database)
        {
            if (null == database)
            {
                throw new ArgumentNullException("database");
            }

            this.database = database;
        }
        #endregion

        #region Methods
        public IList<TableData> Retrieve(IEnumerable<IDefinition> schemas)
        {
            var tables = new List<TableData>();
            foreach (var schema in schemas)
            {
                var sql = string.Format("SELECT * FROM [{0}].[{1}] WITH(NOLOCK)", schema.Preface, schema.Name);
                Trace.TraceInformation(sql);

                var cmd = this.database.CreateCommand();
                cmd.CommandText = sql;
                var adapter = new SqlDataAdapter(cmd);

                var ds = new DataSet();
                adapter.Fill(ds);
                var table = ds.Tables[0];

                var data = new TableData
                {
                    Data = loader.Dictionaries(table),
                    Name = string.Format("[{0}].[{1}]", schema.Preface, schema.Name),
                    PrimaryKeyColumns = from v in schema.Variables
                          where v.IsPrimaryKey
                          select v.ParameterName,
                };
                tables.Add(data);
                Trace.TraceInformation("Rows Read: {0}", data.Data.Count());
            }

            return tables;
        }
        #endregion
    }
}