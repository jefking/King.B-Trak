namespace King.BTrak
{
    using King.Azure.Data;
    using King.BTrak.Models;
    using King.BTrak.Sql;
    using King.Data.Sql.Reflection;
    using King.Mapper;
    using King.Mapper.Data;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// SQL Data Writer
    /// </summary>
    public class SqlDataWriter : ISqlDataWriter
    {
        #region Members
        /// <summary>
        /// Table Name
        /// </summary>
        protected readonly string tableName = null;

        /// <summary>
        /// Schema Reader
        /// </summary>
        protected readonly ISchemaReader reader = null;

        /// <summary>
        /// Executor
        /// </summary>
        protected readonly IExecutor executor = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        public SqlDataWriter(ISchemaReader reader, IExecutor executor, string tableName)
        {
            if (null == reader)
            {
                throw new ArgumentNullException("reader");
            }
            if (null == executor)
            {
                throw new ArgumentNullException("executor");
            }
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new ArgumentException("tableName");
            }

            this.reader = reader;
            this.executor = executor;
            this.tableName = tableName;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Initialize
        /// </summary>
        /// <returns>Success</returns>
        public virtual async Task<bool> Initialize()
        {
            var tableStatement = string.Format(SqlStatements.CreateTable, SqlStatements.Schema, this.tableName);
            var sprocStatement = string.Format(SqlStatements.CreateStoredProcedure, SqlStatements.Schema, this.tableName);
            return await this.Create(SchemaTypes.Table, this.tableName, tableStatement)
                && await this.Create(SchemaTypes.StoredProcedure, SqlStatements.StoredProcedureName, sprocStatement);
        }

        /// <summary>
        /// Create
        /// </summary>
        /// <param name="type">Type</param>
        /// <param name="name">Name</param>
        /// <param name="statement">Statement</param>
        /// <returns>Success</returns>
        public virtual async Task<bool> Create(SchemaTypes type, string name, string statement)
        {
            var exists = (from t in await this.reader.Load(type)
                          where t.Name == name
                           && t.Preface == SqlStatements.Schema
                          select true).FirstOrDefault();
            if (!exists)
            {
                Trace.TraceInformation("Creating {0} to load data into table: '{1}'.", type, this.tableName);

                return await this.executor.NonQuery(statement) == -1;
            }

            return true;
        }

        /// <summary>
        /// Stores Data
        /// </summary>
        /// <param name="dataSets">Data Sets</param>
        /// <returns>Task</returns>
        public virtual async Task Store(IEnumerable<ITableData> dataSets)
        {
            var created = await this.Initialize();
            if (created)
            {
                foreach (var dataSet in dataSets)
                {
                    foreach (var row in dataSet.Rows)
                    {
                        var sproc = row.Map<SaveData>();
                        sproc.TableName = dataSet.TableName;
                        sproc.Id = Guid.NewGuid();
                        sproc.Data = string.Format("<data>{0}</data>",
                            string.Concat(
                                            from k in row.Keys
                                            where k != TableStorage.ETag
                                                && k != TableStorage.PartitionKey
                                                && k != TableStorage.RowKey
                                                && k != TableStorage.Timestamp
                                            select string.Format("<{0}>{1}</{0}>", k, row[k])
                                        )
                                    );

                        await this.executor.NonQuery(sproc);
                    }
                }
            }
            else
            {
                Trace.TraceError("Table or Stored Procedure is not created, no data can be loaded.");
            }
        }
        #endregion
    }
}