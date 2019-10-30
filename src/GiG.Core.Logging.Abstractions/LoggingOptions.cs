using System.Collections.Generic;

namespace GiG.Core.Logging.Abstractions
{
    /// <summary>
    /// Logging Options.
    /// </summary>
    public class LoggingOptions
    {
        /// <summary>
        /// The configuration default section name.
        /// </summary>
        public const string DefaultSectionName = "Logging";
        
        /// <summary>
        /// The Sink providers.
        /// </summary>
        public IDictionary<string, BasicSinkOptions> Sinks { get; set; } = new Dictionary<string, BasicSinkOptions>();
    }
}