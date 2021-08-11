using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbReflector.Core
{
    using System;
    using System.Collections.Generic;
    using CodeGenerationRoslynTest.Generators;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.DependencyInjection;
    using DbReflector.Databases;

    namespace DbReflector.CLI
    {
        public class Orchestrator
        {
            static CommandLineConfiguration Configuration = new CommandLineConfiguration();

            private readonly IPostgresMetadataMapper _postgresMetadataMapper;
            private readonly ISqlServerMetadataMapper _sqlServerMetadataMapper;
            private readonly IProjectLoader _projectLoader;
            private readonly IGenerator _entityGenerator;
            private readonly IGenerator _mapperGenerator;

            public Program(ISqlServerMetadataMapper sqlServerMapper, IPostgresMetadataMapper postgresMapper, IEnumerable<IGenerator> generators, IProjectLoader projectLoader)
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

            static void InitConfig()
            {
                //Postgres Config
                Configuration = new CommandLineConfiguration
                {
                    //CSharpProjectFilePath = "Your csproj file root here.",
                    CSharpProjectFilePath = "C:/Users/cjime/Desktop/Professional Projects/LaCarte/LaCarteAPI/NewSolution/RestaurantAdmin/LaCarte.RestaurantAdmin.DataAccess/LaCarte.RestaurantAdmin.DataAccess.csproj",
                    GenerateRepoDbMapper = true,
                    DatabaseName = "restaurant-admin",
                    ConnectionString = "User ID=postgres;Password=123456;Server=localhost;Port=5432;Database=restaurant-admin;",
                    DatabaseEngine = SupportedDatabases.Postgres,
                    ForceRecreate = true,
                    TablesToIgnore = new List<string>() { "VersionInfo" } //Default migrations table for Fluent Migrator.
                };

                //SQL Server Config
                /*Configuration = new CommandLineConfiguration
                {
                    //CSharpProjectFilePath = "Your csproj file root here.",
                    CSharpProjectFilePath = "C:/Users/cjime/Desktop/Professional Projects/SqlServerSample/SqlServerSample.csproj",
                    GenerateRepoDbMapper = true,
                    DatabaseName = "Northwind",
                    ConnectionString = "Server=localhost;Database=Northwind;Trusted_Connection=True;",
                    //ConnectionString = "User ID=postgres;Password=123456;Server=localhost;Port=5432;Database=trainme-dev;",
                    //SupportedDatabase = SupportedDatabases.Postgres,
                    DatabaseEngine = SupportedDatabases.SqlServer,
                    ForceRecreate = true,
                    TablesToIgnore = new List<string>() { "migrations" } //Default migrations table for Fluent Migrator.
                };*/
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

                    switch (Configuration.DatabaseEngine)
                    {
                        case SupportedDatabases.Postgres:
                            databaseMetadata = _postgresMetadataMapper.CreateGeneratorModel(Configuration.ConnectionString, Configuration.DatabaseName, Configuration.TablesToIgnore);
                            break;
                        case SupportedDatabases.SqlServer:
                            databaseMetadata = _sqlServerMetadataMapper.CreateGeneratorModel(Configuration.ConnectionString, Configuration.DatabaseName, Configuration.TablesToIgnore);
                            break;
                    }

                    //Entities must always be generated.
                    _entityGenerator.Generate(Configuration, projectMetadata, databaseMetadata);

                    if (Configuration.GenerateRepoDbMapper)
                        _mapperGenerator.Generate(Configuration, projectMetadata, databaseMetadata);
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

            private static IHostBuilder CreateHostBuilder(string[] args)
            {
                return Host.CreateDefaultBuilder(args).ConfigureServices(services =>
                {
                    services.AddTransient<Program>();
                    services.AddTransient<IDbScanner<PostgresTable, PostgresColumn>>(provider => new PostgresDatabaseScanner(Configuration.ConnectionString));
                    services.AddTransient<IDbScanner<SqlServerTable, SqlServerColumn>>(provider => new SqlServerDatabaseScanner(Configuration.ConnectionString));
                    services.AddTransient<IGenerator, EntityGenerator>();
                    services.AddTransient<IGenerator, RepoDbMapperGenerator>();
                    services.AddTransient<IPostgresMetadataMapper, PostgresMetadataMapper>();
                    services.AddTransient<ISqlServerMetadataMapper, SqlServerMetadataMapper>();
                    services.AddTransient<IProjectLoader, ProjectLoader>();
                });
            }
        }
    }

}
