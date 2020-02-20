using System.Collections.Generic;

namespace GiG.Core.Orleans.Streams.Abstractions
{
    /// <summary>
    /// Interface for Stream Message.
    /// </summary>
    public interface IMessage
    {
        /// <summary>
        /// The current context's Correlation ID.
        /// </summary>
        string CorrelationId { get; set; }
        
        /// <summary>
        /// The Activity Baggage.
        /// string-string key-value pairs that represent information that will
        /// be passed along to children of this activity.
        /// </summary>
        IEnumerable<KeyValuePair<string, string>> Baggage { get; set; }
        
        /// <summary>
        /// The current Activity's Trace Id.
        /// </summary>
        string TraceId { get; set; }
        
        /// <summary>
        /// The current Activity's Span Id.
        /// </summary>
        string SpanId { get; set; }

        /// <summary>
        /// The current Activity's Parent Id.
        /// </summary>
        string ParentId { get; set; }

        /// <summary>
        /// The current Activity's Parent Span Id.
        /// </summary>
        string ParentSpanId { get; set; }

        /// <summary>
        /// The name of the Operation.
        /// </summary>
        string OperationName { get; set; }
    }
}