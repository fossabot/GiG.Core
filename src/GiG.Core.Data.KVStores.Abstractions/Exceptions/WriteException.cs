using System;

namespace GiG.Core.Data.KVStores.Abstractions.Exceptions
{
    /// <summary>
    /// Represents errors that occur during KV write execution.
    /// </summary>
    public class WriteException : Exception
    {
        /// <inheritdoc />
        public WriteException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}