using GiG.Core.Messaging.Avro.Schema.Abstractions.Annotations;
using GiG.Core.Messaging.Avro.Schema.Generator.Exceptions;
using GiG.Core.Messaging.Avro.Schema.Generator.Extensions;
using GiG.Core.Messaging.Avro.Schema.Generator.Templates;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;


namespace GiG.Core.Messaging.Avro.Schema.Generator
{
    /// <summary>
    /// Generates the avro schema code files.
    /// </summary>
    public class CodeGenerator
    {
        private Stopwatch _stopwatch;
        
        private static StringBuilder _serializer;
        private static StringBuilder _deserializer;
        private static string _filepath;
        private static string _preExistingSchema;
        private static int _counter;

        private readonly WellKnownTypes _wellKnownTypes;
        private readonly Compilation _compilation;
        private readonly ILogger _log;

        private static List<string> _alreadyRegistered = new List<string>();

        private const string GeneratedFileSuffix = ".Avro.cs";
        private const string Indentation = "                ";

        #region Constructors
        /// <summary>
        /// Default Constructor. 
        /// </summary>
        /// <param name="compilation">The <see cref="Compilation"/>.</param>
        /// <param name="log">The <see cref="ILogger"/>.</param>
        public CodeGenerator(Compilation compilation, ILogger log)
        {
            _compilation = compilation;
            _log = log;
            _wellKnownTypes = WellKnownTypes.FromCompilation(compilation);
        }

        #endregion

        /// <summary>
        /// Generates the avro schema code files.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/>.</param>
        public void GenerateCode(CancellationToken cancellationToken)
        {
            _stopwatch = Stopwatch.StartNew();

            foreach (var syntaxTree in _compilation.SyntaxTrees)
            {
                var semanticModel = _compilation.GetSemanticModel(syntaxTree);
                var syntaxRoot = syntaxTree.GetRoot();

                // Iterate through class declarations
                foreach (var classNode in syntaxRoot.DescendantNodes().OfType<ClassDeclarationSyntax>())
                {
                    _counter = 0;
                    _serializer = new StringBuilder();
                    _deserializer = new StringBuilder();
                    _alreadyRegistered = new List<string>();

                    var classModel = semanticModel.GetDeclaredSymbol(classNode);

                    if (!CheckClassPreconditions(syntaxRoot, classModel, classNode))
                    {
                        continue;
                    }

                    var generatedSchema = GenerateSchema(semanticModel, classNode, classModel, false);
                    var cleanSchema = CleanSchema(generatedSchema);

                    if (_preExistingSchema == cleanSchema)
                    {
                        continue;
                    }

                    // Delete previously generated file
                    if (File.Exists(_filepath))
                    {
                        File.Delete(_filepath);
                    }

                    // Generate new Avro.cs file
                    var sourceCode = TemplateManager
                        .AvroClassTemplate
                        .Replace("{{namespace}}", classModel.ContainingNamespace.ToString())
                        .Replace("{{classname}}", classModel.Name)
                        .Replace("{{avro_schema}}", cleanSchema)
                        .Replace("{{avro_serializer}}", _serializer.ToString())
                        .Replace("{{avro_deserializer}}", _deserializer.ToString());

                    File.WriteAllText(_filepath, sourceCode.ToString(), Encoding.UTF8);

                    EnsureClassIsPartial(classNode, classModel, syntaxRoot);
                }
            }
        }

        private bool CheckClassPreconditions(SyntaxNode syntaxRoot, INamedTypeSymbol namedTypeSymbol, ClassDeclarationSyntax classDeclarationSyntax)
        {
            // Check whether the current class has the NamedSchemaAttribute
            if (!namedTypeSymbol.HasAttribute(_wellKnownTypes.NamedSchemaAttribute))
            {
                return false;
            }

            // Make sure that the class is public
            if (!classDeclarationSyntax.IsPublic())
            {
                throw new CodeGenerationException($"{namedTypeSymbol.Name} is not public.");
            }

            var classFilepath = new FileInfo(syntaxRoot.GetLocation().SourceTree.FilePath);

            // If file ends with ".Avro.cs", we read the "_SCHEMA" field and store the value to be compared with the new generation
            if (classFilepath.Name.EndsWith(GeneratedFileSuffix))
            {
                var fullPreExistingSchema = classDeclarationSyntax
                    .DescendantNodes()
                    .OfType<FieldDeclarationSyntax>()
                    .FirstOrDefault()
                    ?.ToFullString();

                var startOfSchema = fullPreExistingSchema?.IndexOf("{\"\"type\"\":\"\"record\"\"", StringComparison.CurrentCulture);
                var endOfSchema = fullPreExistingSchema?.LastIndexOf("\");", StringComparison.CurrentCulture);

                _preExistingSchema = fullPreExistingSchema
                    ?.Substring(startOfSchema.GetValueOrDefault(), endOfSchema.GetValueOrDefault() - startOfSchema.GetValueOrDefault());

                return false;
            }

            // Generate the Avro partial class filename
            var filename = Regex.Replace(classFilepath.Name, "(.*?)\\.(\\w{2})", $"$1{GeneratedFileSuffix}");
            _filepath = Path.Combine(classFilepath.DirectoryName ?? "", filename);

            return true;
        }

        #region Code Generation
        private dynamic GenerateSchema(SemanticModel semanticModel, SyntaxNode classNode, ISymbol classModel, bool isChildClass)
        {
            var schema = GetAvroRecordMetaData(classModel);
            schema["fields"] = new JArray();

            // Populate schema and serializers for all properties with the [Field] attribute
            foreach (var propertyDeclarationNode in classNode.DescendantNodes().OfType<PropertyDeclarationSyntax>())
            {
                var propertyModel = semanticModel.GetDeclaredSymbol(propertyDeclarationNode);
                var propertyAttributes = propertyModel.GetAttributes();

                if (!propertyAttributes.Any(a => a.AttributeClass.Equals(_wellKnownTypes.FieldAttribute)))
                {
                    continue;
                }

                var fieldAttribute = propertyModel.GetFieldAttribute(_wellKnownTypes);
                if (fieldAttribute == null)
                {
                    throw new CodeGenerationException($"FieldAttribute not set for property {propertyModel.Name}");
                }

                // Add the Avro field
                var field = GetAvroField(propertyModel.Type, fieldAttribute);
                schema["fields"].Add(field);

                if (isChildClass)
                {
                    var fullName = $"{classModel.ContainingNamespace}.{classModel.Name}";
                    if (!_alreadyRegistered.Contains(fullName))
                    {
                        _alreadyRegistered.Add(fullName);
                    }
                }
                else
                {
                    GenerateAvroSerializerCode(propertyModel.Type, propertyModel.Name);
                }
            }
            return schema;
        }

        private dynamic GenerateChildSchema(ISymbol type, bool isArray = false)
        {
            dynamic fields = new JArray();

            foreach (var syntaxTree in _compilation.SyntaxTrees)
            {
                var semanticModel = _compilation.GetSemanticModel(syntaxTree);
                var syntaxNode = syntaxTree.GetRoot();

                foreach (var classNode in syntaxNode.DescendantNodes().OfType<ClassDeclarationSyntax>())
                {
                    var classModel = semanticModel.GetDeclaredSymbol(classNode);
                    var classFilepath = new FileInfo(syntaxNode.GetLocation().SourceTree.FilePath);
                    if (!classModel.Equals(type) || classFilepath.Name.EndsWith(GeneratedFileSuffix))
                    {
                        continue;
                    }

                    if (!isArray)
                    {
                        fields.Add("null");
                    }

                    var fullName = $"{classModel.ContainingNamespace}.{classModel.Name}";
                    fields.Add(_alreadyRegistered.Contains(fullName)
                        ? fullName
                        : GenerateSchema(semanticModel, classNode, classModel, true));
                }
            }
            return fields;
        }

        private void GenerateAvroSerializerCode(ITypeSymbol typeSymbol, string typeName)
        {
            _serializer.Append($"{Indentation}case {_counter}: ");
            _deserializer.Append($"{Indentation}case {_counter}: ");

            // Handle property serialization and deserialization according to type
            if (typeSymbol.Equals(_wellKnownTypes.StringType))
            {
                _serializer.Append($"return {typeName};\r\n");
                _deserializer.Append($"{typeName} = ({typeSymbol})fieldValue; break;\r\n");
            }
            else if (ByteType(typeSymbol))
            {
                var isNullable = NullableType(typeSymbol);

                _serializer.Append($"return SerializeBytes(new[] {{{typeName}{(isNullable ? ".GetValueOrDefault()" : "")}}});\r\n");
                _deserializer.Append($"{typeName} = DeserializeBytes((byte[]) fieldValue)[0]; break;\r\n");
            }
            else if (DateTimeOffsetType(typeSymbol))
            {
                var isNullable = NullableType(typeSymbol);

                _serializer.Append($"return {typeName}{(isNullable ? ".GetValueOrDefault()" : "")}.ToString(\"O\");\r\n");
                _deserializer.Append($"{typeName} = DateTimeOffset.Parse(fieldValue.ToString()); break;\r\n");
            }
            else if (DateTimeType(typeSymbol))
            {
                var isNullable = NullableType(typeSymbol);

                _serializer.Append($"return {(isNullable ? $"{typeName} is null ? (long?) null : SerializeDateTime({typeName}.Value)" : $"SerializeDateTime({typeName})")};\r\n");
                _deserializer.Append($"{typeName} = DeserializeDateTime((long)fieldValue); break;\r\n");
            }
            else if (DecimalType(typeSymbol))
            {
                var isNullable = NullableType(typeSymbol);

                _serializer.Append($"return {(isNullable ? $"{typeName} is null ? null : SerializeDecimal({typeName}.Value)" : $"SerializeDecimal({typeName})")};\r\n");
                _deserializer.Append($"{typeName} = DeserializeDecimal(fieldValue as byte[]); break;\r\n");
            }
            else if (GuidType(typeSymbol))
            {
                var isNullable = NullableType(typeSymbol);

                _serializer.Append($"return {typeName}{(isNullable ? ".GetValueOrDefault()" : "")}.ToString();\r\n");
                _deserializer.Append($"{typeName} = Guid.Parse((string)fieldValue); break;\r\n");
            }
            else if (ShortType(typeSymbol))
            {
                var isNullable = NullableType(typeSymbol);

                _serializer.Append($"return {(isNullable ? $"(int?){typeName}" : $"(int){typeName}")};\r\n");
                _deserializer.Append($"{typeName} = ({typeSymbol})fieldValue; break;\r\n");
            }
            else if (TimeSpanType(typeSymbol))
            {
                var isNullable = NullableType(typeSymbol);

                _serializer.Append($"return {typeName}{(isNullable ? ".GetValueOrDefault()" : "")}.ToString(\"G\");\r\n");
                _deserializer.Append($"{typeName} = TimeSpan.Parse((string)fieldValue); break;\r\n");
            }
            else if (ValueType(typeSymbol))
            {
                _serializer.Append($"return {typeName};\r\n");
                _deserializer.Append($"{typeName} = ({typeSymbol})fieldValue; break;\r\n");
            }
            else if (typeSymbol.BaseType != null && typeSymbol.BaseType.Equals(_wellKnownTypes.EnumType))
            {
                _serializer.Append($"return SerializeEnum({typeName});\r\n");
                _deserializer.Append($"{typeName} = DeserializeEnum<{typeSymbol}>(fieldValue); break;\r\n");
            }
            else if (typeSymbol.Equals(_wellKnownTypes.ByteArrayType))
            {
                _serializer.Append($"return SerializeBytes({typeName});\r\n");
                _deserializer.Append($"{typeName} = DeserializeBytes((byte[])fieldValue); break;\r\n");
            }
            else if (_wellKnownTypes.GenericCollectionTypes.Any(t => t.Name.Equals(typeSymbol.Name)))
            {
                _serializer.Append($"return {typeName}?.ToArray();\r\n");

                var namedTypeSymbol = (typeSymbol as INamedTypeSymbol)?.TypeArguments[0];
                _deserializer.Append($"{typeName} = fieldValue is IList<object> ? ((List<object>)fieldValue).Cast<{namedTypeSymbol}>().ToList() : (List<{namedTypeSymbol}>)fieldValue; break;\r\n");
            }
            else if (typeSymbol.TypeKind.Equals(TypeKind.Array))
            {
                _serializer.Append($"return {typeName};\r\n");

                var arrayTypeSymbol = (typeSymbol as IArrayTypeSymbol)?.ElementType;
                _deserializer.Append($"{typeName} = fieldValue is IList<object> ? ((List<object>)fieldValue).Cast<{arrayTypeSymbol}>().ToArray() : ((List<{arrayTypeSymbol}>)fieldValue).ToArray(); break;\r\n");
            }
            else if (_wellKnownTypes.LocalTypes.Contains(typeSymbol))
            {
                _serializer.Append($"return {typeName};\r\n");
                _deserializer.Append($"{typeName} = ({typeSymbol})fieldValue; break;\r\n");
            }
            else
            {
                throw new Exception($"Unsupported type {typeSymbol}");
            }

            _counter++;
        }

        private dynamic GetAvroField(ITypeSymbol type, FieldAttribute fieldAttribute, bool isNullable = false, bool isArray = false)
        {
            dynamic field = new JObject();

            if (type.Equals(_wellKnownTypes.StringType) || type.Equals(_wellKnownTypes.GuidType) || type.Equals(_wellKnownTypes.TimeSpanType))
            {
                field = Convert.String(fieldAttribute.Name);
            }
            else if (type.Equals(_wellKnownTypes.ByteType))
            {
                field = Convert.ByteArray(fieldAttribute.Name);
            }
            else if (type.Equals(_wellKnownTypes.DateTimeType))
            {
                field = Convert.DateTime(fieldAttribute.Name, isNullable);
            }
            else if (type.Equals(_wellKnownTypes.DateTimeOffsetType))
            {
                field = Convert.String(fieldAttribute.Name);
            }
            else if (type.Equals(_wellKnownTypes.BooleanType))
            {
                field = Convert.Boolean(fieldAttribute.Name, isNullable);
            }
            else if (type.Equals(_wellKnownTypes.Int16Type) || type.Equals(_wellKnownTypes.Int32Type))
            {
                field = Convert.Int(fieldAttribute.Name, isNullable);
            }
            else if (type.Equals(_wellKnownTypes.Int64Type))
            {
                field = Convert.Long(fieldAttribute.Name, isNullable);
            }
            else if (type.Equals(_wellKnownTypes.DecimalType))
            {
                field = Convert.Decimal(fieldAttribute.Name, isNullable);
            }
            else if (type.Equals(_wellKnownTypes.DoubleType))
            {
                field = Convert.Double(fieldAttribute.Name, isNullable);
            }
            else if (type.Equals(_wellKnownTypes.FloatType))
            {
                field = Convert.Float(fieldAttribute.Name, isNullable);
            }
            else if (type.Equals(_wellKnownTypes.ByteArrayType))
            {
                field = Convert.ByteArray(fieldAttribute.Name);
            }
            else if (type.OriginalDefinition.Equals(_wellKnownTypes.NullableType))
            {
                var innerType = (type as INamedTypeSymbol)?.TypeArguments[0];
                field = GetAvroField(innerType, fieldAttribute, true);
            }
            else if (type.TypeKind.Equals(TypeKind.Array) || _wellKnownTypes.GenericCollectionTypes.Any(t => t.Name.Equals(type.Name)))
            {
                field.name = fieldAttribute.Name;

                var arrayType = new JObject
                {
                    ["type"] = "array"
                };

                var innerType = type.TypeKind.Equals(TypeKind.Array)
                    ? ((IArrayTypeSymbol)type).ElementType
                    : (type as INamedTypeSymbol)?.TypeArguments[0];

                arrayType["items"] = _alreadyRegistered.Contains($"{innerType?.ContainingNamespace}.{innerType?.Name}")
                    ? innerType?.Name
                    : GetAvroField(innerType, fieldAttribute, isArray: true).type;

                field.type = arrayType;
            }
            else if (type.BaseType != null && type.BaseType.Equals(_wellKnownTypes.EnumType))
            {
                field = Convert.Enum(fieldAttribute.Name, type, fieldAttribute.DefaultValue);
            }
            else if (_wellKnownTypes.LocalTypes.Contains(type))
            {
                field.name = fieldAttribute.Name;
                field.type = GenerateChildSchema(type, isArray);
            }
            else
            {
                throw new CodeGenerationException($"Unsupported type {type}");
            }

            if (!string.IsNullOrWhiteSpace(fieldAttribute.Documentation))
            {
                field.doc = fieldAttribute.Documentation;
            }

            return field;
        }

        private dynamic GetAvroRecordMetaData(ISymbol classModel)
        {
            dynamic schema = new JObject();

            schema["type"] = "record";
            schema["name"] = classModel.Name;
            schema["namespace"] = classModel.ContainingNamespace.ToString();

            var namedSchemaAttribute = classModel.GetAttributes().FirstOrDefault(a => a.AttributeClass.Equals(_wellKnownTypes.NamedSchemaAttribute));

            var documentation = (string)namedSchemaAttribute.ConstructorArguments[0].Value;

            if (!string.IsNullOrWhiteSpace(documentation))
            {
                schema["doc"] = documentation;
            }

            return schema;
        }
        #endregion

        #region Type Resolvers
        private bool ByteType(ITypeSymbol type) => type.Equals(_wellKnownTypes.ByteType) || NullableType(type, _wellKnownTypes.ByteType);

        private bool DateTimeOffsetType(ITypeSymbol type) => type.Equals(_wellKnownTypes.DateTimeOffsetType) || NullableType(type, _wellKnownTypes.DateTimeOffsetType);

        private bool DateTimeType(ITypeSymbol type) => type.Equals(_wellKnownTypes.DateTimeType) || NullableType(type, _wellKnownTypes.DateTimeType);

        private bool DecimalType(ITypeSymbol type) => type.Equals(_wellKnownTypes.DecimalType) || NullableType(type, _wellKnownTypes.DecimalType);

        private bool GuidType(ITypeSymbol type) => type.Equals(_wellKnownTypes.GuidType) || NullableType(type, _wellKnownTypes.GuidType);

        private bool NullableType(ITypeSymbol typeSymbol, INamedTypeSymbol namedTypeSymbol = null)
        {
            var result = typeSymbol.OriginalDefinition.Equals(_wellKnownTypes.NullableType);

            if (result && namedTypeSymbol != null)
            {
                var namedSymbol = typeSymbol as INamedTypeSymbol;
                result = namedSymbol?.TypeArguments.Length > 0 && namedSymbol.TypeArguments[0].Equals(namedTypeSymbol);
            }

            return result;
        }

        private bool ShortType(ITypeSymbol type) => type.Equals(_wellKnownTypes.Int16Type) || NullableType(type, _wellKnownTypes.Int16Type);

        private bool TimeSpanType(ITypeSymbol type) => type.Equals(_wellKnownTypes.TimeSpanType) || NullableType(type, _wellKnownTypes.TimeSpanType);

        private bool ValueType(ITypeSymbol type) =>
            type.Equals(_wellKnownTypes.BooleanType) || NullableType(type, _wellKnownTypes.BooleanType) ||
            type.Equals(_wellKnownTypes.Int32Type) || NullableType(type, _wellKnownTypes.Int32Type) ||
            type.Equals(_wellKnownTypes.Int64Type) || NullableType(type, _wellKnownTypes.Int64Type) ||
            type.Equals(_wellKnownTypes.DoubleType) || NullableType(type, _wellKnownTypes.DoubleType) ||
            type.Equals(_wellKnownTypes.FloatType) || NullableType(type, _wellKnownTypes.FloatType);

        #endregion

        private static string CleanSchema(dynamic schema)
        {
            var json = schema.ToString();
            json = new Regex("(\"(?:[^\"\\\\]|\\\\.)*\")|\\s+").Replace(json, "$1");
            json = new Regex("\"").Replace(json, "\"\"");

            return json;
        }

        private static void EnsureClassIsPartial(ClassDeclarationSyntax classNode, ISymbol classModel, SyntaxNode syntaxRoot)
        {
            if (classNode == null || classNode.IsPartial() || classModel == null || syntaxRoot == null)
            {
                return;
            }

            var location = syntaxRoot.GetLocation();
            var filepath = location.SourceTree.FilePath;

            if (!File.Exists(filepath))
            {
                throw new FileNotFoundException("The class file was not found.", filepath);
            }

            var sourceCode = ReadFile(filepath);

            var regex = new Regex($"(class)\\s({classModel.Name})", RegexOptions.Multiline);

            sourceCode = regex.Replace(sourceCode, "partial $1 $2");

            File.WriteAllText(filepath, sourceCode);
        }

        private void LogInfo(string message)
        {
            _log.LogInformation($"{message} completed in {_stopwatch.ElapsedMilliseconds}ms.");
            _stopwatch.Restart();
        }

        private static string ReadFile(string filePath)
        {
            string fileContents;
            using (var fileStream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (var streamReader = new StreamReader(fileStream))
                {
                    fileContents = streamReader.ReadToEnd();
                }
            }

            return fileContents;
        }
    }
}