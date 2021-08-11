using System.Collections.Generic;
using DbReflector.CodeGeneration.Models;

namespace DbReflector.Core.MetadataMappers
{
    public interface IPostgresMetadataMapper
    {
        Database CreateGeneratorModel(string connectionString, string dbName, List<string> tablesToIgnore, string schema = "public");
    }
}
