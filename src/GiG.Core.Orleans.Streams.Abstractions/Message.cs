using System.Collections.Generic;

namespace GiG.Core.Orleans.Streams.Abstractions
{
    /// <summary>
    /// The base message of streams.
    /// </summary>
    public abstract class Message : IMessage
    {
        /// <inheritdoc />
        public string CorrelationId { get; set; }

        /// <inheritdoc />
        public IEnumerable<KeyValuePair<string, string>> Baggage { get; set; }

        /// <inheritdoc />
        public string TraceId { get; set; }

        /// <inheritdoc />
        public string SpanId { get; set; }

        /// <inheritdoc />
        public string ParentId { get; set; }

        /// <inheritdoc />
        public string ParentSpanId { get; set; }

        /// <inheritdoc />
        public string OperationName { get; set; }
    }
}