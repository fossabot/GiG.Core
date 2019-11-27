using System;

namespace GiG.Core.Messaging.Kafka.Abstractions.Exceptions
{ 
    [Serializable]
    public class MessageHandlerUnassignedException : Exception
    {
        public MessageHandlerUnassignedException() : base(KafkaConstants.MessageHandlerUnassignedExceptionMessage) { }
        public MessageHandlerUnassignedException(string message) : base(message) { }
    }
}