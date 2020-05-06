namespace GiG.Core.Messaging.Kafka.Abstractions
{
    /// <summary>
    /// Constants.
    /// </summary>
    public class Constants
    {
        /// <summary>
        /// Header name for the Message type.
        /// </summary>
        public const string MessageTypeHeaderName = "message_type";
        
        /// <summary>
        /// Header name for the Message ID.
        /// </summary>
        public const string MessageIdHeaderName = "message_id";
        
        /// <summary>
        /// Header name for the Message ID.
        /// </summary>
        public const string CorrelationIdHeaderName = "correlation_id";
    }
}