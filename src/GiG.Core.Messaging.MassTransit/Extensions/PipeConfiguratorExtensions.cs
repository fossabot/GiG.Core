using GiG.Core.Messaging.MassTransit.Internal;
using GreenPipes;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Trace.Configuration;
using System;

namespace GiG.Core.Messaging.MassTransit.Extensions
{
    /// <summary>
    /// Pipeline Configurator Extensions.
    /// </summary>
    public static class PipeConfiguratorExtensions
    {
        /// <summary>
        /// Adds the ActivityFilter Middleware to the Publish Pipeline. 
        /// </summary>
        /// <param name="configurator">The <see cref="IPipeConfigurator{T}"/> on which to add the ActivityFilter. </param>
        /// <returns>The <see cref="IPipeConfigurator{T}"/>. </returns>
        public static IPipeConfigurator<T> UseActivityFilter<T>([NotNull] this IPipeConfigurator<T> configurator)
         where T : class, PipeContext
        {
            if (configurator == null) throw new ArgumentNullException(nameof(configurator));

            configurator.AddPipeSpecification(new ActivityFilterSpecification<T>());

            return configurator;
        }

        /// <summary>
        /// Adds the ActivityFilter Middleware with Tracing to the Publish Pipeline. 
        /// </summary>
        /// <param name="configurator">The <see cref="IPipeConfigurator{T}"/> on which to add the ActivityFilter. </param>
        /// <param name="serviceProvider">The <see cref="IServiceProvider"/>.</param>
        /// <returns>The <see cref="IPipeConfigurator{T}"/>. </returns>
        public static IPipeConfigurator<T> UseActivityFilterWithTracing<T>([NotNull] this IPipeConfigurator<T> configurator,
             [NotNull] IServiceProvider serviceProvider)
         where T : class, PipeContext
        {
            if (configurator == null) throw new ArgumentNullException(nameof(configurator));
            if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));

            var tracerFactory = serviceProvider.GetService<TracerFactory>();
            configurator.AddPipeSpecification(new ActivityFilterSpecification<T>(tracerFactory));

            return configurator;
        }
    }
}