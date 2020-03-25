using System.Collections.Generic;
using System.Diagnostics;

namespace GiG.Core.DistributedTracing.Abstractions
{
    /// <summary>
    /// Activity Context Accessor.
    /// </summary>
    public interface IActivityContextAccessor
    {
        /// <summary>
        /// The current Activty's Id.
        /// </summary>
        string ActivityId { get; }

        /// <summary>
        /// The current context's Correlation ID.
        /// </summary>
        string CorrelationId { get; }
        
        /// <summary>
        /// The Activity Baggage.
        /// string-string key-value pairs that represent information that will
        /// be passed along to children of this activity.
        /// </summary>
        IEnumerable<KeyValuePair<string, string>> Baggage { get; }
        
        /// <summary>
        /// The current Activity's Trace Id.
        /// </summary>
        string TraceId { get; }
        
        /// <summary>
        /// The current Activity's Span Id.
        /// </summary>
        string SpanId { get; }

        /// <summary>
        /// The current Activity's Parent Id.
        /// </summary>
        string ParentId { get; }

        /// <summary>
        /// The current Activity's Parent Span Id.
        /// </summary>
        string ParentSpanId { get; }

        /// <summary>
        /// The name of the Operation.
        /// </summary>
        string OperationName { get; }

        /// <summary>
        /// Gets the current <see cref="System.Diagnostics.Activity"/>.
        /// </summary>
        Activity CurrentActivity { get; }
    }
}