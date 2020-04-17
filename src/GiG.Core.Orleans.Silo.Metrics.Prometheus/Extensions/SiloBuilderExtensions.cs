using GiG.Core.Orleans.Silo.Metrics.Abstractions;
using GiG.Core.Orleans.Silo.Metrics.Prometheus.Consumer;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Orleans.Configuration;
using Orleans.Hosting;
using System;

namespace GiG.Core.Orleans.Silo.Metrics.Prometheus.Extensions
{
    /// <summary>
    /// Silo Builder Extensions.
    /// </summary>
    public static class SiloBuilderExtensions
    {
        /// <summary>
        /// Adds Prometheus Telemetry Consumer to Silo Builder.
        /// </summary>
        /// <param name="siloBuilder">The Orleans <see cref="ISiloBuilder"/>.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/> from which to bind to <see cref="OrleansMetricsOptions"/>.</param>
        /// <returns>Returns the <see cref="ISiloBuilder"/> so that more methods can be chained.</returns>
        public static ISiloBuilder AddPrometheusTelemetry(this ISiloBuilder siloBuilder, [NotNull] IConfiguration configuration)
        {
            if (siloBuilder == null) throw new ArgumentNullException(nameof(siloBuilder));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            
            var configurationSection = configuration.GetSection(OrleansMetricsOptions.DefaultSectionName);
            siloBuilder.ConfigureServices(x => x.Configure<OrleansMetricsOptions>(configurationSection));

            var orleansMetricsOptions = configurationSection.Get<OrleansMetricsOptions>() ?? new  OrleansMetricsOptions();
            if (orleansMetricsOptions.IsEnabled)
            {
                siloBuilder.ConfigureServices(x =>
                    x.Configure<TelemetryOptions>(options => options.AddConsumer<TelemetryConsumer>()));
            }

            return siloBuilder;
        }
    }
}