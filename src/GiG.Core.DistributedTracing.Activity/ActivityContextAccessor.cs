using GiG.Core.DistributedTracing.Abstractions;
using System.Collections.Generic;

namespace GiG.Core.DistributedTracing.Activity
{
    /// <inheritdoc />
    internal class ActivityContextAccessor : IActivityContextAccessor
    {
        /// <inheritdoc />
        public string CorrelationId => System.Diagnostics.Activity.Current.RootId;

        /// <inheritdoc />
        public IEnumerable<KeyValuePair<string, string>> Baggage => System.Diagnostics.Activity.Current.Baggage;

        /// <inheritdoc />
        public string TraceId => System.Diagnostics.Activity.Current.TraceId.ToString();

        /// <inheritdoc />
        public string SpanId => System.Diagnostics.Activity.Current.SpanId.ToString();

        /// <inheritdoc />
        public string ParentSpanId => System.Diagnostics.Activity.Current.ParentSpanId.ToString();

        /// <inheritdoc />
        public string OperationName => System.Diagnostics.Activity.Current.OperationName;
    }
}