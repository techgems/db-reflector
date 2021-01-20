using CodeGenerationRoslynTest.DbScanners;
using CodeGenerationRoslynTest.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepoDbCodeGeneration.DbScanners
{
    public class SqlServerDatabaseScanner : IDbScanner<SqlServerTable, SqlServerColumn>
    {
        public List<SqlServerColumn> GetColumnsFromDatabase(string databaseName, string schema)
        {
            throw new NotImplementedException();
        }

        public List<SqlServerColumn> GetColumnsFromDatabaseWithPK(string databaseName, string schema)
        {
            throw new NotImplementedException();
        }

        public List<SqlServerColumn> GetColumnsFromTable(string tableName)
        {
            throw new NotImplementedException();
        }

        public List<SqlServerTable> GetTablesFromDatabase(string databaseName, string schema)
        {
            throw new NotImplementedException();
        }
    }
}
