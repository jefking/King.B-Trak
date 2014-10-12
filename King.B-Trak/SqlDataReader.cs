namespace King.BTrak
{
    using King.BTrak.Models;
    using King.BTrak.Sql;
    using King.Data.Sql.Reflection;
    using King.Data.Sql.Reflection.Models;
    using King.Mapper.Data;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// SQL Data Loader
    /// </summary>
    public class SqlDataReader : ISqlDataLoader
    {
        #region Members
        /// <summary>
        /// SQL Executor
        /// </summary>
        protected readonly IExecutor executor = null;

        /// <summary>
        /// Dynamic Loader
        /// </summary>
        protected readonly IDynamicLoader loader = null;
        
        /// <summary>
        /// Schema Reader
        /// </summary>
        protected readonly ISchemaReader sqlSchemaReader = null;

        /// <summary>
        /// SQL Table Name
        /// </summary>
        private readonly string sqlTableName = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        public SqlDataReader(IExecutor executor, ISchemaReader schemaReader, IDynamicLoader loader, string sqlTableName)
        {
            if (null == executor)
            {
                throw new ArgumentNullException("executor");
            }
            if (null == schemaReader)
            {
                throw new ArgumentNullException("schemaReader");
            }
            if (null == loader)
            {
                throw new ArgumentNullException("loader");
            }
            if (string.IsNullOrWhiteSpace(sqlTableName))
            {
                throw new ArgumentException("sqlTableName");
            }

            this.executor = executor;
            this.sqlSchemaReader = schemaReader;
            this.loader = loader;
            this.sqlTableName = sqlTableName;
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
                   where t.Preface != SqlStatements.Schema && t.Name != this.sqlTableName
                   select t;
        }

        /// <summary>
        /// Retrieve Table Data
        /// </summary>
        /// <param name="schemas">Schemas</param>
        /// <returns>Table Data</returns>
        public virtual async Task<IEnumerable<TableSqlData>> Retrieve(IEnumerable<IDefinition> schemas)
        {
            var tables = new List<TableSqlData>();
            foreach (var schema in schemas)
            {
                var sql = string.Format(SqlStatements.SelectDataFormat, schema.Preface, schema.Name);

                Trace.TraceInformation("Loading data from [{0}].[{1}]", schema.Preface, schema.Name);

                var ds = await this.executor.Query(sql);

                var data = new TableSqlData
                {
                    Rows = this.loader.Dictionaries(ds),
                    TableName = string.Format("[{0}].[{1}]", schema.Preface, schema.Name),
                    PrimaryKeyColumns = from v in schema.Variables
                          where v.IsPrimaryKey
                          select v.ParameterName,
                };
                tables.Add(data);

                Trace.TraceInformation("Rows Read: {0}", data.Rows.Count());
            }

            return tables;
        }
        #endregion
    }
}