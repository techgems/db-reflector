using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbReflector.Common
{
    public class CommandLineConfiguration
    {
        public string DatabaseName { get; set; } = "";
        public string CSharpProjectFilePath { get; set; } = "";
        public bool GenerateRepoDbMapper { get; init; }
        public string ConnectionString { get; set; } = "";
        public SupportedDatabases DatabaseEngine { get; init; }
        public string EntitiesFolder { get; set; } = "Entities";
        public bool ForceRecreate { get; init; }
        public List<string> TablesToIgnore { get; set; } = new List<string>();
        public EntityOutputCasing CaseToOutput { get; set; } = EntityOutputCasing.PascalCase;
    }
}
