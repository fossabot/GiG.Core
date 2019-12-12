using System;

namespace GiG.Core.Messaging.Avro.Schema.Abstractions.Annotations
{
    /// <summary>
    /// Attributes being used within classes to mark fields in the message to be serialized.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class FieldAttribute : Attribute
    {
        /// <summary>
        /// Default Constructor.
        /// </summary>
        /// <param name="name">Name of field.</param>
        /// <param name="doc">Documentation for the field.</param>
        /// <param name="defaultValue">Default value of the field.</param>
        public FieldAttribute(string name = null, string doc = null, object defaultValue = null)
        {
            Name = name;
            Documentation = doc;
            DefaultValue = defaultValue;
        }

        /// <summary>
        /// Name of field.
        /// </summary>
        public string Name { get; }
        
        /// <summary>
        /// Documentation for the field.
        /// </summary>
        public string Documentation { get; }
        
        /// <summary>
        /// Default value of the field.
        /// </summary>
        public object DefaultValue { get; }
    }
}