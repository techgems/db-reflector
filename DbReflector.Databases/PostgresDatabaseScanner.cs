using DbReflector.Databases.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using RepoDb;

namespace DbReflector.Databases
{
    public class PostgresDatabaseScanner : IDbScanner<PostgresTable, PostgresColumn>
    {
        public List<PostgresTable> GetTablesFromDatabase(string connectionString, string databaseName, string schema = "public")
        {
            RepoDb.PostgreSqlBootstrap.Initialize();
            using (var conn = new NpgsqlConnection(connectionString))
            {
                var tablesQuery = "select table_catalog, table_schema, table_name from information_schema.tables where table_catalog = @database and table_schema = @schema";

                var tables = conn.ExecuteQuery<PostgresTable>(tablesQuery,
                    new { database = databaseName, schema = schema }).ToList();

                return tables;
            }
        }

        public List<PostgresColumn> GetColumnsFromDatabase(string connectionString, string databaseName, string schema = "public")
        {
            RepoDb.PostgreSqlBootstrap.Initialize();
            using (var conn = new NpgsqlConnection(connectionString))
            {
                var columnsQuery = "select * from information_schema.columns where table_catalog = @database and table_schema = @schema";

                var columns = conn.ExecuteQuery<PostgresColumn>(columnsQuery, new { database = databaseName, schema = schema }).ToList();

                return columns;
            }
        }

        public List<PostgresColumn> GetColumnsFromDatabaseWithPK(string connectionString, string databaseName, string schema)
        {
            RepoDb.PostgreSqlBootstrap.Initialize();
            using (var conn = new NpgsqlConnection(connectionString))
            {
                var columnsWithPrimaryKeyQuery = @"select distinct(col.column_name), col.table_name, col.table_catalog 
                                                    from information_schema.columns as col
                                                    left join information_schema.constraint_column_usage as cus on col.column_name = cus.column_name
                                                    left join information_schema.table_constraints as con on con.constraint_name = cus.constraint_name 
                                                    where col.table_catalog = @database and col.table_schema = @schema and con.constraint_type = 'PRIMARY KEY'";

                var columns = conn.ExecuteQuery<PostgresColumn>(columnsWithPrimaryKeyQuery, new { database = databaseName, schema = schema }).ToList();

                return columns;
            }
        }
    }
}
