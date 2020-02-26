using GiG.Core.Logging.Abstractions;

namespace GiG.Core.Logging.Sinks.Console.Internal
{
    /// <summary>
    /// Console Sink Configuration Options.
    /// </summary>
    public class ConsoleSinkOptions : BasicSinkOptions
    {
        /// <summary>
        /// Enables structured logging using ElasticSearch formatter. 
        /// </summary>
        public bool UseStructuredLogging { get; set; }
    }
}