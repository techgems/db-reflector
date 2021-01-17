using CodeGenerationRoslynTest.DbScanners;
using CodeGenerationRoslynTest.Exceptions;
using CodeGenerationRoslynTest.Models.Config;
using CodeGenerationRoslynTest.Models.Database;
using CodeGenerationRoslynTest.Models.Generator;
using System;
using System.Collections.Generic;
using System.Linq;
using Humanizer;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerationRoslynTest.Converters
{
    public class PostgresMetadataMapper : IMetadataMapper
    {
        private readonly IDbScanner<PostgresTable, PostgresColumn> _databaseScanner;

        public PostgresMetadataMapper(IDbScanner<PostgresTable, PostgresColumn> databaseScanner)
        {
            _databaseScanner = databaseScanner;
        }

        public Database CreateGeneratorModel(string connectionString, string dbName, List<string> tablesToIgnore, string schema = "public")
        {
            var tableList = new List<Table>();

            try
            {
                var tablesMetadata = _databaseScanner.GetTablesFromDatabase(dbName, schema);
                var columnsMetadata = _databaseScanner.GetColumnsFromDatabase(dbName, schema);
                var columnsWithPrimaryKeys = _databaseScanner.GetColumnsFromDatabaseWithPK(dbName, schema);

                foreach (var table in tablesMetadata)
                {
                    if (tablesToIgnore.Contains(table.table_name))
                        continue;

                    var tableColumns = new List<Column>();

                    var columnsInTable = columnsMetadata.Where(x => x.table_name == table.table_name);

                    var columnsGeneratorModel = columnsInTable.Select(column =>
                    {
                        bool isNull = column.is_nullable == "YES";
                        bool isPrimary = columnsWithPrimaryKeys.Any(col => col.column_name == column.column_name);

                        bool isIdentity = column.is_identity == "YES";

                        var type = new ColumnType(column.udt_name, isNull, SupportedDatabases.Postgres);

                        return new Column(column.column_name, type, isPrimary, isIdentity);
                    }).ToList();

                    tableList.Add(new Table(table.table_name, columnsGeneratorModel, SupportedDatabases.Postgres));
                }

                return new Database
                {
                    Name = dbName,
                    FormattedName = dbName.Replace("-", "_").Pascalize(),
                    Tables = tableList
                };
            }
            catch(Exception e)
            {
                throw new DatabaseScanningException(@"The metadata mapper failed to read the database. 
                        Make sure your connection string is valid or that the user has the proper permissions.", e);
            }



        }
    }
}
