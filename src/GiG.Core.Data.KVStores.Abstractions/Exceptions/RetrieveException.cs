using System;

namespace GiG.Core.Data.KVStores.Abstractions.Exceptions
{
    /// <summary>
    /// Represents errors that occur during KV retrieve execution.
    /// </summary>
    public class RetrieveException : Exception
    {
        /// <inheritdoc />
        public RetrieveException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}