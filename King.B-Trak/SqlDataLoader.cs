namespace King.BTrak
{
    using King.BTrak.Models;
    using King.Data.Sql.Reflection;
    using King.Data.Sql.Reflection.Models;
    using King.Mapper.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// SQL Data Loader
    /// </summary>
    public class SqlDataLoader : ISqlDataLoader
    {
        #region Members
        /// <summary>
        /// SQL Connection
        /// </summary>
        protected readonly SqlConnection database = null;

        /// <summary>
        /// Dynamic Loader
        /// </summary>
        protected readonly IDynamicLoader loader = new DynamicLoader();
        
        /// <summary>
        /// Schema Reader
        /// </summary>
        protected readonly ISchemaReader sqlSchemaReader = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="database"></param>
        public SqlDataLoader(SqlConnection database, ISchemaReader schemaReader)
        {
            if (null == database)
            {
                throw new ArgumentNullException("database");
            }

            this.database = database;
            this.sqlSchemaReader = schemaReader;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Load Definitions
        /// </summary>
        /// <returns>Schema</returns>
        public async Task<IEnumerable<IDefinition>> Load()
        {
            return from t in await this.sqlSchemaReader.Load(SchemaTypes.Table)
                   where t.Preface != SqlStatements.Schema
                        && t.Name != "TableData"
                   select t;
        }

        /// <summary>
        /// Retrieve Table Data
        /// </summary>
        /// <param name="schemas">Schemas</param>
        /// <returns>Table Data</returns>
        public virtual IEnumerable<TableData> Retrieve(IEnumerable<IDefinition> schemas)
        {
            var tables = new List<TableData>();
            foreach (var schema in schemas)
            {
                var sql = string.Format("SELECT * FROM [{0}].[{1}] WITH(NOLOCK);", schema.Preface, schema.Name);
                Trace.TraceInformation(sql);

                var cmd = this.database.CreateCommand();
                cmd.CommandText = sql;

                var adapter = new SqlDataAdapter(cmd);
                var ds = new DataSet();
                adapter.Fill(ds);

                var data = new TableData
                {
                    Data = this.loader.Dictionaries(ds),
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