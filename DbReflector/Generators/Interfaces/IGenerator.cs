using CodeGenerationRoslynTest.Models.Config;
using CodeGenerationRoslynTest.Models.Generator;
using CodeGenerationRoslynTest.Models.VisualStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerationRoslynTest.Generators.Interfaces
{
    public interface IGenerator
    {
        public void Generate(CommandLineConfiguration cliConfig, VSProjectMetadata projectMetadata, Database database);
    }
}
