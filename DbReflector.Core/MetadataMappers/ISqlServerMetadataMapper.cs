using System.Collections.Generic;
using DbReflector.CodeGeneration.Models;

namespace DbReflector.Core.MetadataMappers
{
    public interface ISqlServerMetadataMapper
    {
        Database CreateGeneratorModel(string connectionString, string dbName, List<string> tablesToIgnore, string schema = "dbo");
    }
}
