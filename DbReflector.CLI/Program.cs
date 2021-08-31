using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbReflector.Common.CommandModels;
using DbReflector.Common;
using Microsoft.Extensions.Hosting;
using DbReflector.Core.DI;
using DbReflector.Core;
using Microsoft.Extensions.DependencyInjection;
using DbReflector.Databases.Exceptions;
using DbReflector.Core.Exceptions;
using DbReflector.CodeGeneration.Exceptions;
using Spectre.Console;

namespace DbReflector.CLI
{
    public class Program
    {
        /*static void InitConfig()
        {
            //Postgres Config
            Configuration = new CommandLineConfiguration
            {
                CSharpProjectFilePath = "C:/Users/cjime/Desktop/Professional Projects/LaCarte/LaCarteAPI/NewSolution/RestaurantAdmin/LaCarte.RestaurantAdmin.DataAccess/LaCarte.RestaurantAdmin.DataAccess.csproj",
                GenerateRepoDbMapper = true,
                DatabaseName = "restaurant-admin",
                ConnectionString = "User ID=postgres;Password=123456;Server=localhost;Port=5432;Database=restaurant-admin;",
                DatabaseEngine = SupportedDatabases.Postgres,
                ForceRecreate = true,
                TablesToIgnore = new List<string>() { "VersionInfo" } //Default migrations table for Fluent Migrator.
            };
        }*/

        private readonly IOrchestrator _orchestrator;

        public Program(IOrchestrator orchestrator)
        {
            _orchestrator = orchestrator;
        }

        static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args).ConfigureServices(services =>
            {
                services.AddTransient<Program>();
                services.AddDbReflector();
            });
        }

        static SupportedDatabases MatchDbEngineString(string dbEngine)
        {
            SupportedDatabases databaseEngine = SupportedDatabases.SqlServer;

            switch (dbEngine)
            {
                case "SQL Server":
                case "mssql":
                    databaseEngine = SupportedDatabases.SqlServer;
                    break;
                case "pgsql":
                case "Postgres":
                    databaseEngine = SupportedDatabases.Postgres;
                    break;
            }

            return databaseEngine;
        }

        void Reflect(string database, string connString, string project, string engine, bool force, List<string> ignoreList, IConsole console)
        {
            SupportedDatabases databaseEngine = MatchDbEngineString(engine);

            var commandModel = new ReflectCommandModel()
            {
                DatabaseName = database,
                CSharpProjectFilePath = project,
                EntitiesFolder = "Entities",
                TablesToIgnore = ignoreList,
                ConnectionString = connString,
                CaseToOutput = EntityOutputCasing.PascalCase,
                ForceRecreate = force,
                DatabaseEngine = databaseEngine
            };

            try
            {
                _orchestrator.Reflect(commandModel);
            }
            catch (CodeGenerationException codeGenException)
            {
                AnsiConsole.MarkupLine($"[red]Code generation failed. Exception Message: {codeGenException.Message}[/]");
            }
            catch (ProjectLoadException projectException)
            {
                AnsiConsole.MarkupLine($"[red]Project loading failed. Exception Message: {projectException.Message}[/]");
            }
            catch (DatabaseScanningException databaseException)
            {
                AnsiConsole.MarkupLine($"[red]Database metadata failed to load. Exception Message: {databaseException.Message}[/]");
            }
            catch (Exception e)
            {
                AnsiConsole.MarkupLine($"[red]An unknown error has occurred. Exception Message: {e.Message}[/]");
            }

            AnsiConsole.MarkupLine("[green]Generation was successful! X tables found. Y tables ignored.[/]");
        }

        async Task<int> Run(string[] args)
        {
            var rootCommand = new RootCommand();
            rootCommand.Name = "db-reflector";
            rootCommand.Description = "A productivity tool for automating the writing of boring Data Access code.";

            var reflectCommand = new Command("reflect")
            {
                new Option<string>(new string[] {"-d", "--database" })
                {
                    Description = "Database name.",
                    Required = true
                },
                new Option<string>(new string[] {"-c", "--conn-string" })
                {
                    Description = "Connection string.",
                    Required = true
                },
                new Option<string>(new string[] {"-p", "--project" })
                {
                    Description = "C# Project path.",
                    Required = true
                },
                new Option<string>(new string[] {"-e", "--engine" }, getDefaultValue: () => "SQL Server")
                {
                    Description = "The database engine that you want to scan. SQL Server is assumed when this option is omitted.",
                    Required = false
                },
                new Option<bool>(new string[] {"-f", "--force" }, getDefaultValue: () => false)
                {
                    Description = "If a model is already stored in disk, this flag must be set to true to override the previous model.",
                    Required = false
                },
                new Option<List<string>>(new string[] {"-i", "--ignore-list" }, getDefaultValue: () => new List<string>())
                {
                    Description = "List of tables to ignore",
                    Required = false
                }
            };

            reflectCommand.Handler = CommandHandler.Create<string, string, string, string, bool, List<string>, IConsole>(Reflect);
            rootCommand.Add(reflectCommand);

            AnsiConsole.Render(
                new FigletText("DB REFLECTOR")
                .Color(new Color(20, 20, 200))
            );

            return await rootCommand.InvokeAsync(args);
        }

        public static async Task<int> Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            return await host.Services.GetRequiredService<Program>().Run(args);
        }
    }
}
