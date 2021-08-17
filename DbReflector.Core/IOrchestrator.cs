using DbReflector.Common.CommandModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbReflector.Core
{
    public interface IOrchestrator
    {
        void Reflect(ReflectCommandModel reflectCommand);

        void Scan(ScanCommandModel scanCommand);
    }
}
