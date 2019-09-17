using GiG.Core.Logging.Abstractions;
using JetBrains.Annotations;

namespace GiG.Core.Logging.Enrichers.ApplicationMetadata.Extensions
{
    /// <summary>
    /// Logger Configuration Builder extensions.
    /// </summary>
    public static class LoggerConfigurationBuilderExtensions
    {
        /// <summary>
        /// Enrich log events with Application Metadata.
        /// </summary>
        /// <param name="builder">The <see cref="LoggerConfigurationBuilder"/>.</param>
        /// <returns>The <see cref="LoggerConfigurationBuilder" />.</returns>
        public static LoggerConfigurationBuilder EnrichWithApplicationMetadata([NotNull] this LoggerConfigurationBuilder builder)
        {
            builder.LoggerConfiguration.Enrich.WithProperty("ApplicationName", Hosting.ApplicationMetadata.Name);
            builder.LoggerConfiguration.Enrich.WithProperty("ApplicationVersion", Hosting.ApplicationMetadata.Version);

            return builder;
        }
    }
}