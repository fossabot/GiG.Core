using GiG.Core.Messaging.Avro.Schema.Generator.MSBuild.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace GiG.Core.Messaging.Avro.Schema.Generator.MSBuild
{
    /// <summary>
    /// Command to generate the avro schema code files.
    /// </summary>
    public class CodeGeneratorCommand
    {
        /// <summary>
        /// The Logger.
        /// </summary>
        public ILogger Log { get; set; }

        /// <summary>
        /// The MSBuild project path.
        /// </summary>
        public string ProjectPath { get; set; }

        /// <summary>
        /// The optional ProjectGuid.
        /// </summary>
        public string ProjectGuid { get; set; }

        /// <summary>
        /// The output type, such as Exe, or Library.
        /// </summary>
        public string OutputType { get; set; }

        /// <summary>
        /// The target path of the compilation.
        /// </summary>
        public string TargetPath { get; set; }

        /// <summary>
        /// The source files.
        /// </summary>
        public List<string> Compile { get; } = new List<string>();

        /// <summary>
        /// The libraries referenced by the project.
        /// </summary>
        public List<string> Reference { get; } = new List<string>();

        /// <summary>
        /// The defined constants for the project.
        /// </summary>
        public List<string> DefineConstants { get; } = new List<string>();

        /// <summary>
        /// The project's assembly name, important for id calculations.
        /// </summary>
        public string AssemblyName { get; set; }

        /// <summary>
        /// Execute Code Generation.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/></param>
        /// <returns></returns>
        public async Task Execute(CancellationToken cancellationToken)
        {
            var stopwatch = Stopwatch.StartNew();
            var projectName = Path.GetFileNameWithoutExtension(ProjectPath);
            var projectId = !string.IsNullOrEmpty(ProjectGuid) && Guid.TryParse(ProjectGuid, out var projectIdGuid)
                ? ProjectId.CreateFromSerialized(projectIdGuid)
                : ProjectId.CreateNewId();

            if (Log.IsEnabled(LogLevel.Debug))
            {
                Log.LogDebug($"AssemblyName: {AssemblyName}");
                if (!string.IsNullOrWhiteSpace(ProjectGuid)) Log.LogDebug($"ProjectGuid: {ProjectGuid}");
                Log.LogDebug($"ProjectID: {projectId}");
                Log.LogDebug($"ProjectName: {projectName}");
                Log.LogDebug($"ProjectPath: {ProjectPath}");
                Log.LogDebug($"OutputType: {OutputType}");
                Log.LogDebug($"TargetPath: {TargetPath}");
                Log.LogDebug($"DefineConstants ({DefineConstants.Count}): {string.Join(", ", DefineConstants)}");
                Log.LogDebug($"Sources ({Compile.Count}): {string.Join(", ", Compile)}");
                Log.LogDebug($"References ({Reference.Count}): {string.Join(", ", Reference)}");
            }

            var projectInfo = ProjectInfo.Create(
                projectId,
                VersionStamp.Default,
                projectName,
                AssemblyName,
                LanguageNames.CSharp,
                ProjectPath,
                TargetPath,
                CreateCompilationOptions(this),
                documents: GetDocuments(Compile, projectId),
                metadataReferences: GetMetadataReferences(Reference),
                parseOptions: new CSharpParseOptions(preprocessorSymbols: DefineConstants)
            );

            var workspace = new AdhocWorkspace();
            workspace.AddProject(projectInfo);
            
            var project = workspace.CurrentSolution.Projects.Single();
            Log.LogInformation($"Workspace creation completed in {stopwatch.ElapsedMilliseconds}ms.");
            stopwatch.Restart();

            var compilation = await project.GetCompilationAsync(cancellationToken);
            Log.LogInformation($"GetCompilation completed in {stopwatch.ElapsedMilliseconds}ms.");
            stopwatch.Restart();

            if (!compilation.SyntaxTrees.Any())
            {
                Log.LogWarning($"Skipping empty project, {compilation.AssemblyName}.");
                return;
            }

            var referencesValid = ProjectNugetReferencesAreValid(compilation);
            Log.LogInformation($"ProjectNugetReferencesAreValid completed in {stopwatch.ElapsedMilliseconds}ms.");
            if (referencesValid)
            {
                var generator = new CodeGenerator(compilation);
                generator.GenerateCode();
            }

            Log.LogInformation($"GenerateCode completed in {stopwatch.ElapsedMilliseconds}ms.");
        }
        
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

        private static IEnumerable<MetadataReference> GetMetadataReferences(IEnumerable<string> references) =>
            references
                ?.Where(File.Exists)
                .Select(x => MetadataReference.CreateFromFile(x))
            ?? (IEnumerable<MetadataReference>)Array.Empty<MetadataReference>();

        private static CompilationOptions CreateCompilationOptions(CodeGeneratorCommand command)
        {
            var kind = command.OutputType switch
            {
                "Exe" => OutputKind.ConsoleApplication,
                "Module" => OutputKind.NetModule,
                "Winexe" => OutputKind.WindowsApplication,
                _ => OutputKind.DynamicallyLinkedLibrary
            };

            return new CSharpCompilationOptions(kind)
                .WithMetadataImportOptions(MetadataImportOptions.All)
                .WithAllowUnsafe(true)
                .WithConcurrentBuild(true)
                .WithOptimizationLevel(OptimizationLevel.Debug);
        }

        private bool ProjectNugetReferencesAreValid(Compilation compilation)
        {
            const string annotationsAssemblyShortName = "GiG.Core.Messaging.Avro.Schema.Abstractions";
            const string confluentApacheAvroAssemblyShortName = "Confluent.Apache.Avro";
            const string confluentApacheAvroAssemblyVersion = "1.7.7.7";

            var projectSyntax = ReadFile(ProjectPath);

            // check for GiG.Core.Messaging.Avro.Schema.Abstractions nuget reference, and exit if it's not there (we cannot inject it as we don't know the version to use)
            var annotationsPackageMatch = new Regex($"<PackageReference\\s.*?\"{annotationsAssemblyShortName}\".*?/>", RegexOptions.Multiline).Match(projectSyntax);
            if (!annotationsPackageMatch.Success)
            {
                Log.LogError($"Project {compilation.AssemblyName} does not reference {annotationsAssemblyShortName} (references: {string.Join(", ", compilation.ReferencedAssemblyNames)})");
                return false;
            }

            // check for Confluent.Apache.Avro nuget reference, and inject it into the .csproj file if it's not there
            var avroPackageReference = $"<PackageReference Include=\"{confluentApacheAvroAssemblyShortName}\" Version=\"{confluentApacheAvroAssemblyVersion}\" />";

            var hasPackageMatch = new Regex($"<PackageReference\\s.*?\"{confluentApacheAvroAssemblyShortName}\".*?/>", RegexOptions.Multiline).Match(projectSyntax);
            if (hasPackageMatch.Success && !hasPackageMatch.Value.Equals(avroPackageReference))
            {
                // Update package reference version
                projectSyntax = projectSyntax.Replace(hasPackageMatch.Value, avroPackageReference);
                UpdateProjectFile();
            }

            if (!hasPackageMatch.Success)
            {
                var hasPackageReferenceItemGroupMatch = new Regex("(<ItemGroup>)(?:(?!</ItemGroup>).)*<PackageReference", RegexOptions.Singleline).Match(projectSyntax);
                if (hasPackageReferenceItemGroupMatch.Success)
                {
                    // Add package reference with all other existing package references
                    var itemGroupMatch = hasPackageReferenceItemGroupMatch.Groups[1];
                    var startIndex = itemGroupMatch.Index + itemGroupMatch.Length;

                    projectSyntax = projectSyntax.Insert(startIndex, avroPackageReference);
                    UpdateProjectFile();
                }
            }

            // Add file tree settings for .Avro.cs and .cs files
            const string fileTreeSetting = "<Compile Update=\"**/*.Avro.cs\"><DependentUpon>$([System.String]::Copy('%(Filename).cs').Replace('Avro.',''))</DependentUpon></Compile>";
            
            var findPropertyGroupMatch = new Regex("</PropertyGroup>", RegexOptions.Multiline).Match(projectSyntax);
            if (!findPropertyGroupMatch.Success)
            {
                Log.LogError($"Project {compilation.AssemblyName} does not contain a <PropertyGroup> item.");
                return false;
            }

            var hasFileTreeItemGroupMatch = new Regex("(<ItemGroup>)(?:(?!</ItemGroup>).)*<Compile Update=\\\"\\*\\*/\\*.Avro.cs\\\">", RegexOptions.Singleline).Match(projectSyntax);
            if (!hasFileTreeItemGroupMatch.Success)
            {
                var propertyGroupEndIndex = findPropertyGroupMatch.Index + findPropertyGroupMatch.Length;

                var compileUpdateWithItemGroup = $"<ItemGroup>{fileTreeSetting}</ItemGroup>";
                projectSyntax = projectSyntax.Insert(propertyGroupEndIndex, compileUpdateWithItemGroup);
                UpdateProjectFile();
            }

            return true;

            static string ReadFile(string filePath)
            {
                string fileContents;
                using (var fileStream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using var streamReader = new StreamReader(fileStream);
                    fileContents = streamReader.ReadToEnd();
                }

                return fileContents;
            }

            void UpdateProjectFile()
            {
                // Format the xml document with proper indentation
                projectSyntax = projectSyntax.FormatXml();
                File.WriteAllText(ProjectPath, projectSyntax, Encoding.UTF8);   
            }
        }
    }
}