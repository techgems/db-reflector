﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbReflector.Common.CommandModels
{
    public class ReflectCommandModel
    {
        public string DatabaseName { get; set; } = "";
        public string CSharpProjectFilePath { get; set; } = "";
        public string ConnectionString { get; set; } = "";
        public SupportedDatabases DatabaseEngine { get; init; }
        public string EntitiesFolder { get; set; } = "Entities";
        public bool ForceRecreate { get; init; }
        public List<string> TablesToIgnore { get; set; } = new List<string>();
        public EntityOutputCasing CaseToOutput { get; set; } = EntityOutputCasing.PascalCase;
    }
}
