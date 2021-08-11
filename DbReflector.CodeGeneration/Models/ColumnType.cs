using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbReflector.Common;

namespace DbReflector.CodeGeneration.Models
{
    public class ColumnType
    {
        public ColumnType(string databaseType, bool isNullable, SupportedDatabases dbEngine)
        {
            DatabaseType = databaseType;
            IsNullable = isNullable;

            switch (dbEngine)
            {
                case SupportedDatabases.Postgres:
                    MapPostgresTypeToCSharpType(databaseType);
                    break;
                case SupportedDatabases.SqlServer:
                    MapSqlServerTypeToCSharpType(databaseType);
                    break;
            }
        }

        public string DatabaseType { get; set; } = "";
        public string CSharpTypeString { get; set; } = "";
        public bool IsNullable { get; set; }

        private void MapPostgresTypeToCSharpType(string databaseType) 
        {
            var basicType = "";
            var isReferenceType = false;

            switch(databaseType)
            {
                case "bool":
                case "boolean":
                    basicType = "bool";
                    break;
                case "uuid":
                    basicType = "Guid";
                    break;
                case "int2":
                    basicType = "short";
                    break;
                case "int4":
                    basicType = "int";
                    break;
                case "int8":
                    basicType = "long";
                    break;
                case "float8":
                    basicType = "double";
                    break;
                case "money":
                    basicType = "decimal";
                    break;
                case "varchar":
                case "text":
                case "json":
                case "jsonb":
                    isReferenceType = true;
                    basicType = "string";
                    break;
                case "date":
                case "time":
                case "timestamp":
                    basicType = "DateTime";
                    break;
            }

            CSharpTypeString = (!string.IsNullOrEmpty(basicType) && IsNullable && !isReferenceType) ? $"{basicType}?" : basicType;
        }

        private void MapSqlServerTypeToCSharpType(string databaseType)
        {
            var basicType = "";
            var isReferenceType = false;

            switch(databaseType)
            {
                case "bit":
                    basicType = "bool";
                    break;
                case "uniqueidentifier":
                    basicType = "Guid";
                    break;
                case "tinyint":
                case "smallint":
                    basicType = "short";
                    break;
                case "int":
                    basicType = "int";
                    break;
                case "bigint":
                    basicType = "long";
                    break;
                case "float":
                    basicType = "float";
                    break;
                case "real":
                    basicType = "double";
                    break;
                case "money":
                case "decimal":
                    basicType = "decimal";
                    break;
                case "varchar":
                case "nvarchar":
                case "ntext":
                case "text":
                case "nchar":
                case "char":
                    isReferenceType = true;
                    basicType = "string";
                    break;
                case "date":
                case "time":
                case "datetime":
                case "datetime2":
                case "datetimeoffset":
                case "smalldatetime":
                    basicType = "DateTime";
                    break;
            }

            CSharpTypeString = (!string.IsNullOrEmpty(basicType) && IsNullable && !isReferenceType) ? $"{basicType}?" : basicType;
        }
    }
}
