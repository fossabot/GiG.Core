namespace GiG.Core.Logging.Enrichers.DistributedTracing
{
    /// <summary>
    /// Distributed tracing fields from current Activity.
    /// </summary>
    public static class TracingFields
    {
        /// <summary>
        /// The CorrelationId.
        /// </summary>
        public const string CorrelationId = "CorrelationId";

        /// <summary>
        /// The TraceId part of the Id.
        /// </summary>
        public const string TraceId = "TraceId";
        
        /// <summary>
        /// The SPAN part of the Id.
        /// </summary>
        public const string SpanId = "SpanId";
        
        /// <summary>
        /// The ID of this activity's parent.
        /// </summary>
        public const string ParentId = "ParentId";
        
        /// <summary>
        /// Prefix for Baggage items.
        /// </summary>
        public const string BaggagePrefix = "baggage.";    
    }
}