using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbReflector.Databases
{
    public interface IDbScanner<TableType, ColumnType>
    {
        List<TableType> GetTablesFromDatabase(string databaseName, string schema);
        List<ColumnType> GetColumnsFromDatabase(string databaseName, string schema);
        List<ColumnType> GetColumnsFromDatabaseWithPK(string databaseName, string schema);
    }
}
