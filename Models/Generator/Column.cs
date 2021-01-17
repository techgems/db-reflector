using CodeGenerationRoslynTest.Models.Config;
using Humanizer;

namespace CodeGenerationRoslynTest.Models.Generator
{
    public class Column
    {
        public Column(string columnName, ColumnType type, bool isPrimaryKey, bool isIdentity, EntityOutputCasing casing = EntityOutputCasing.PascalCase)
        {
            ColumnName = columnName;
            Type = type;
            Casing = casing;
            IsPrimaryKey = isPrimaryKey;
        }

        public bool IsIdentity { get; set; }
        public bool IsPrimaryKey { get; set; }
        public string ColumnName { get; set; }
        public string FormattedColumnName
        {
            get
            {
                switch(Casing)
                {
                    case EntityOutputCasing.KeepSource:
                        return ColumnName;
                    case EntityOutputCasing.PascalCase:
                        return ColumnName.Singularize().Pascalize();
                    case EntityOutputCasing.SnakeCase:
                        return ColumnName.Singularize().Underscore();
                    default:
                        return "";
                }
            }
        }

        public ColumnType Type { get; set; }
        public EntityOutputCasing Casing { get; set; }
    }
}
