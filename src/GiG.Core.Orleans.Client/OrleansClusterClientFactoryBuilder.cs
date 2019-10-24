using GiG.Core.Orleans.Client.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Orleans;
using System;

namespace GiG.Core.Orleans.Client
{
    /// <summary>
    /// The OrleansClusterClientFactory Builder.
    /// </summary>
    public class OrleansClusterClientFactoryBuilder
    {
        private readonly OrleansClusterClientFactory factory = new OrleansClusterClientFactory();

        /// <summary>
        /// Creates an Emptyu OrleansClusterClientFactory.
        /// </summary>
        /// <returns>The <see cref="OrleansClusterClientFactoryBuilder"/>.</returns>
        public static OrleansClusterClientFactoryBuilder CreateClusterClientFactoryBuilder()
        {
            return new OrleansClusterClientFactoryBuilder();
        }

        /// <summary>
        /// Adds a Named Orleans Cluster Client to the Factory.
        /// </summary>
        /// <param name="name">The Name of the Orleans Cluster Client.</param>
        /// <param name="clusterClient">The Orleans <see cref="IClusterClient"/>.</param>
        /// <returns>The <see cref="OrleansClusterClientFactoryBuilder"/>.</returns>
        public OrleansClusterClientFactoryBuilder AddClusterClient(string name, IClusterClient clusterClient)
        {
            factory.AddClusterClient(name, clusterClient);

            return this;
        }

        /// <summary>
        /// Adds a Named Orleans Cluster Client to the Factory.
        /// </summary>
        /// <param name="name">The Name of the Orleans Cluster Client.</param>
        /// <param name="createClient">The <see cref="Func{IClusterClient}"/> which will be used to create the Orleans Cluster Client</param>
        /// <returns>The <see cref="OrleansClusterClientFactoryBuilder"/>.</returns>
        public OrleansClusterClientFactoryBuilder AddClusterClient(string name, Func<IClusterClient> createClient)
        {
            return AddClusterClient(name, createClient.Invoke());
        }

        /// <summary>
        /// Registers the Orleans Cluster Client Factory.
        /// </summary>
        public void RegisterFactory(IServiceCollection services)
        {
            services.TryAddSingleton<IOrleansClusterClientFactory>(factory);
        }
    }
}
