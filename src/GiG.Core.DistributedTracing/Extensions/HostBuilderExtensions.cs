﻿using GiG.Core.DistributedTracing.Abstractions;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Configuration;

namespace GiG.Core.DistributedTracing.Extensions
{
    /// <summary>
    /// Host Builder Extensions.
    /// </summary>
    public static class HostBuilderExtensions
    {
        /// <summary>
        /// Configures Tracing Exporters.
        /// </summary>
        /// <param name="builder">The <see cref="IHostBuilder"/>.</param>
        /// <param name="tracingConfigurationBuilder">>A delegate that is used to configure the <see cref="TracingConfigurationBuilder" />.</param>
        /// <param name="sectionName">The configuration section name.</param>
        /// <returns>The <see cref="IHostBuilder"/>.</returns>
        public static IHostBuilder ConfigureTracing([NotNull] this IHostBuilder builder, Action<TracingConfigurationBuilder> tracingConfigurationBuilder,
            [NotNull] string sectionName = TracingOptions.DefaultSectionName)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (sectionName == null) throw new ArgumentNullException(nameof(sectionName));
            
            return builder
                .ConfigureServices((context, services) => ConfigureTracingInternal(context, services, tracingConfigurationBuilder, sectionName));
        }

        private static void ConfigureTracingInternal(HostBuilderContext context, IServiceCollection services, Action<TracingConfigurationBuilder> configureTracing, string sectionName)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            
            var configuration = context.Configuration;

            var configurationSection = configuration.GetSection(sectionName);
            if (configurationSection == null)
            {
                throw new ConfigurationErrorsException($"Configuration section '{sectionName}' does not exist");
            }
            
            var tracingOptions = configurationSection.Get<TracingOptions>();
            
            if (tracingOptions?.IsEnabled != true)
            {
                return;
            }
            
            if (tracingOptions.Exporters == null)
            {
                throw new ConfigurationErrorsException("No tracing exporters were configured.  Please add at least one tracing exporter");
            }

            var builder = new TracingConfigurationBuilder(services, tracingOptions.Exporters);

            configureTracing?.Invoke(builder);

            if (!builder.IsExporterConfigured)
            {
                throw new ConfigurationErrorsException("Tracing is enabled but no tracing exporters were configured.");
            }
        }
    }
}