using CodeGenerationRoslynTest.Converters;
using CodeGenerationRoslynTest.DbScanners;
using CodeGenerationRoslynTest.Exceptions;
using CodeGenerationRoslynTest.Models.Config;
using CodeGenerationRoslynTest.Models.Database;
using CodeGenerationRoslynTest.Models.Generator;
using Humanizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepoDbCodeGeneration.Converters
{
    public class SqlServerMetadataMapper : ISqlServerMetadataMapper
    {
        private readonly IDbScanner<SqlServerTable, SqlServerColumn> _databaseScanner;

        public SqlServerMetadataMapper(IDbScanner<SqlServerTable, SqlServerColumn> databaseScanner)
        {
            _databaseScanner = databaseScanner;
        }

        public Database CreateGeneratorModel(string connectionString, string dbName, List<string> tablesToIgnore, string schema = "dbo")
        {
            var tableList = new List<Table>();

            try
            {
                var tablesMetadata = _databaseScanner.GetTablesFromDatabase(dbName, schema);
                var columnsMetadata = _databaseScanner.GetColumnsFromDatabase(dbName, schema);
                var columnsWithPrimaryKeys = _databaseScanner.GetColumnsFromDatabaseWithPK(dbName, schema);

                foreach(var table in tablesMetadata)
                {
                    if(tablesToIgnore.Contains(table.TABLE_NAME))
                        continue;

                    var tableColumns = new List<Column>();

                    var columnsInTable = columnsMetadata.Where(x => x.TABLE_NAME == table.TABLE_NAME);

                    var columnsGeneratorModel = columnsInTable.Select(column =>
                    {
                        bool isNull = column.IS_NULLABLE == "YES";
                        bool isPrimary = columnsWithPrimaryKeys.Any(col => col.COLUMN_NAME == column.COLUMN_NAME);

                        bool isIdentity = column.IS_IDENTITY == "YES";

                        var type = new ColumnType(column.DATA_TYPE, isNull, SupportedDatabases.SqlServer);

                        return new Column(column.COLUMN_NAME, type, isPrimary, isIdentity);
                    }).ToList();

                    tableList.Add(new Table(table.TABLE_NAME, columnsGeneratorModel, SupportedDatabases.SqlServer));
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
