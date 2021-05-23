# Repo DB Code Generation

This project is a dev tool for entities and mapping code generation for the ORM [Repo Db](https://repodb.net/).
Repo DB is an amazing tool that grants the best of both worlds between a micro-ORM like Dapper and a full ORM like Entity Framework.
Libraries like RepoDb are amazing, but often come with the side effect that the development aids or tools for them are either very immature or non existent.

This project was inspired by Entity Framework Core's [reverse engineering tool](https://docs.microsoft.com/en-us/ef/core/managing-schemas/scaffolding?tabs=dotnet-core-cli).
The ability to scan a database and auto-generate entities and database mapping code is a great asset that can reduce time waste considerably when building new applications or when thinking in migrating an app from using EF Core to Repo Db.

## What the tool does

The tool here is a small console app that receives some configuration parameters, like the database name, the connection string and the C# project you want to generate your files into.
It then scans database tables, columns and primary keys and maps the SQL types to proper C# types. The tool assumes the C# code is to be generated with Pascal Casing naming rules regardless of the naming scheme the database may have.
It is worthy of note that the output code can be configured to be generated in some other notation like snake case or camel case.

The code generator routine also supports via parameter the generation of a mappings file. The preference was to use Repo DBs [fluent mapping](https://repodb.net/feature/classmapping) for mapping the columns to the specific C# classes.
In my opinion, fluent mapping is more expressive and customizable than attribute mapping. The mappings file has a "Setup" method that can be called from application startup and the mappings can be registered for RepoDb to use.

## Supported Databases

SQL Server and PostgreSQL are supported. MySQL support will come if there is demand for it.

## Visual Studio Support

As of now, the code written will only properly supports VS2017 and onward projects. If you're using the old project format, you should probably move your projects to the new format even if you don't plan to migrate to .NET Core soon. This is because of a change in the way that csproj files are built. You can read more about that from Scott Hanselman [here](https://www.hanselman.com/blog/upgrading-an-existing-net-project-files-to-the-lean-new-csproj-format-from-net-core).

## Future of the project

I plan to work on the generation of default [repositories](https://repodb.net/feature/repositories). Repositories often require more customization, so this will imply not just code generation, but code analysis so that the code generator can be smart enough to respect the code that the developer has written and not cause loss of work.

If demand is good enough, this could also be turned into a proper CLI that can be installed via nuget into any project, or as a Visual Studio Extension. But time will tell.

## Suggested use

Use this tool along a migration tool. I personally use [FluentMigrator](https://fluentmigrator.github.io/), but there are others you can use ([Evolve](https://evolve-db.netlify.app/), [Roundhouse](https://github.com/chucknorris/roundhouse)). That way you can have both database source control and a tool that facilitates adapting from the output of the migrations.
