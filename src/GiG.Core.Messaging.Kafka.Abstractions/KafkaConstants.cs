using GiG.Core.DistributedTracing.Abstractions;

namespace GiG.Core.Messaging.Kafka.Abstractions
{
    internal class KafkaConstants
    {
        public const string MessageTypeHeaderName = "MessageType";
        public const string MessageIdHeaderName = "MessageId";
        public const string CorrelationIdHeaderName = Constants.Header;
        public const string SchemaNameHeaderName = "SchemaName";

        public const string MessageHandlerUnassignedExceptionMessage = "A message handler needs to be assigned to process incoming messages.";
    }
}