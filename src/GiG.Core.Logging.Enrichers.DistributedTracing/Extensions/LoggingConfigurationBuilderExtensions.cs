using GiG.Core.DistributedTracing.Abstractions;
using GiG.Core.Logging.Abstractions;
using GiG.Core.Logging.Enrichers.DistributedTracing.Internal;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace GiG.Core.Logging.Enrichers.DistributedTracing.Extensions
{
    /// <summary>
    /// Logging Configuration Builder extensions.
    /// </summary>
    public static class LoggingConfigurationBuilderExtensions
    {
        /// <summary>
        /// Enrich log events with a Correlation ID.
        /// </summary>
        /// <param name="builder">The delegate for configuring the <see cref="LoggingConfigurationBuilder" />.</param>
        /// <returns><see cref="LoggingConfigurationBuilder" /> object allowing method chaining.</returns>
        public static LoggingConfigurationBuilder EnrichWithCorrelationId(
            [NotNull] this LoggingConfigurationBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

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