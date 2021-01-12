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
            switch(databaseType)
            {
                case "bool":
                case "boolean":
                    if(!IsNullable) 
                    { 
                        CSharpTypeString = "bool";
                    }
                    else 
                    { 
                        CSharpTypeString = "bool?";
                    }
                    break;
                case "uuid":
                    if (!IsNullable)
                    {
                        CSharpTypeString = "Guid";
                    }
                    else
                    {
                        CSharpTypeString = "Guid?";
                    }
                    break;
                case "int2":
                    if (!IsNullable)
                    {
                        CSharpTypeString = "short";
                    }   
                    else
                    { 
                        CSharpTypeString = "short?";
                    }
                    break;
                case "int4":
                    if (!IsNullable)
                    { 
                        CSharpTypeString = "int";
                    }
                    else
                    { 
                        CSharpTypeString = "int?";
                    }
                    break;
                case "int8":
                    if (!IsNullable)
                    {
                        CSharpTypeString = "long";
                    }
                    else
                    {
                        CSharpTypeString = "long?";
                    }
                    break;
                case "money":
                    if (!IsNullable)
                    {
                        CSharpTypeString = "decimal";
                    }
                    else
                    {
                        CSharpTypeString = "decimal?";
                    }
                    break;
                case "varchar":
                case "text":
                case "json":
                case "jsonb":
                    CSharpTypeString = "string";
                    break;
                case "date":
                case "timestamp":
                    if (!IsNullable)
                    {
                        CSharpTypeString = "DateTime";
                    }
                    else
                    {
                        CSharpTypeString = "DateTime?";
                    }
                    break;
            }
        }

        private string MapSqlServerTypeToCSharpType(string databaseType)
        {
            return "DOESN'T WORK";
        }
    }
}
