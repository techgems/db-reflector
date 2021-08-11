using Humanizer;
using System.Collections.Generic;
using DbReflector.Common;


namespace DbReflector.CodeGeneration.Models
{
    public class Table
    {
        public Table(string tableName, List<Column> columns, SupportedDatabases dbEngine, EntityOutputCasing casing = EntityOutputCasing.PascalCase)
        {
            IsDefaultSchema = true;

            switch(dbEngine)
            {
                case SupportedDatabases.Postgres:
                    Schema = "public";
                    break;
                case SupportedDatabases.SqlServer:
                    Schema = "dbo";
                    break;
            }

            Columns = columns;
            TableName = tableName;
            Casing = casing;
        }

        public Table(string tableName, List<Column> columns, string schema, EntityOutputCasing casing = EntityOutputCasing.PascalCase)
        {
            Schema = schema;
            TableName = tableName;
            Columns = columns;
            IsDefaultSchema = false;
            Casing = casing;
        }

        public bool IsDefaultSchema { get; set; }
        public string? Schema { get; set; }
        public string TableName { get; set; }
        public string FormattedTableName
        {
            get
            {
                switch(Casing)
                {
                    case EntityOutputCasing.KeepSource:
                        return TableName;
                    case EntityOutputCasing.PascalCase:
                        return TableName.Singularize().Pascalize();
                    case EntityOutputCasing.SnakeCase:
                        return TableName.Singularize().Underscore();
                    default:
                        return "";
                }
            }
        }
        public List<Column> Columns { get; set; }
        public EntityOutputCasing Casing { get; set; }
    }
}
