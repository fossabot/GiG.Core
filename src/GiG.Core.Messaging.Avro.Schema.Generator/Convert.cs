using Microsoft.CodeAnalysis;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace GiG.Core.Messaging.Avro.Schema.Generator
{
    internal static class Convert
    {
        internal static dynamic Boolean(string fieldName, bool nullable)
        {
            dynamic field = new JObject();
            field.name = fieldName;

            if (nullable)
            {
                field.type = new JArray { "null", "boolean" };
                field["default"] = null;
            }
            else
            {
                field.type = "boolean";
            }

            return field;
        }

        internal static dynamic ByteArray(string fieldName)
        {
            dynamic field = new JObject();
            field.name = fieldName;
            field.type = "bytes";

            return field;
        }

        internal static dynamic DateTime(string fieldName, bool nullable)
        {
            dynamic field = new JObject();
            field.name = fieldName;

            if (nullable)
            {
                dynamic logicalType = new JObject();
                logicalType.type = "long";
                logicalType.logicalType = "timestamp-millis";

                field.type = new JArray { "null", logicalType };
                field["default"] = null;
            }
            else
            {
                field.type = "long";
                field.logicalType = "timestamp-millis";
            }

            return field;
        }

        internal static dynamic Decimal(string fieldName, bool nullable)
        {
            dynamic field = new JObject();
            field.name = fieldName;

            if (nullable)
            {
                dynamic logicalType = new JObject();
                logicalType.type = "bytes";
                logicalType.logicalType = "decimal";
                logicalType.precision = 29;
                logicalType.scale = 10;

                field.type = new JArray { "null", logicalType };
                field["default"] = null;
            }
            else
            {
                field.type = "bytes";
                field.logicalType = "decimal";
                field.precision = 29;
                field.scale = 10;
            }

            return field;
        }

        internal static dynamic Double(string fieldName, bool nullable)
        {
            dynamic field = new JObject();
            field.name = fieldName;
            
            if (nullable)
            {
                field.type = new JArray { "null", "double" };
                field["default"] = null;
            }
            else
            {
                field.type = "double";
            }

            return field;
        }

        internal static dynamic Enum(string fieldName, ITypeSymbol type, object defaultValue = null)
        {
            dynamic field = new JObject();
            field.name = fieldName;

            var enumMembersArray = new JArray();

            // Populate the members array
            type
                .GetMembers()
                .OfType<IFieldSymbol>()
                .Select(m => m.Name)
                .ToList()
                .ForEach(m => enumMembersArray.Add(m.ToUpperInvariant()));

            var enumTypeField = new JObject
            {
                ["name"] = type.Name,
                ["type"] = "enum",
                ["symbols"] = enumMembersArray
            };

            field.type = enumTypeField;

            if (defaultValue != null)
            {
                field["default"] = enumMembersArray[defaultValue];
            }

            return field;
        }

        internal static dynamic Float(string fieldName, bool nullable)
        {
            dynamic field = new JObject();
            field.name = fieldName;
            
            if (nullable)
            {
                field.type = new JArray { "null", "float" };
                field["default"] = null;
            }
            else
            {
                field.type = "float";
            }

            return field;
        }

        internal static dynamic Guid(string fieldName)
        {
            dynamic field = new JObject();
            field.name = fieldName;
            field.logicalType = "uuid";  // available from Avro specification v1.9.0

            return field;
        }

        internal static dynamic Int(string fieldName, bool nullable)
        {
            dynamic field = new JObject();
            field.name = fieldName;

            if (nullable)
            {
                field.type = new JArray { "null", "int" };
                field["default"] = null;
            }
            else
            {
                field.type = "int";
            }

            return field;
        }

        internal static dynamic Long(string fieldName, bool nullable)
        {
            dynamic field = new JObject();
            field.name = fieldName;

            if (nullable)
            {
                field.type = new JArray { "null", "long" };
                field["default"] = null;
            }
            else
            {
                field.type = "long";
            }

            return field;
        }

        internal static dynamic String(string fieldName)
        {
            dynamic field = new JObject();
            field.name = fieldName;
            field.type = new JArray { "null", "string" };
            field["default"] = null;

            return field;
        }
    }
}
