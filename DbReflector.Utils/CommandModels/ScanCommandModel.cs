using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbReflector.Common.CommandModels
{
    public class ScanCommandModel
    {
        public string DatabaseName { get; set; }

        public string ConnectionString { get; set; }

        public string OutputFolder { get; set; }

        public SupportedDatabases DbEngine { get; set; }

        public bool ForceOverride { get; set; }

    }
}
