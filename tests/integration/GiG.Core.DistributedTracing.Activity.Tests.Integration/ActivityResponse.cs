namespace GiG.Core.DistributedTracing.Activity.Tests.Integration
{
    public class ActivityResponse
    {
        /// <summary>
        /// The current context's Correlation ID.
        /// </summary>
        public string CorrelationId { get; set; }

        /// <summary>
        /// The current Activity's Trace Id.
        /// </summary>
        public string TraceId { get; set; }

        /// <summary>
        /// The current Activity's Span Id.
        /// </summary>
        public string SpanId { get; set; }

        /// <summary>
        /// The name of the Operation.
        /// </summary>
        public string OperationName { get; set; }
    }
}
