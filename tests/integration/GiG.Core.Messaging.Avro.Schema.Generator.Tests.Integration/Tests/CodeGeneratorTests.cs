using GiG.Core.Messaging.Avro.Schema.Generator.MSBuild;
using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace GiG.Core.Messaging.Avro.Schema.Generator.Tests.Integration.Tests
{
    public class CodeGeneratorTests
    {
        private CodeGeneratorCommand _cmd;

        private List<string> _docs = new List<string>();
        private List<string> _references = new List<string>();

        public CodeGeneratorTests()
        {
            MSBuildLocator.RegisterDefaults();
        }

        [Fact]
        public async Task CodeGenerator_GenerateSchemaFiles_Success()
        {
            // Arrange
            Assembly assembly = typeof(CodeGeneratorTests).Assembly;
            var assemblyName = assembly.GetName().Name;
            var projPath = assembly.Location.Substring(0, assembly.Location.IndexOf(assemblyName));
            projPath = projPath + assemblyName + @"\" + assemblyName + ".csproj";

            // Delete previous avro files
            await DeleteAvroFilesAsync(projPath);

            GetDependencies();
            await GetProjectDocsAsync(projPath);

            using (new AssemblyResolver())
            {
                _cmd = new CodeGeneratorCommand { };
                _cmd.DefineConstants.AddRange(new List<string> { "TRACE", "DEBUG", "NETCOREAPP", "NETCOREAPP3_1" });
                _cmd.OutputType = "Library";
                _cmd.AssemblyName = assemblyName;
                _cmd.TargetPath = assembly.Location;
                _cmd.ProjectPath = projPath;
                _cmd.Compile.AddRange(_docs);
                _cmd.Reference.AddRange(_references);
            }

            var projectName = Path.GetFileNameWithoutExtension(_cmd.ProjectPath);
            var projectId = !string.IsNullOrEmpty(_cmd.ProjectGuid) && Guid.TryParse(_cmd.ProjectGuid, out var projectIdGuid)
                ? ProjectId.CreateFromSerialized(projectIdGuid)
                : ProjectId.CreateNewId();

            var projectInfo = ProjectInfo.Create(
                projectId,
                VersionStamp.Default,
                projectName,
                projectName,
                LanguageNames.CSharp,
                _cmd.ProjectPath,
                _cmd.TargetPath,
                CreateCompilationOptions(_cmd),
                documents: GetDocuments(_cmd.Compile, projectId),
                metadataReferences: GetMetadataReferences(_cmd.Reference),
                parseOptions: new CSharpParseOptions(preprocessorSymbols: _cmd.DefineConstants)
            );

            var workspace = new AdhocWorkspace();
            workspace.AddProject(projectInfo);
            var project = workspace.CurrentSolution.Projects.Single();
            var compilation = await project.GetCompilationAsync(new CancellationToken());

            // Act
            var generator = new CodeGenerator(compilation);
            generator.GenerateCode(new CancellationToken());

            // Assert
            await CheckFileGenerationAsync(projPath);
        }

        #region Private Methods

        private async Task DeleteAvroFilesAsync(string projPath)
        {
            using (var workspace = MSBuildWorkspace.Create())
            {
                var project = await workspace.OpenProjectAsync(projPath);
                var avroFiles = project.Documents.Where(x => x.Name.EndsWith(".Avro.cs")).ToList();
                avroFiles.ForEach(x => File.Delete(x.FilePath));
            }
        }

        private async Task GetProjectDocsAsync(string projPath)
        {
            using (var workspace = MSBuildWorkspace.Create())
            {
                var project = await workspace.OpenProjectAsync(projPath);
                _docs = project.Documents.Select(x => x.FilePath).ToList();
            }
        }

        private List<string> GetDependencies(Assembly assembly)
        {
            var references = assembly.GetReferencedAssemblies()
                  .Select(Assembly.Load);

            List<string> dependencies = new List<string>();
            dependencies.AddRange(references.Select(x => x.Location).ToList());
            return dependencies;
        }

        private void GetDependencies()
        {
            Assembly assembly = typeof(CodeGeneratorTests).Assembly;
            var referencies = assembly.GetReferencedAssemblies()
                    .Select(Assembly.Load);

            _references.AddRange(referencies.Select(x => x.Location).ToList());
            var runtimeAssembly = referencies.Where(x => x.GetName().Name.Equals("System.Runtime")).FirstOrDefault();
            _references.AddRange(GetDependencies(runtimeAssembly));

            var abstractionAssembly = referencies.Where(x => x.GetName().Name.Equals("GiG.Core.Messaging.Avro.Schema.Abstractions")).FirstOrDefault();
            _references.AddRange(GetDependencies(abstractionAssembly));
        }

        private async Task CheckFileGenerationAsync(string projPath)
        {
            using (var workspace = MSBuildWorkspace.Create())
            {
                var project = await workspace.OpenProjectAsync(projPath);
                var addressAvro = project.Documents.FirstOrDefault(x => x.Name.Equals("Address.Avro.cs"));
                var personAvro = project.Documents.FirstOrDefault(x => x.Name.Equals("Person.Avro.cs"));
                var regionAvro = project.Documents.FirstOrDefault(x => x.Name.Equals("Region.Avro.cs"));

                Assert.NotNull(addressAvro);
                Assert.NotNull(personAvro);
                Assert.NotNull(regionAvro);

                // TODO add checks per file
            }
        }

        private static CompilationOptions CreateCompilationOptions(CodeGeneratorCommand command)
        {
            OutputKind kind;
            switch (command.OutputType)
            {
                case "Exe":
                    kind = OutputKind.ConsoleApplication;
                    break;
                case "Module":
                    kind = OutputKind.NetModule;
                    break;
                case "Winexe":
                    kind = OutputKind.WindowsApplication;
                    break;
                default:
                    kind = OutputKind.DynamicallyLinkedLibrary;
                    break;
            }

            return new CSharpCompilationOptions(kind)
                .WithMetadataImportOptions(MetadataImportOptions.All)
                .WithAllowUnsafe(true)
                .WithConcurrentBuild(true)
                .WithOptimizationLevel(OptimizationLevel.Debug);
        }

        private static IEnumerable<MetadataReference> GetMetadataReferences(IEnumerable<string> references) =>
            references
                ?.Where(File.Exists)
                .Select(x => MetadataReference.CreateFromFile(x))
            ?? (IEnumerable<MetadataReference>)Array.Empty<MetadataReference>();

        private static IEnumerable<DocumentInfo> GetDocuments(IEnumerable<string> sources, ProjectId projectId) =>
            sources
                ?.Where(File.Exists)
                .Select(x => DocumentInfo.Create(
                    DocumentId.CreateNewId(projectId),
                    Path.GetFileName(x),
                    loader: TextLoader.From(
                        TextAndVersion.Create(
                            SourceText.From(File.ReadAllText(x)), VersionStamp.Create())),
                    filePath: x))
            ?? Array.Empty<DocumentInfo>();

        #endregion
    }
}
