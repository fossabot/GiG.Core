using System;

namespace GiG.Core.Messaging.Kafka.Abstractions.Exceptions
{
    /// <summary>
    /// Represents errors that occur during message handling.
    /// </summary>
    [Serializable]
    public class MessageHandlerUnassignedException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageHandlerUnassignedException"></see> class.
        /// </summary>
        public MessageHandlerUnassignedException() : base(KafkaConstants.MessageHandlerUnassignedExceptionMessage) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageHandlerUnassignedException"></see> class with a specified error message.
        /// </summary>
        /// <param name="message"></param>
        public MessageHandlerUnassignedException(string message) : base(message) { }
    }
}