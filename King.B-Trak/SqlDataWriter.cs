namespace King.BTrak
{
    using King.Azure.Data;
    using King.BTrak.Models;
    using King.Data.Sql.Reflection;
    using King.Mapper;
    using King.Mapper.Data;
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
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
        public SqlDataWriter(string tableName, ISchemaReader reader, string connectionString)
            : this(tableName, reader, new Executor(new SqlConnection(connectionString)))
        {
        }

        /// <summary>
        /// Mockable Constructor
        /// </summary>
        public SqlDataWriter(string tableName, ISchemaReader reader, IExecutor executor)
        {
            this.tableName = tableName;
            this.reader = reader;
            this.executor = executor;
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
                    && await this.Create(SchemaTypes.StoredProcedure, "SaveTableData", sprocStatement);
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
        /// <param name="dataSet">Data Sets</param>
        /// <returns>Task</returns>
        public virtual async Task Store(IEnumerable<TableData> dataSet)
        {
            var created = await this.Initialize();
            if (created)
            {
                foreach (var data in dataSet)
                {
                    foreach (var row in data.Rows)
                    {
                        var sproc = row.Map<SqlTable>();
                        sproc.TableName = data.TableName;
                        sproc.Id = Guid.NewGuid();
                        var keys = from k in row.Keys
                                   where k != TableStorage.ETag
                                       && k != TableStorage.PartitionKey
                                       && k != TableStorage.RowKey
                                       && k != TableStorage.Timestamp
                                   select k;
                        var values = new StringBuilder();
                        foreach (var k in keys)
                        {
                            values.AppendFormat("<{0}>{1}</{0}>", k, row[k]);
                        }

                        sproc.Data = string.Format("<data>{0}</data>", values);

                        await this.executor.NonQuery(sproc);
                    }
                }
            }
            else
            {
                Trace.TraceError("Stored Procedure is not created, no data can be loaded.");
            }
        }
        #endregion
    }
}