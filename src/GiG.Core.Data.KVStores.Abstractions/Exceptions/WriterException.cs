using System;

namespace GiG.Core.Data.KVStores.Abstractions.Exceptions
{
    /// <summary>
    /// Represents errors that occur during KV write execution.
    /// </summary>
    public class WriterException : Exception
    {
        /// <inheritdoc />
        public WriterException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}