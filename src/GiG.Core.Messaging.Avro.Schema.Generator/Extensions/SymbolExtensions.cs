using GiG.Core.Messaging.Avro.Schema.Abstractions.Annotations;
using Microsoft.CodeAnalysis;
using System;
using System.Linq;

namespace GiG.Core.Messaging.Avro.Schema.Generator.Extensions
{
    internal static class SymbolExtensions
    {
        internal static FieldAttribute GetFieldAttribute(this IPropertySymbol property, WellKnownTypes wellKnownTypes)
        {
            if (property == null)
            {
                return null;
            }

            var attributes = property.GetAttributes();

            if (attributes.Length == 0)
            {
                return null;
            }

            var fieldAttribute = attributes.FirstOrDefault(a => a.AttributeClass.Equals(wellKnownTypes.FieldAttribute));

            var args = fieldAttribute.ConstructorArguments.Select(c => c.Value).ToArray();

            var field = (FieldAttribute)Activator.CreateInstance(typeof(FieldAttribute), args);

            return field;
        }

        internal static bool HasAttribute(this INamedTypeSymbol classSymbol, INamedTypeSymbol attributeSymbol)
        {
            if (classSymbol == null || attributeSymbol == null)
            {
                return false;
            }
            
            var classAttributes = classSymbol.GetAttributes();

            return classAttributes.Any(a => a.AttributeClass.Equals(attributeSymbol));
        }
    }
}