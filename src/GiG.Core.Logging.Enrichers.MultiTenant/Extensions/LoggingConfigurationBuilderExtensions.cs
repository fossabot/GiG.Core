using GiG.Core.Logging.Abstractions;
using GiG.Core.Logging.Enrichers.MultiTenant.Internal;
using GiG.Core.MultiTenant.Abstractions;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace GiG.Core.Logging.Enrichers.MultiTenant.Extensions
{
    /// <summary>
    /// Logging Configuration Builder extensions.
    /// </summary>
    public static class LoggingConfigurationBuilderExtensions
    {
        /// <summary>
        /// Enrich log events with a Tenant ID.
        /// </summary>
        /// <param name="builder">The delegate for configuring the <see cref="LoggingConfigurationBuilder" />.</param>
        /// <returns><see cref="LoggingConfigurationBuilder" /> object allowing method chaining.</returns>
        public static LoggingConfigurationBuilder EnrichWithTenantId(
            [NotNull] this LoggingConfigurationBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            var tenantAccessor = builder
                .Services
                .BuildServiceProvider()
                .GetService<ITenantAccessor>();

            if (tenantAccessor != null)
            {
                builder.LoggerConfiguration.Enrich.With(new TenantEnricher(tenantAccessor));
            }

            return builder;
        }
    }
}