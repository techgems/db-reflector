using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbReflector.Common.CommandModels
{
    public class GenerateCommandModel
    {
        public string DatabaseName { get; set; } = "";
        public string CSharpProjectFilePath { get; set; } = "";
        public bool GenerateRepoDbMapper { get; init; }
        public string ConnectionString { get; set; } = "";
        public SupportedDatabases DatabaseEngine { get; init; }
        public SupportedORMs ORM { get; init; }
        public string EntitiesFolder { get; set; } = "Entities";
        public string RepositoriesFolder { get; set; } = "Repositories";
        public bool ForceRecreate { get; init; }
        public EntityOutputCasing CaseToOutput { get; set; } = EntityOutputCasing.PascalCase;
    }
}
