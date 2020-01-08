using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace GiG.Core.Messaging.Avro.Schema.Generator
{
    [SuppressMessage("ReSharper", "InconsistentNaming", Justification = "These property names reflect type names.")]
    internal class WellKnownTypes
    {
        public static WellKnownTypes FromCompilation(Compilation compilation)
        {
            var localTypes = new Collection<INamedTypeSymbol>();
            foreach (var m in compilation.Assembly.GlobalNamespace.GetMembers())
            {
                ProcessTypeOrNamespace(localTypes, m);
            }

            return new WellKnownTypes
            {
                ArrayType = compilation.GetSpecialType(SpecialType.System_Array),
                BooleanType = compilation.GetSpecialType(SpecialType.System_Boolean),
                ByteArrayType = ResolveArrayType(compilation.GetSpecialType(SpecialType.System_Byte)),
                ByteType = compilation.GetSpecialType(SpecialType.System_Byte),
                DateTimeType = compilation.GetSpecialType(SpecialType.System_DateTime),
                DecimalType = compilation.GetSpecialType(SpecialType.System_Decimal),
                DoubleType = compilation.GetSpecialType(SpecialType.System_Double),
                EnumType = compilation.GetSpecialType(SpecialType.System_Enum),
                FieldAttribute = Type("GiG.Core.Messaging.Avro.Schema.Abstractions.Annotations.FieldAttribute"),
                FloatType = compilation.GetSpecialType(SpecialType.System_Single),
                GenericCollectionTypes = new Collection<INamedTypeSymbol>
                {
                    compilation.GetSpecialType(SpecialType.System_Collections_Generic_IEnumerable_T), 
                    compilation.GetSpecialType(SpecialType.System_Collections_Generic_IList_T), 
                    compilation.GetSpecialType(SpecialType.System_Collections_Generic_IReadOnlyList_T), 
                    compilation.GetSpecialType(SpecialType.System_Collections_Generic_ICollection_T)
                },
                GuidType = Type("System.Guid"),
                TimeSpanType = Type("System.TimeSpan"),
                DateTimeOffsetType = Type("System.DateTimeOffset"),
                Int16Type = compilation.GetSpecialType(SpecialType.System_Int16),
                Int32Type = compilation.GetSpecialType(SpecialType.System_Int32),
                Int64Type = compilation.GetSpecialType(SpecialType.System_Int64),
                LocalTypes = localTypes,
                NamedSchemaAttribute = Type("GiG.Core.Messaging.Avro.Schema.Abstractions.Annotations.NamedSchemaAttribute"),
                NullableType = compilation.GetSpecialType(SpecialType.System_Nullable_T),
                StringType = compilation.GetSpecialType(SpecialType.System_String)
            };

            void ProcessTypeOrNamespace(ICollection<INamedTypeSymbol> types, INamespaceOrTypeSymbol m)
            {
                if (m.IsType)
                {
                    var type = (INamedTypeSymbol)m;
                    if (type.IsValueType)
                        return;

                    types.Add(type);
                }
                else
                {
                    foreach (var n in m.GetMembers().OfType<INamespaceOrTypeSymbol>())
                        ProcessTypeOrNamespace(types, n);
                }
            }

            INamedTypeSymbol Type(string type)
            {
                var result = ResolveType(type);
                if (result == null)
                {
                    throw new InvalidOperationException($"Unable to find type with metadata name \"{type}\".");
                }
                return result;
            }

            IArrayTypeSymbol ResolveArrayType(ITypeSymbol type) => compilation.CreateArrayTypeSymbol(type);

            INamedTypeSymbol ResolveType(string type)
            {
                var result = compilation.GetTypeByMetadataName(type);

                if (result != null)
                {
                    return result;
                }
                
                foreach (var reference in compilation.References)
                {
                    if (!(compilation.GetAssemblyOrModuleSymbol(reference) is IAssemblySymbol asm)) continue;

                    result = asm.GetTypeByMetadataName(type);
                    if (result != null) break;
                }

                return result;
            }
        }

        public INamedTypeSymbol ArrayType { get; private set; }
        public INamedTypeSymbol BooleanType { get; private set; }
        public IArrayTypeSymbol ByteArrayType { get; private set; }
        public INamedTypeSymbol ByteType { get; private set; }
        public INamedTypeSymbol DateTimeOffsetType { get; private set; }
        public INamedTypeSymbol DateTimeType { get; private set; }
        public INamedTypeSymbol DecimalType { get; private set; }
        public INamedTypeSymbol DoubleType { get; private set; }
        public INamedTypeSymbol EnumType { get; private set; }
        public INamedTypeSymbol FieldAttribute { get; private set; }
        public INamedTypeSymbol FloatType { get; private set; }
        public ICollection<INamedTypeSymbol> GenericCollectionTypes { get; private set; }
        public INamedTypeSymbol GuidType { get; private set; }
        public INamedTypeSymbol Int16Type { get; private set; }
        public INamedTypeSymbol Int32Type { get; private set; }
        public INamedTypeSymbol Int64Type { get; private set; }
        public ICollection<INamedTypeSymbol> LocalTypes { get; private set; }
        public INamedTypeSymbol NamedSchemaAttribute { get; private set; }
        public INamedTypeSymbol NullableType { get; private set; }
        public INamedTypeSymbol StringType { get; private set; }
        public INamedTypeSymbol TimeSpanType { get; private set; }
    }
}