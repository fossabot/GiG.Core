using GiG.Core.Extensions.Logging.Sinks.Console;
using GiG.Core.Extensions.Logging.Sinks.Fluentd;
using GiG.Core.Logging.Abstractions;
using GiG.Core.Logging.Enrichers.ApplicationMetadata;
using GiG.Core.Logging.Enrichers.DistributedTracing;
using JetBrains.Annotations;
using Microsoft.Extensions.Hosting;

namespace GiG.Core.Extensions.Logging.All
{
    /// <summary>
    /// Host Builder extensions.
    /// </summary>
    public static class HostBuilderExtensions
    {
        /// <summary>
        /// Configures the default logging sinks and enrichers.
        /// </summary>
        /// <param name="builder">Host builder.</param>
        /// <param name="sectionName">Configuration section name.</param>
        /// <returns>Host builder.</returns>
        public static IHostBuilder ConfigureLogging([NotNull] this IHostBuilder builder,
            [NotNull] string sectionName = LoggerOptions.DefaultSectionName) =>
            builder.ConfigureLogging(x => x
                .WriteToConsole()
                .WriteToFluentd()
                .EnrichWithApplicationMetadata()
                .EnrichWithCorrelationId(), sectionName);
    }
}