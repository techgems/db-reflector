using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbReflector.Common.CommandModels;
using DbReflector.Common;

namespace DbReflector.CLI
{
    //TO DO: Add DI.
    public class Program
    {
        static void Reflect()
        {

        }

        static void Generate()
        {

        }

        static void Scan(string dbName, string connectionString, string outputFolder, string dbEngine, bool forceOverride, IConsole console)
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

            var commandModel = new ScanCommandModel()
            {
                DatabaseName = dbName,
                ConnectionString = connectionString,
                OutputFolder = outputFolder,
                ForceOverride = forceOverride,
                DbEngine = databaseEngine
            };

            //Execute command.
        }

        public static async Task<int> Main(string[] args)
        {
            var rootCommand = new RootCommand();
            rootCommand.Name = "db-reflector";
            rootCommand.Description = "A productivity tool for automating the writing of boring Data Access code.";

            var scanCommand = new Command("scan") {
                new Option<string>("-db")
                {
                    Description = "The name of the database to scan.",
                    Required = true
                },
                new Option<string>("-c")
                {
                    Description = "The connection string to utilize.",
                    Required = true
                },
                new Option<string>("-o")
                {
                    Description = "The directory to store the database JSON model.",
                    Required = true
                },
                new Option<string>("-e", getDefaultValue: () => "SQL Server")
                {
                    Description = "The database engine that you want to scan. SQL Server is assumed when this option is omitted.",
                    Required = false
                },

                new Option<bool>("-f", getDefaultValue: () => false)
                {
                    Description = "If a model is already stored in disk, this flag must be set to true to override the previous model.",
                    Required = false
                }
            };

            scanCommand.Handler = CommandHandler.Create<string, string, string, string, bool, IConsole>(Scan);
            rootCommand.Add(scanCommand);

            var generateCommand = new Command("generate");
            rootCommand.Add(generateCommand);

            var reflectCommand = new Command("reflect");
            rootCommand.Add(reflectCommand);
            return await rootCommand.InvokeAsync(args);
        }
    }
}
