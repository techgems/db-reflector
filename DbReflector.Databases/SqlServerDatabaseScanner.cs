using DbReflector.Databases.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbReflector.Databases
{
    public class SqlServerDatabaseScanner : IDbScanner<SqlServerTable, SqlServerColumn>
    {
        public List<SqlServerColumn> GetColumnsFromDatabase(string connectionString, string databaseName, string schema)
        {
            using(var conn = new SqlConnection(connectionString))
            {
                var columnsQuery = @"SELECT *,
                                         CASE 
                                            WHEN COLUMNPROPERTY(object_id(TABLE_SCHEMA+'.'+TABLE_NAME), COLUMN_NAME, 'IsIdentity') = 1 THEN 'YES'
                                            ELSE 'NO'
                                         END as IS_IDENTITY
                                     FROM INFORMATION_SCHEMA.COLUMNS
                                     WHERE table_catalog = @database and table_schema = @schema";

                var columns = conn.Query<SqlServerColumn>(columnsQuery, new { database = databaseName, schema = schema }).ToList();

                return columns;
            }
        }

        public List<SqlServerColumn> GetColumnsFromDatabaseWithPK(string connectionString, string databaseName, string schema)
        {
            using(var conn = new SqlConnection(connectionString))
            {
                var columnsWithPrimaryKeyQuery = @"SELECT  CC.COLUMN_NAME, CC.TABLE_NAME, CC.TABLE_CATALOG
                                                   FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS C
                                                   JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS K ON C.TABLE_NAME = K.TABLE_NAME
                                                    AND C.CONSTRAINT_CATALOG = K.CONSTRAINT_CATALOG
                                                    AND C.CONSTRAINT_SCHEMA = K.CONSTRAINT_SCHEMA
                                                    AND C.CONSTRAINT_NAME = K.CONSTRAINT_NAME
                                                   JOIN INFORMATION_SCHEMA.COLUMNS AS CC ON CC.COLUMN_NAME = K.COLUMN_NAME
                                                    AND CC.TABLE_NAME = K.TABLE_NAME
                                                   WHERE C.CONSTRAINT_TYPE = 'PRIMARY KEY' and CC.TABLE_CATALOG = @database and CC.TABLE_SCHEMA = @schema";

                var columns = conn.Query<SqlServerColumn>(columnsWithPrimaryKeyQuery, new { database = databaseName, schema = schema }).ToList();

                return columns;
            }
        }

        public List<SqlServerTable> GetTablesFromDatabase(string connectionString, string databaseName, string schema)
        {
            using(var conn = new SqlConnection(connectionString))
            {
                var tablesQuery = "select * from information_schema.tables where table_catalog = @database and table_schema = @schema and table_type = 'BASE TABLE'";

                var tables = conn.Query<SqlServerTable>(tablesQuery,
                    new { database = databaseName, schema = schema }).ToList();

                return tables;
            }
        }
    }
}
