using CodeGenerationRoslynTest.Generators;
using DbReflector.CodeGeneration.Interfaces;
using DbReflector.CodeGeneration.RepoDB;
using DbReflector.Core.MetadataMappers;
using DbReflector.Core.ProjectMetadata;
using DbReflector.Databases;
using DbReflector.Databases.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbReflector.Core.DI
{
    public static class DbReflectorDI
    {
        public static void AddDbReflector(this IServiceCollection services)
        {
            services.AddTransient<IOrchestrator, Orchestrator>();
            services.AddTransient<IDbScanner<PostgresTable, PostgresColumn>>();
            services.AddTransient<IDbScanner<SqlServerTable, SqlServerColumn>>();
            services.AddTransient<IGenerator, EntityGenerator>();
            services.AddTransient<IGenerator, RepoDbMapperGenerator>();
            services.AddTransient<IPostgresMetadataMapper, PostgresMetadataMapper>();
            services.AddTransient<ISqlServerMetadataMapper, SqlServerMetadataMapper>();
            services.AddTransient<IProjectLoader, ProjectLoader>();
        }
    }
}
