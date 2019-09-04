using System.Collections.Generic;

namespace GiG.Core.Logging.Abstractions
{
    /// <summary>
    /// Logger Options.
    /// </summary>
    public class LoggerOptions
    {
        /// <summary>
        /// Logger default section name.
        /// </summary>
        public const string DefaultSectionName = "Logging";
        
        /// <summary>
        /// Sink providers for logging.
        /// </summary>
        public IDictionary<string, BasicSinkOptions> Sinks { get; set; } = new Dictionary<string, BasicSinkOptions>();
    }
}