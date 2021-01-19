using CodeGenerationRoslynTest.Models.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerationRoslynTest.Models.Generator
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

            CSharpTypeString = (IsNullable && !isReferenceType) ? $"{basicType}?" : basicType;
        }

        private string MapSqlServerTypeToCSharpType(string databaseType)
        {
            return "DOESN'T WORK";
        }
    }
}
