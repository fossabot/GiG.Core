using GiG.Core.DistributedTracing.Abstractions;
using GiG.Core.Logging.Abstractions;
using GiG.Core.Logging.Enrichers.DistributedTracing.Internal;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace GiG.Core.Logging.Enrichers.DistributedTracing.Extensions
{
    /// <summary>
    /// Logging Configuration Builder Extensions.
    /// </summary>
    public static class LoggingConfigurationBuilderExtensions
    {
        /// <summary>
        /// Enrich Log Events with a Correlation ID.
        /// </summary>
        /// <param name="builder">The <see cref="LoggingConfigurationBuilder" />.</param>
        /// <returns>The <see cref="LoggingConfigurationBuilder" />.</returns>
        public static LoggingConfigurationBuilder EnrichWithCorrelationId([NotNull] this LoggingConfigurationBuilder builder)
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