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

        public void Reflect(ReflectCommandModel command)
        {
            var databaseMetadata = new Database();
            var projectMetadata = _projectLoader.GetCSharpProjectMetadata(command.CSharpProjectFilePath);

            switch (command.DatabaseEngine)
            {
                case SupportedDatabases.Postgres:
                    databaseMetadata = _postgresMetadataMapper.CreateGeneratorModel(command.ConnectionString, command.DatabaseName, command.TablesToIgnore);
                    break;
                case SupportedDatabases.SqlServer:
                    databaseMetadata = _sqlServerMetadataMapper.CreateGeneratorModel(command.ConnectionString, command.DatabaseName, command.TablesToIgnore);
                    break;
            }

            //Entities must always be generated.
            _entityGenerator.Generate(command.EntitiesFolder, command.ForceRecreate, projectMetadata, databaseMetadata);

            _mapperGenerator.Generate(command.EntitiesFolder, command.ForceRecreate, projectMetadata, databaseMetadata);

        }

        public void Scan(ScanCommandModel command)
        {
            try
            {
                var databaseMetadata = new Database();

                switch (command.DatabaseEngine)
                {
                    case SupportedDatabases.Postgres:
                        databaseMetadata = _postgresMetadataMapper.CreateGeneratorModel(command.ConnectionString, command.DatabaseName, command.TablesToIgnore);
                        break;
                    case SupportedDatabases.SqlServer:
                        databaseMetadata = _sqlServerMetadataMapper.CreateGeneratorModel(command.ConnectionString, command.DatabaseName, command.TablesToIgnore);
                        break;
                }
            }
            catch (Exception e)
            {

            }
        }
    }
}

