using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbReflector.Core.ProjectMetadata
{
    public record VSProjectMetadata(string DefaultNamespace, string BasePath, string Language);

    public interface IProjectLoader
    {
        VSProjectMetadata GetCSharpProjectMetadata(string projectPath);
    }
}
