using Buildalyzer;
using Buildalyzer.Workspaces;
using GiG.Core.Messaging.Avro.Schema.Abstractions.Annotations;
using Microsoft.CodeAnalysis;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace GiG.Core.Messaging.Avro.Schema.Generator.Tests.Integration.Tests
{
    [Trait("Category", "Integration")]
    public class CodeGeneratorTests
    {
        [Fact]
        public async Task CodeGenerator_GenerateSchemaFiles_Success()
        {
            // Arrange
            var assembly = typeof(CodeGeneratorTests).Assembly;
            var assemblyName = assembly.GetName().Name;
            var projectPath = assembly.Location.Substring(0, assembly.Location.IndexOf(assemblyName, StringComparison.Ordinal));
            projectPath = projectPath + assemblyName + @"/" + assemblyName + ".csproj";

            var attributeLib = typeof(NamedSchemaAttribute).Assembly;

            // Delete previous avro files - comment this out when debugging
            DeleteAvroFiles(projectPath);

            var manager = new AnalyzerManager();
            var analyzer = manager.GetProject(projectPath);
            using var workspace = analyzer.GetWorkspace();
            
            var project = workspace.CurrentSolution.Projects.Single();
            var compilation = await project.GetCompilationAsync();

            if (compilation.ReferencedAssemblyNames.Any(x => x.Name == "GiG.Core.Messaging.Avro.Schema.Abstractions") == false)
            {
                compilation = compilation.AddReferences(MetadataReference.CreateFromFile(attributeLib.Location));
            }

            // Act
            var generator = new CodeGenerator(compilation);
            generator.GenerateCode();

            // Assert
            CheckFileGeneration(projectPath);
        }

        #region Private Methods

        private void DeleteAvroFiles(string projectPath)
        {
            var manager = new AnalyzerManager();
            var analyzer = manager.GetProject(projectPath);
            using var workspace = analyzer.GetWorkspace();
            var project = workspace.CurrentSolution.Projects.Single();

            var avroFiles = project.Documents.Where(x => x.Name.EndsWith(".Avro.cs")).ToList();
            avroFiles.ForEach(x => File.Delete(x.FilePath));
        }

        private void CheckFileGeneration(string projectPath)
        {
            var manager = new AnalyzerManager();
            var analyzer = manager.GetProject(projectPath);
            using var workspace = analyzer.GetWorkspace();

            var project = workspace.CurrentSolution.Projects.Single();

            var addressAvro = project.Documents.FirstOrDefault(x => x.Name.Equals("Address.Avro.cs"));
            var personAvro = project.Documents.FirstOrDefault(x => x.Name.Equals("Person.Avro.cs"));
            var regionAvro = project.Documents.FirstOrDefault(x => x.Name.Equals("Region.Avro.cs"));

            Assert.NotNull(addressAvro);
            Assert.NotNull(personAvro);
            Assert.NotNull(regionAvro);

            // TODO add checks per file
        }

        #endregion
    }
}
