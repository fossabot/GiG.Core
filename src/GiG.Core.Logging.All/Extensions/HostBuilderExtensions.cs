using GiG.Core.Logging.Abstractions;
using GiG.Core.Logging.Enrichers.ApplicationMetadata.Extensions;
using GiG.Core.Logging.Enrichers.Context.Extensions;
using GiG.Core.Logging.Enrichers.DistributedTracing.Extensions;
using GiG.Core.Logging.Extensions;
using GiG.Core.Logging.Sinks.Console.Extensions;
using GiG.Core.Logging.Sinks.File.Extensions;
using GiG.Core.Logging.Sinks.Fluentd.Extensions;
using GiG.Core.Logging.Sinks.RabbitMQ.Extensions;
using JetBrains.Annotations;
using Microsoft.Extensions.Hosting;
using System;

namespace GiG.Core.Logging.All.Extensions
{
    /// <summary>
    /// Host Builder Extensions.
    /// </summary>
    public static class HostBuilderExtensions
    {
        /// <summary>
        /// Configures the default logging sinks (Console, Fluentd and File) and enrichers (Application metadata and Correlation ID).
        /// </summary>
        /// <param name="builder">Host builder.</param>
        /// <param name="sectionName">Configuration section name.</param>
        /// <returns>Host builder.</returns>
        public static IHostBuilder ConfigureLogging([NotNull] this IHostBuilder builder,
            [NotNull] string sectionName = LoggingOptions.DefaultSectionName)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (string.IsNullOrWhiteSpace(sectionName)) throw new ArgumentException($"'{nameof(sectionName)}' must not be null, empty or whitespace.", nameof(sectionName));

            return builder.ConfigureLogging(x => x
                .WriteToConsole()
                .WriteToFluentd()
                .WriteToRabbitMQ()
                .WriteToFile()
                .EnrichWithApplicationMetadata()
                .EnrichWithActivityContext()
                .EnrichWithRequestContext()
                , sectionName);
        }
    }
}