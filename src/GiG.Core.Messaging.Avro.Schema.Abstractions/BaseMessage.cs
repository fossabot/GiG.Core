using GiG.Core.Messaging.Avro.Schema.Abstractions.Annotations;
using System;

namespace GiG.Core.Messaging.Avro.Schema.Abstractions
{
    // TODO To verify if this calls is required
    /// <summary>
    /// Base Message.
    /// </summary>
    public abstract class BaseMessage
    {
        /// <summary>
        /// Transaction Id.
        /// </summary>
        [Field(nameof(TransactionId))]
        public Guid TransactionId { get; set; }

        /// <summary>
        /// Timestamp.
        /// </summary>
        [Field(nameof(Timestamp))]
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// CorrelationId.
        /// </summary>
        [Field(nameof(CorrelationId))]
        public Guid CorrelationId { get; set; }
    }
}
