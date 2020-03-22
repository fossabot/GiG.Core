using GiG.Core.DistributedTracing.Activity.Extensions;
using GiG.Core.Orleans.Streams.Abstractions;
using JetBrains.Annotations;
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
        /// Creates and registers a new <see cref="IStreamFactory"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <returns>The <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddStream([NotNull] this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddActivityAccessor();
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
    }
}