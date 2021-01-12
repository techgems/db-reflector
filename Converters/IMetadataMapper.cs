using CodeGenerationRoslynTest.Models.Generator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerationRoslynTest.Converters
{
    public interface IMetadataMapper
    {
        Database CreateGeneratorModel(string connectionString, string dbName, List<string> tablesToIgnore, string schema = "public");
    }
}
