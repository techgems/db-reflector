using System.Collections.Generic;
using CodeGenerationRoslynTest.Generators.Interfaces;
using CodeGenerationRoslynTest.Models.Generator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.IO;
using System;
using System.Text;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp.Formatting;
using CodeGenerationRoslynTest.Models.VisualStudio;
using CodeGenerationRoslynTest.Models.Config;
using CodeGenerationRoslynTest.Exceptions;

namespace CodeGenerationRoslynTest.Generators
{
    public class RepoDbMapperGenerator : IGenerator
    {
        public void Generate(CommandLineConfiguration cliConfig, VSProjectMetadata projectMetadata, Database database)
        {
            var mapperStringBuilder = new StringBuilder();

            mapperStringBuilder.AppendLine(UsingStatements(projectMetadata.DefaultNamespace));
            mapperStringBuilder.AppendLine();

            mapperStringBuilder.AppendLine($"namespace {projectMetadata.DefaultNamespace}");
            mapperStringBuilder.AppendLine("{");
            mapperStringBuilder.AppendLine($"    public static class {database.FormattedName}Mappers");
            mapperStringBuilder.AppendLine("    {");
            mapperStringBuilder.AppendLine("        public static void Setup()");
            mapperStringBuilder.AppendLine("        {");
            mapperStringBuilder.AppendLine("            SetTableMappers();");
            mapperStringBuilder.AppendLine("        }");
            mapperStringBuilder.AppendLine();
            mapperStringBuilder.AppendLine("        private static void SetTableMappers()");
            mapperStringBuilder.AppendLine("        {");

            foreach(var table in database.Tables)
            {
                mapperStringBuilder.AppendLine(@$"            FluentMapper.Entity<{table.FormattedTableName}>().Table(""{table.TableName}"")");
                foreach(var column in table.Columns)
                {
                    if(column.IsPrimaryKey)
                    {
                        mapperStringBuilder.AppendLine($@"                .Primary(c => c.{column.FormattedColumnName})");
                    }

                    if(column.IsIdentity)
                    {
                        mapperStringBuilder.AppendLine($@"                .Identity(c => c.{column.FormattedColumnName})");
                    }

                    mapperStringBuilder.AppendLine($@"                .Column(c => c.{column.FormattedColumnName}, ""{column.ColumnName}"")");
                }
                mapperStringBuilder.Append(";");
            }

            mapperStringBuilder.AppendLine("        }");
            mapperStringBuilder.AppendLine("    }");
            mapperStringBuilder.AppendLine("}");

            var filePath = $"{projectMetadata.BasePath}/{database.FormattedName}Mappers.cs";

            if (File.Exists(filePath))
            {
                if(cliConfig.ForceRecreate)
                {
                    WriteCodeFileToDisk(filePath, mapperStringBuilder.ToString());
                }
                else
                {
                    throw new CodeGenerationException("To be able to run the generation routine when you have previously generated code, you must set ForceRecreate to true or your target directories must be empty.");
                }
            }
            else
            {
                WriteCodeFileToDisk(filePath, mapperStringBuilder.ToString());
            }
        }

        private void WriteCodeFileToDisk(string path, string code)
        {
            using (var sourceWriter = new StreamWriter(path))
            {
                sourceWriter.WriteLine(code);
            }
        }

        private string UsingStatements(string defaultNamespace)
        {
            var usingStatements = new StringBuilder();

            usingStatements.AppendLine("using System;");
            usingStatements.AppendLine("using RepoDb;");
            usingStatements.AppendLine($"using {defaultNamespace}.Entities;");

            return usingStatements.ToString();
        }

        /*public void Generate(VSProjectMetadata projectMetadata, Database database)
        {
            var compilationUnit = SyntaxFactory.CompilationUnit();

            var systemImport = SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System"));
            var entitiesImport = SyntaxFactory.UsingDirective(SyntaxFactory.ParseName($"{projectMetadata.DefaultNamespace}.Entities"));
            var repoDbImport = SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("RepoDb"));

            compilationUnit = compilationUnit.AddUsings(systemImport, entitiesImport, repoDbImport);

            var mapperNamespace = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName(projectMetadata.DefaultNamespace)).NormalizeWhitespace();

            var mapperClass = SyntaxFactory.ClassDeclaration($"{database.Name}Mappers")
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword), SyntaxFactory.Token(SyntaxKind.StaticKeyword));

            var setupMethod = SyntaxFactory.MethodDeclaration(SyntaxFactory.ParseTypeName("void"), "Setup")
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword), SyntaxFactory.Token(SyntaxKind.StaticKeyword))
                .WithBody(SyntaxFactory.Block(SyntaxFactory.ParseStatement("SetTableMappers();")));

            var tableStatementList = new List<StatementSyntax>();

            foreach(var table in database.Tables)
            {
                var setTableMapperBodyStringBuilder = new StringBuilder();
                setTableMapperBodyStringBuilder.Append($@"FluentMapper.Entity<{table.FormattedTableName}>().Table(""{table.TableName}"")");
                
                foreach(var column in table.Columns)
                {
                    if(column.IsPrimaryKey)
                    {
                        setTableMapperBodyStringBuilder.Append($@".Primary(c => c.{column.FormattedColumnName})");
                    }

                    setTableMapperBodyStringBuilder.Append($@".Column(c => c.{column.FormattedColumnName}, ""{column.ColumnName}"")");
                    setTableMapperBodyStringBuilder.AppendLine();
                }

                setTableMapperBodyStringBuilder.Append(";").Append(Environment.NewLine);

                tableStatementList.Add(SyntaxFactory.ParseStatement(setTableMapperBodyStringBuilder.ToString()));
            }

            var setTableMappersMethod = SyntaxFactory.MethodDeclaration(SyntaxFactory.ParseTypeName("void"), "SetTableMappers")
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PrivateKeyword), SyntaxFactory.Token(SyntaxKind.StaticKeyword))
                .WithBody(SyntaxFactory.Block(tableStatementList));

            mapperClass = mapperClass.AddMembers(setupMethod, setTableMappersMethod);

            mapperNamespace = mapperNamespace.AddMembers(mapperClass);

            compilationUnit = compilationUnit.AddMembers(mapperNamespace);

            var code = compilationUnit
                .NormalizeWhitespace()
                .ToFullString();

            using(var sourceWriter = new StreamWriter($"{projectMetadata.BasePath}/{database.Name}Mappers.cs"))
            {
                sourceWriter.WriteLine(code);
            }
        }*/
    }
}
