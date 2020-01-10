using GiG.Core.Messaging.MassTransit.Internal;
using GreenPipes;
using JetBrains.Annotations;
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

            configurator.AddPipeSpecification(new ActivitySpecification<T>());

            return configurator;
        }
    }
}