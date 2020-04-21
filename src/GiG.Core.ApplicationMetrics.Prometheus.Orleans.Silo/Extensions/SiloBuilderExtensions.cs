using GiG.Core.ApplicationMetrics.Prometheus.Orleans.Silo.Abstractions;
using GiG.Core.ApplicationMetrics.Prometheus.Orleans.Silo.Consumer;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Orleans.Configuration;
using Orleans.Hosting;
using System;

namespace GiG.Core.Metrics.Prometheus.Orleans.Silo.Extensions
{
    /// <summary>
    /// The <see cref="ISiloBuilder" /> Extensions.
    /// </summary>
    public static class SiloBuilderExtensions
    {
        /// <summary>
        /// Adds Prometheus Telemetry Consumer to Silo Builder.
        /// </summary>
        /// <param name="siloBuilder">The Orleans <see cref="ISiloBuilder"/>.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/> which binds to <see cref="OrleansMetricsOptions"/>.</param>
        /// <returns>Returns the <see cref="ISiloBuilder"/> so that more methods can be chained.</returns>
        public static ISiloBuilder AddPrometheusTelemetry([NotNull] this ISiloBuilder siloBuilder, [NotNull] IConfiguration configuration)
        {
            if (siloBuilder == null) throw new ArgumentNullException(nameof(siloBuilder));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            
            var configurationSection = configuration.GetSection(OrleansMetricsOptions.DefaultSectionName);
          
            return AddPrometheusTelemetry(siloBuilder, configurationSection);
        }
        
        /// <summary>
        /// Adds Prometheus Telemetry Consumer to Silo Builder from configuration section.
        /// </summary>
        /// <param name="siloBuilder">The Orleans <see cref="ISiloBuilder"/>.</param>
        /// <param name="configurationSection">The <see cref="IConfigurationSection"/> which binds to <see cref="OrleansMetricsOptions"/>.</param>
        /// <returns>Returns the <see cref="ISiloBuilder"/> so that more methods can be chained.</returns>
        public static ISiloBuilder AddPrometheusTelemetry([NotNull] this ISiloBuilder siloBuilder, [NotNull] IConfigurationSection configurationSection)
        {
            if (siloBuilder == null) throw new ArgumentNullException(nameof(siloBuilder));
     
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