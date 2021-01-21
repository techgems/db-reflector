using CodeGenerationRoslynTest.Models.Generator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepoDbCodeGeneration.Converters
{
    public interface ISqlServerMetadataMapper
    {
        Database CreateGeneratorModel(string connectionString, string dbName, List<string> tablesToIgnore, string schema = "dbo");
    }
}
