using CodeGenerationRoslynTest.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Npgsql;
using CodeGenerationRoslynTest.Models;
using CodeGenerationRoslynTest.Models.Generator;

namespace CodeGenerationRoslynTest.DbScanners
{
    public class PostgresDatabaseScanner : IDbScanner<PostgresTable, PostgresColumn>
    {
        private string ConnectionString { get; set; }

        public PostgresDatabaseScanner(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public List<PostgresTable> GetTablesFromDatabase(string databaseName, string schema = "public")
        {
            using (var conn = new NpgsqlConnection(ConnectionString))
            {
                var tablesQuery = "select * from information_schema.tables where table_catalog = @database and table_schema = @schema";

                var tables = conn.Query<PostgresTable>(tablesQuery,
                    new { database = databaseName, schema = schema }).ToList();

                return tables;
            }
        }

        public List<PostgresColumn> GetColumnsFromDatabase(string databaseName, string schema = "public")
        {
            using (var conn = new NpgsqlConnection(ConnectionString))
            {
                var columnsQuery = "select * from information_schema.columns where table_catalog = @database and table_schema = @schema";

                var columns = conn.Query<PostgresColumn>(columnsQuery, new { database = databaseName, schema = schema }).ToList();

                return columns;
            }
        }

        public List<PostgresColumn> GetColumnsFromDatabaseWithPK(string databaseName, string schema)
        {
            using(var conn = new NpgsqlConnection(ConnectionString))
            {
                var columnsWithPrimaryKeyQuery = @"select distinct(col.column_name), col.table_name, col.table_catalog 
                                                    from information_schema.columns as col
                                                    left join information_schema.constraint_column_usage as cus on col.column_name = cus.column_name
                                                    left join information_schema.table_constraints as con on con.constraint_name = cus.constraint_name 
                                                    where col.table_catalog = @database and col.table_schema = @schema and con.constraint_type = 'PRIMARY KEY'";

                var columns = conn.Query<PostgresColumn>(columnsWithPrimaryKeyQuery, new { database = databaseName, schema = schema }).ToList();

                return columns;
            }
        }
    }
}
