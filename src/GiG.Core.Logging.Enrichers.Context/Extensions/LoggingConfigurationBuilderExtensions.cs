using GiG.Core.Context.Abstractions;
using GiG.Core.Logging.Abstractions;
using GiG.Core.Logging.Enrichers.Context.Internal;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace GiG.Core.Logging.Enrichers.Context.Extensions
{
    /// <summary>
    /// Logging Configuration Builder extensions.
    /// </summary>
    public static class LoggingConfigurationBuilderExtensions
    {
        /// <summary>
        /// Enrich log events with a Request Context info.
        /// </summary>
        /// <param name="builder">Logging enrichment configuration.</param>
        /// <returns>Configuration object allowing method chaining.</returns>
        public static LoggingConfigurationBuilder EnrichWithRequestContext(
            [NotNull] this LoggingConfigurationBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            var requestContextAccessor = builder
                .Services
                .BuildServiceProvider()
                .GetService<IRequestContextAccessor>();

            if (requestContextAccessor != null)
            {
                builder.LoggerConfiguration.Enrich.With(new RequestContextEnricher(requestContextAccessor));
            }

            return builder;
        }
    }
}
