using System;
using System.Collections.Generic;
using CodeGenerationRoslynTest.Generators;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using DbReflector.Databases;
using DbReflector.CodeGeneration.Interfaces;
using DbReflector.Common;
using DbReflector.Core.MetadataMappers;
using DbReflector.Core.ProjectMetadata;
using DbReflector.CodeGeneration.RepoDB;
using DbReflector.CodeGeneration.Models;
using DbReflector.CodeGeneration.Exceptions;
using DbReflector.Core.Exceptions;
using DbReflector.Databases.Exceptions;
using DbReflector.Databases.Models;
using DbReflector.Core.DI;
using DbReflector.Common.CommandModels;

namespace DbReflector.Core
{
    public class Orchestrator : IOrchestrator
    {
        private readonly IPostgresMetadataMapper _postgresMetadataMapper;
        private readonly ISqlServerMetadataMapper _sqlServerMetadataMapper;
        private readonly IProjectLoader _projectLoader;
        private readonly IGenerator _entityGenerator;
        private readonly IGenerator _mapperGenerator;

        public Orchestrator(ISqlServerMetadataMapper sqlServerMapper, IPostgresMetadataMapper postgresMapper, IEnumerable<IGenerator> generators, IProjectLoader projectLoader)
        {
            _sqlServerMetadataMapper = sqlServerMapper;
            _postgresMetadataMapper = postgresMapper;
            _projectLoader = projectLoader;

            foreach (var generator in generators)
            {
                if (generator.GetType() == typeof(EntityGenerator))
                {
                    _entityGenerator = generator;
                }
                else if (generator.GetType() == typeof(RepoDbMapperGenerator))
                {
                    _mapperGenerator = generator;
                }
            }
        }

        public void Execute(CommandLineConfiguration config)
        {
            try
            {
                var databaseMetadata = new Database();
                var projectMetadata = _projectLoader.GetCSharpProjectMetadata(config.CSharpProjectFilePath);

                switch (config.DatabaseEngine)
                {
                    case SupportedDatabases.Postgres:
                        databaseMetadata = _postgresMetadataMapper.CreateGeneratorModel(config.ConnectionString, config.DatabaseName, config.TablesToIgnore);
                        break;
                    case SupportedDatabases.SqlServer:
                        databaseMetadata = _sqlServerMetadataMapper.CreateGeneratorModel(config.ConnectionString, config.DatabaseName, config.TablesToIgnore);
                        break;
                }

                //Entities must always be generated.
                _entityGenerator.Generate(config, projectMetadata, databaseMetadata);

                if (config.GenerateRepoDbMapper)
                    _mapperGenerator.Generate(config, projectMetadata, databaseMetadata);
            }
            catch (CodeGenerationException codeGenException)
            {
                Console.Out.WriteLine($"Code generation failed. Exception Message: {codeGenException.Message}");
            }
            catch (ProjectLoadException projectException)
            {
                Console.Out.WriteLine($"Project loading failed. Exception Message: {projectException.Message}");
            }
            catch (DatabaseScanningException databaseException)
            {
                Console.Out.WriteLine($"Database metadata failed to load. Exception Message: {databaseException.Message}");
            }
            catch (Exception e)
            {
                Console.Out.WriteLine($"An unknown error has occurred. Exception Message: {e.Message}");
            }

            Console.Out.WriteLine("Finished process.");
        }

        public void Reflect(ReflectCommandModel reflectCommand)
        {
        }

        public void Scan(ScanCommandModel scanCommand)
        {
        }
    }
}

