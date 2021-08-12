using DbReflector.Common;

namespace DbReflector.Core.ProjectMetadata
{
    public interface IProjectLoader
    {
        VSProjectMetadata GetCSharpProjectMetadata(string projectPath);
    }
}
