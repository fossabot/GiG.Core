using System;
using System.Runtime.Serialization;

namespace GiG.Core.TokenManager.Exceptions
{
    /// <summary>
    /// Represents errors that occur during Token Manager execution.
    /// </summary>
    public class TokenManagerException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TokenManagerException"></see> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"></see> that contains contextual information about the source or destination.</param>
        protected TokenManagerException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenManagerException"></see> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public TokenManagerException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenManagerException"></see> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="exception">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public TokenManagerException(string message, Exception exception) : base(message, exception)
        {
        }
    }
}