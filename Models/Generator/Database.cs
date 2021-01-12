using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerationRoslynTest.Models.Generator
{
    public class Database
    {
        public string Name { get; set; } = "";
        public string FormattedName { get; set; } = "";
        public List<Table> Tables { get; set; } = new List<Table>();
    }
}
