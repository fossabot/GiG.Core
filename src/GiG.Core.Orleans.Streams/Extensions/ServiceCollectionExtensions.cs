using GiG.Core.DistributedTracing.Activity.Extensions;
using GiG.Core.Orleans.Streams.Abstractions;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace GiG.Core.Orleans.Streams.Extensions
{
    /// <summary>
    /// The <see cref="IServiceCollection" /> Extensions.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Configures the Stream.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/> which binds to <see cref="StreamOptions"/>.</param>
        /// <returns>The <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection ConfigureStream([NotNull] this IServiceCollection services, [NotNull] IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            services.ConfigureStream(configuration.GetSection(StreamOptions.DefaultSectionName));

            return services;
        }

        /// <summary>
        /// Configures the Stream.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="configurationSection">The <see cref="IConfigurationSection"/> which binds to <see cref="StreamOptions"/>.</param>
        /// <returns>The <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection ConfigureStream([NotNull] this IServiceCollection services, [NotNull] IConfigurationSection configurationSection)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (configurationSection == null) throw new ArgumentNullException(nameof(configurationSection));

            StreamHelper.NamespacePrefix = configurationSection.Get<StreamOptions>()?.NamespacePrefix;
            if (string.IsNullOrWhiteSpace(StreamHelper.NamespacePrefix))
            {
                StreamHelper.NamespacePrefix = null;
            }

            services.Configure<StreamOptions>(configurationSection);

            return services;
        }
        
        /// <summary>
        /// Adds Stream.
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
        /// Adds the Command Dispatcher.
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
        /// Adds the Stream Id Provider.
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