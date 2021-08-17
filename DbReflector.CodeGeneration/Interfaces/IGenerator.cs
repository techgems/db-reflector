using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbReflector.Common;
using DbReflector.CodeGeneration.Models;

namespace DbReflector.CodeGeneration.Interfaces
{
    public interface IGenerator
    {
        public void Generate(string entitiesFolder, bool force, VSProjectMetadata projectMetadata, Database database);
    }
}
