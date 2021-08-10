using CodeGenerationRoslynTest.Models.VisualStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerationRoslynTest.VisualStudio
{
    public interface IProjectLoader
    {
        VSProjectMetadata GetCSharpProjectMetadata(string projectPath);
    }
}
