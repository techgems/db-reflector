using System;
using System.Threading.Tasks;
using CodeGenerationRoslynTest.Converters;
using CodeGenerationRoslynTest.DbScanners;
using CodeGenerationRoslynTest.Models.Config;
using System.Collections.Generic;
using CodeGenerationRoslynTest.Models.Database;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using CodeGenerationRoslynTest.Generators;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using CodeGenerationRoslynTest.Generators.Interfaces;
using CodeGenerationRoslynTest.VisualStudio;
using CodeGenerationRoslynTest.Exceptions;
using CodeGenerationRoslynTest.Models.Generator;

namespace CodeGenerationRoslynTest
{
    class Program
    {
        static CommandLineConfiguration Configuration = new CommandLineConfiguration();

        private readonly IMetadataMapper _postgresMetadataMapper;
        private readonly IProjectLoader _projectLoader;
        private readonly IGenerator _entityGenerator;
        private readonly IGenerator _mapperGenerator;

        public Program(IMetadataMapper metadataMapper, IEnumerable<IGenerator> generators, IProjectLoader projectLoader)
        {
            _postgresMetadataMapper = metadataMapper;
            _projectLoader = projectLoader;

            foreach(var generator in generators)
            {
                if(generator.GetType() == typeof(EntityGenerator))
                {
                    _entityGenerator = generator;
                }
                else if (generator.GetType() == typeof(RepoDbMapperGenerator))
                {
                    _mapperGenerator = generator;
                }
            }
        }

        static void InitConfig()
        {
            Configuration = new CommandLineConfiguration
            {
                CSharpProjectFilePath = "C:/Users/cjime/Desktop/Professional Projects/LaCarte/LaCarteAPI/DB/DataAccess/LaCarte.SuperAdmin.DataAccess/LaCarte.SuperAdmin.DataAccess.csproj",
                //CSharpProjectFilePath = "C:/Users/aroger/Desktop/Carlos/LaCarteAPI/DB/DataAccess/LaCarte.SuperAdmin.DataAccess/LaCarte.SuperAdmin.DataAccess.csproj",
                GenerateRepoDbMapper = true,
                DatabaseName = "la-carte-admin",
                ConnectionString = "User ID=postgres;Password=123456;Server=localhost;Port=5432;Database=la-carte-admin;",
                SupportedDatabase = SupportedDatabases.Postgres,
                ForceRecreate = true,
                TablesToIgnore = new List<string>() { "VersionInfo" }
            };
        }

        static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            InitConfig();
            host.Services.GetRequiredService<Program>().Run();
        }

        void Run()
        {
            try
            {
                var databaseMetadata = new Database();
                var projectMetadata = _projectLoader.GetCSharpProjectMetadata(Configuration.CSharpProjectFilePath);

                switch (Configuration.SupportedDatabase)
                {
                    case SupportedDatabases.Postgres:
                        databaseMetadata = _postgresMetadataMapper.CreateGeneratorModel(Configuration.ConnectionString, Configuration.DatabaseName, Configuration.TablesToIgnore);
                        break;
                    case SupportedDatabases.SqlServer:
                        break;
                }

                //Entities must always be generated.
                _entityGenerator.Generate(Configuration, projectMetadata, databaseMetadata);

                if (Configuration.GenerateRepoDbMapper)
                    _mapperGenerator.Generate(Configuration, projectMetadata, databaseMetadata);
            }
            catch(CodeGenerationException codeGenException)
            {
                Console.Out.WriteLine($"Code generation failed. Exception Message: {codeGenException.Message}");
            }
            catch(ProjectLoadException projectException)
            {
                Console.Out.WriteLine($"Project loading failed. Exception Message: {projectException.Message}");
            }
            catch(DatabaseScanningException databaseException)
            {
                Console.Out.WriteLine($"Database metadata failed to load. Exception Message: {databaseException.Message}");
            }
            catch(Exception e)
            {
                Console.Out.WriteLine($"An unknown error has occurred. Exception Message: {e.Message}");
            }

            Console.Out.WriteLine("Finished process.");
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args).ConfigureServices(services =>
            {
                services.AddTransient<Program>();
                services.AddTransient<IGenerator, EntityGenerator>(); 
                services.AddTransient<IGenerator, RepoDbMapperGenerator>();
                services.AddTransient<IMetadataMapper, PostgresMetadataMapper>();
                services.AddTransient<IProjectLoader, ProjectLoader>();
                services.AddTransient<IDbScanner<PostgresTable, PostgresColumn>>(provider => new PostgresDatabaseScanner(Configuration.ConnectionString));
            });
        }
    }
}
