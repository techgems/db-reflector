using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbReflector.Databases
{
    public interface IDbScanner<TableType, ColumnType>
    {
        List<TableType> GetTablesFromDatabase(string connectionString, string databaseName, string schema);
        List<ColumnType> GetColumnsFromDatabase(string connectionString, string databaseName, string schema);
        List<ColumnType> GetColumnsFromDatabaseWithPK(string connectionString, string databaseName, string schema);
    }
}
