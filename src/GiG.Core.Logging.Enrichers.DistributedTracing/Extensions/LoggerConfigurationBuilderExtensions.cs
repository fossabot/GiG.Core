using GiG.Core.DistributedTracing.Abstractions;
using GiG.Core.Logging.Abstractions;
using GiG.Core.Logging.Enrichers.DistributedTracing.Internal;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace GiG.Core.Logging.Enrichers.DistributedTracing.Extensions
{
    /// <summary>
    /// Logger Configuration Builder extensions.
    /// </summary>
    public static class LoggerConfigurationBuilderExtensions
    {
        /// <summary>
        /// Enrich log events with a Correlation ID.
        /// </summary>
        /// <param name="builder">Logger enrichment configuration.</param>
        /// <returns>Configuration object allowing method chaining.</returns>
        public static LoggerConfigurationBuilder EnrichWithCorrelationId([NotNull] this LoggerConfigurationBuilder builder)
        {
            var correlationContextAccessor = builder
                .Services
                .BuildServiceProvider()
                .GetService<ICorrelationContextAccessor>();

            if (correlationContextAccessor != null)
            {
                builder.LoggerConfiguration.Enrich.With(new CorrelationIdEnricher(correlationContextAccessor));
            }

            return builder;
        }
    }
}