using GiG.Core.DistributedTracing.Abstractions;
using System;
using System.Collections.Generic;

namespace GiG.Core.DistributedTracing.Activity
{
    /// <inheritdoc />
    internal class ActivityContextAccessor : IActivityContextAccessor
    {
        private const string ActivityName = "GiG.Core.DistributedTracing.Activity";
       
        /// <inheritdoc />
        public string CorrelationId => Current.RootId ?? string.Empty;

        /// <inheritdoc />
        public IEnumerable<KeyValuePair<string, string>> Baggage => Current.Baggage ?? new List<KeyValuePair<string,string>>();

        /// <inheritdoc />
        public string TraceId => Current.TraceId.ToString() ?? string.Empty;

        /// <inheritdoc />
        public string SpanId => Current.SpanId.ToString() ?? string.Empty;

        /// <inheritdoc />
        public string ParentId => Current.ParentId;

        /// <inheritdoc />
        public string ParentSpanId => Current.ParentSpanId.ToString() ?? string.Empty;

        /// <inheritdoc />
        public string OperationName => Current.OperationName ?? ActivityName;
        
        private static System.Diagnostics.Activity Current
        {
            get
            {
                var activity = System.Diagnostics.Activity.Current;
                if (activity != null)
                {
                    return activity;
                }
                
                var newActivity = new System.Diagnostics.Activity(ActivityName);
                newActivity.Start();
                return newActivity;
            }
        }
    }
}