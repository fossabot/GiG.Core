using System.Collections.Generic;

namespace GiG.Core.DistributedTracing.Abstractions
{
    /// <summary>
    /// Tracing Options.
    /// </summary>
    public class TracingOptions
    {
        /// <summary>
        /// The configuration default section name.
        /// </summary>
        public const string DefaultSectionName = "Tracing";
        
        /// <summary>
        /// A value to indicates whether a Provider is enabled or not.
        /// </summary>
        public bool IsEnabled { get; set; }
        
        /// <summary>
        /// The Tracing providers.
        /// </summary>
        public IDictionary<string, BasicProviderOptions> Providers { get; set; } = new Dictionary<string, BasicProviderOptions>();
    }
}