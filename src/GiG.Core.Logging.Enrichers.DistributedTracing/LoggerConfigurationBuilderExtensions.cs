using GiG.Core.DistributedTracing.Abstractions.CorrelationId;
using GiG.Core.Logging.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace GiG.Core.Logging.Enrichers.DistributedTracing
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
        public static LoggerConfigurationBuilder EnrichWithCorrelationId(this LoggerConfigurationBuilder builder)
        {
            var correlationContextAccessor = builder.Services.BuildServiceProvider()
                .GetService<ICorrelationContextAccessor>();

            if (correlationContextAccessor != null)
            {
                builder.LoggerConfiguration.Enrich.With(new CorrelationIdEnricher(correlationContextAccessor));
            }

            return builder;
        }
    }
}