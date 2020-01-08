﻿using System;

 namespace GiG.Core.Messaging.Avro.Schema.Abstractions.Annotations
{
    /// <summary>
    /// Attributes being used within classes to set schema description.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum)]
    public class NamedSchemaAttribute : Attribute
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="doc">Description of schema.</param>
        public NamedSchemaAttribute(string doc = null)
        {
            Documentation = doc;
        }

        /// <summary>
        /// Description of schema.
        /// </summary>
        public string Documentation { get; }
    }
}