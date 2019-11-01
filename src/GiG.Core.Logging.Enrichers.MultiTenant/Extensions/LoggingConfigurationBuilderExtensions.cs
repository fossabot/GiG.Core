using GiG.Core.Logging.Abstractions;
using GiG.Core.Logging.Enrichers.MultiTenant.Internal;
using GiG.Core.MultiTenant.Abstractions;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace GiG.Core.Logging.Enrichers.MultiTenant.Extensions
{
    /// <summary>
    /// Logging Configuration Builder Extensions.
    /// </summary>
    public static class LoggingConfigurationBuilderExtensions
    {
        /// <summary>
        /// Enrich Log Events with a Tenant ID.
        /// </summary>
        /// <param name="builder">The <see cref="LoggingConfigurationBuilder" />.</param>
        /// <returns>The <see cref="LoggingConfigurationBuilder" />.</returns>
        public static LoggingConfigurationBuilder EnrichWithTenant([NotNull] this LoggingConfigurationBuilder builder)
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