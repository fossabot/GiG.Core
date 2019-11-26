using System;
using System.Runtime.Serialization;

namespace GiG.Core.TokenManager.Exceptions
{
    public class TokenManagerException : Exception
    {
        protected TokenManagerException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public TokenManagerException(string message) : base(message)
        {
        }

        public TokenManagerException(string message, Exception exception) : base(message, exception)
        {
        }
    }
}