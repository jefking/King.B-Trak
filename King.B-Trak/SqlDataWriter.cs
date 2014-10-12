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
        /// MOckable Constructor
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
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual async Task<bool> CreateTable()
        {
            var exists = (from t in await this.reader.Load(SchemaTypes.Table)
                          where t.Name == this.tableName
                          && t.Preface == SqlStatements.Schema
                          select true).FirstOrDefault();
            if (!exists)
            {
                Trace.TraceInformation("Creating table to load data into: '{0}'.", this.tableName);

                var statement = string.Format(SqlStatements.CreateTable, SqlStatements.Schema, this.tableName);
                return await this.executor.NonQuery(statement) == -1;
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual async Task<bool> CreateSproc()
        {
            var exists = (from t in await this.reader.Load(SchemaTypes.StoredProcedure)
                          where t.Name == "SaveTableData"
                           && t.Preface == SqlStatements.Schema
                          select true).FirstOrDefault();
            if (!exists)
            {
                Trace.TraceInformation("Creating stored procedure to load data into table: '{0}'.", this.tableName);

                var statement = string.Format(SqlStatements.CreateStoredProcedure, SqlStatements.Schema, this.tableName);
                return await this.executor.NonQuery(statement) == -1;
            }

            return true;
        }
        #endregion

        #region Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        public virtual async Task Store(IEnumerable<SqlData> datas)
        {
            var created = await this.CreateTable();
            if (created)
            {
                created = await this.CreateSproc();
                if (created)
                {
                    foreach (var data in datas)
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
            else
            {
                Trace.TraceError("Table is not created, no data can be loaded.");
            }
        }
        #endregion
    }
}