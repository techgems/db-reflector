using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbReflector.Databases.EntityDataModel
{
    public enum DbEngine
    {
        MSSQL19,
        PGSQL12
    }

    public record Database(string dbEngine);

    public record Schema(List<Table> Tables, List<View> Views, List<Sequences> Sequences);

    public record Table(string Name, List<TableColumn> Columns);

    public record TableColumn(string Name, string Type, string IsPrimaryKey, string IsNullable, ColumnForeignKey ForeignKey, string IndexName);

    public record ColumnForeignKey(string ToSchema, string ToTable, string ToColumn);

    public record View(List<ViewColumn> Columns);

    public record ViewColumn(string Name, string Type);

    //TO DO: Probably better if it's not an int.
    public record Sequences(string Name, string Type, int StartValue, int MaxValue);


}
