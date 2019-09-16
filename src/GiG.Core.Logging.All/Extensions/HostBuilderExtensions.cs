using GiG.Core.Logging.Abstractions;
using GiG.Core.Logging.Enrichers.ApplicationMetadata.Extensions;
using GiG.Core.Logging.Enrichers.DistributedTracing.Extensions;
using GiG.Core.Logging.Extensions;
using GiG.Core.Logging.Sinks.Console.Extensions;
using GiG.Core.Logging.Sinks.Fluentd.Extensions;
using JetBrains.Annotations;
using Microsoft.Extensions.Hosting;

namespace GiG.Core.Logging.All.Extensions
{
    /// <summary>
    /// Host Builder extensions.
    /// </summary>
    public static class HostBuilderExtensions
    {
        /// <summary>
        /// Configures the default logging sinks (Console and Fluentd) and enrichers (Application metadata and Correlation ID).
        /// </summary>
        /// <param name="builder">Host builder.</param>
        /// <param name="sectionName">Configuration section name.</param>
        /// <returns>Host builder.</returns>
        public static IHostBuilder ConfigureLogging([NotNull] this IHostBuilder builder,
            [NotNull] string sectionName = LoggingOptions.DefaultSectionName) =>
            builder.ConfigureLogging(x => x
                .WriteToConsole()
                .WriteToFluentd()
                .EnrichWithApplicationMetadata()
                .EnrichWithCorrelationId(), sectionName);
    }
}