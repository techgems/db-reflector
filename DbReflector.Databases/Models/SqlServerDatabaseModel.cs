using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbReflector.Databases.Models
{

    public class SqlServerTable
    {
        public string TABLE_CATALOG { get; set; } = "";
        public string TABLE_SCHEMA { get; set; } = "";
        public string TABLE_NAME { get; set; } = "";
    }

    public class SqlServerColumn
    {
        public string TABLE_CATALOG { get; set; } = "";
        public string TABLE_NAME { get; set; } = "";
        public string IS_NULLABLE { get; set; } = "";
        public string IS_IDENTITY { get; set; } = ""; //This is not in the information schema columns table.
        public string DATA_TYPE { get; set; } = "";
        public string COLUMN_NAME { get; set; } = "";
    }
}
