using System;

namespace GiG.Core.Messaging.Avro.Schema.Generator.Exceptions
{
    /// <summary>
    /// Represents errors that occur during Code Generation execution.
    /// </summary>
    [Serializable]
    public class CodeGenerationException : Exception
    {
        /// <summary>
        /// Default Constructor.
        /// </summary>
        /// <param name="message"></param>
        public CodeGenerationException(string message) : base(message) { }
    }
}