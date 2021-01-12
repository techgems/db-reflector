using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerationRoslynTest.Models.VisualStudio
{
    public record VSProjectMetadata(string DefaultNamespace, string BasePath, string Language);
}
