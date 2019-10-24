using GiG.Core.Logging.Abstractions;
using JetBrains.Annotations;
using System;

namespace GiG.Core.Logging.Enrichers.ApplicationMetadata.Extensions
{
    /// <summary>
    /// Logging Configuration Builder Extensions.
    /// </summary>
    public static class LoggingConfigurationBuilderExtensions
    {
        /// <summary>
        /// Enrich Log Events with Application Metadata.
        /// </summary>
        /// <param name="builder">The <see cref="LoggingConfigurationBuilder"/>.</param>
        /// <returns>The <see cref="LoggingConfigurationBuilder" />.</returns>
        public static LoggingConfigurationBuilder EnrichWithApplicationMetadata([NotNull] this LoggingConfigurationBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            builder.LoggerConfiguration.Enrich.WithProperty("ApplicationName", Hosting.ApplicationMetadata.Name);
            builder.LoggerConfiguration.Enrich.WithProperty("ApplicationVersion", Hosting.ApplicationMetadata.Version);

            return builder;
        }
    }
}