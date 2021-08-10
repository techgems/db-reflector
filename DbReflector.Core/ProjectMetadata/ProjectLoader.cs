using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.CodeAnalysis;
using System.Linq;
using System;
using DbReflector.Core.Exceptions;

namespace DbReflector.Core.ProjectMetadata
{
    public class ProjectLoader : IProjectLoader
    {
        public VSProjectMetadata GetCSharpProjectMetadata(string projectPath)
        {
            var workspace = MSBuildWorkspace.Create();
            Project project;

            try
            {
                project = workspace.OpenProjectAsync(projectPath).Result;
            }
            catch(Exception e)
            {
                throw new ProjectLoadException("The requested project failed to load. Make sure the path to the project file is correct.", e);
            }
            

            if(project.Language != "C#")
            {
                throw new ProjectLoadException("Language not supported");
            }

            if(project.FilePath == null)
            {
                throw new ProjectLoadException("Project didn't load correctly.");
            }

            var defaultNamespace = project.DefaultNamespace ?? project.Name;
            var splitList = project.FilePath.Split("\\").ToList();

            splitList.RemoveAt(splitList.Count - 1);
            var baseDirectory = string.Join("\\", splitList);

            return new VSProjectMetadata(defaultNamespace, baseDirectory, project.Language);
        }
    }
}
