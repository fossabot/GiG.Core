using GiG.Core.DistributedTracing.Abstractions;
using System;
using System.Collections.Generic;

namespace GiG.Core.DistributedTracing.Activity
{
    /// <inheritdoc />
    internal class ActivityContextAccessor : IActivityContextAccessor
    {
        /// <inheritdoc />
        string IActivityContextAccessor.CorrelationId => GetCorrelationId();

        /// <inheritdoc />
        public IEnumerable<KeyValuePair<string, string>> Baggage => System.Diagnostics.Activity.Current?.Baggage ?? new List<KeyValuePair<string,string>>();

        /// <inheritdoc />
        public string TraceId => System.Diagnostics.Activity.Current?.TraceId.ToString() ?? string.Empty;

        /// <inheritdoc />
        public string SpanId => System.Diagnostics.Activity.Current?.SpanId.ToString() ?? string.Empty;

        /// <inheritdoc />
        public string ParentId => System.Diagnostics.Activity.Current?.ParentId?.ToString() ?? string.Empty;

        /// <inheritdoc />
        public string ParentSpanId => System.Diagnostics.Activity.Current?.ParentSpanId.ToString() ?? string.Empty;

        /// <inheritdoc />
        public string OperationName => System.Diagnostics.Activity.Current?.OperationName ?? string.Empty;
        
        private string GetCorrelationId()
        {
            if (System.Diagnostics.Activity.Current != null)
            {
                var activity = new System.Diagnostics.Activity(OperationName);
                activity.Start();
            }

            return System.Diagnostics.Activity.Current?.RootId ?? string.Empty;
        }
    }
}