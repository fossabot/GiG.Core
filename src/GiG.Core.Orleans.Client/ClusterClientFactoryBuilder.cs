using GiG.Core.Orleans.Client.Abstractions;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Orleans;
using System;

namespace GiG.Core.Orleans.Client
{
    /// <summary>
    /// The OrleansClusterClientFactory Builder.
    /// </summary>
    public class ClusterClientFactoryBuilder
    {
        private readonly ClusterClientFactory factory = new ClusterClientFactory();

        /// <summary>
        /// Default Constructor.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to register the factory on.</param>     
        public ClusterClientFactoryBuilder(IServiceCollection services)
        {
            services.TryAddSingleton<IClusterClientFactory>(factory);
        }

        /// <summary>
        /// Adds a Named Orleans Cluster Client to the Factory.
        /// </summary>
        /// <param name="name">The Name of the Orleans Cluster Client.</param>
        /// <param name="clusterClient">The Orleans <see cref="IClusterClient"/>.</param>
        /// <returns>The <see cref="ClusterClientFactoryBuilder"/>.</returns>
        public ClusterClientFactoryBuilder AddClusterClient([NotNull] string name, [NotNull] IClusterClient clusterClient)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException(nameof(name));
            if (clusterClient == null) throw new ArgumentNullException(nameof(clusterClient));

            factory.Add(name, clusterClient);

            return this;
        }

        /// <summary>
        /// Adds a Named Orleans Cluster Client to the Factory.
        /// </summary>
        /// <param name="name">The Name of the Orleans Cluster Client.</param>
        /// <param name="createClient">The <see cref="Func{IClusterClient}"/> which will be used to create the Orleans Cluster Client</param>
        /// <returns>The <see cref="ClusterClientFactoryBuilder"/>.</returns>
        public ClusterClientFactoryBuilder AddClusterClient([NotNull] string name, [NotNull] Func<IClusterClient> createClient)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException(nameof(name));
            if (createClient == null) throw new ArgumentNullException(nameof(createClient));

            return AddClusterClient(name, createClient.Invoke());
        }              
    }
}