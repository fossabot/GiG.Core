﻿using GiG.Core.DistributedTracing.Activity.Extensions;
using GiG.Core.Orleans.Streams.Abstractions;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace GiG.Core.Orleans.Streams.Extensions
{
    /// <summary>
    /// Service Collection Extensions.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Configure the Stream Options from configuration.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/>.</param>
        /// <returns>The <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection ConfigureStream([NotNull] this IServiceCollection services, [NotNull] IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            services.ConfigureStream(configuration.GetSection(StreamOptions.DefaultSectionName));

            return services;
        }
        
        /// <summary>
        /// Configure the Stream Options from configuration.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="configurationSection">The <see cref="IConfigurationSection"/>.</param>
        /// <returns>The <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection ConfigureStream([NotNull] this IServiceCollection services, [NotNull] IConfigurationSection configurationSection)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (configurationSection == null) throw new ArgumentNullException(nameof(configurationSection));

            StreamHelper.NamespacePrefix = configurationSection.Get<StreamOptions>()?.NamespacePrefix;
            services.Configure<StreamOptions>(configurationSection);

            return services;
        }
        
        /// <summary>
        /// Creates and registers a new <see cref="IStreamFactory"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <returns>The <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddStream([NotNull] this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            
            services.AddActivityContextAccessor();
            services.TryAddSingleton<IStreamFactory, StreamFactory>();

            return services;
        }
        
        /// <summary>
        /// Creates and registers a new <see cref="ICommandDispatcherFactory{TCommand, TSuccess, TFailure}"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <returns>The <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddCommandDispatcher([NotNull] this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.TryAddSingleton(typeof(ICommandDispatcherFactory<,,>), typeof(CommandDispatcherFactory<,,>));

            return services;
        }

        /// <summary>
        /// Creates and registers the <see cref="IStreamIdProvider"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <returns>The <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddStreamIdProvider([NotNull] this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.TryAddSingleton<IStreamIdProvider, StreamIdProvider>();

            return services;
        }
    }
}