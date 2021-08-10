using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbReflector.Databases.Models
{
    public class PostgresTable
    {
        public string table_catalog { get; set; } = "";
        public string table_schema { get; set; } = "";
        public string table_name { get; set; } = "";
    }

    public class PostgresColumn
    {
        public string table_catalog { get; set; } = "";
        public string table_name { get; set; } = "";
        public string is_nullable { get; set; } = "";
        public string is_identity { get; set; } = ""; //Only for SQL Standard identity in Postgres 10 and above.
        public string udt_name { get; set; } = "";
        public string column_name { get; set; } = "";
    }
}
