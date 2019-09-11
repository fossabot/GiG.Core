using Microsoft.Extensions.Configuration;
using Orleans;
using Orleans.Configuration;
using System;
using System.Reflection;

namespace GiG.Core.Orleans.Client
{
    /// <summary>
    /// Builder used for creating <see cref="IClusterClient"/> instances.
    /// </summary>
    public interface IClusterClientBuilder
    {
        /// <summary>
        /// Adds Assemblies to Cluster Client Builder with references.
        /// </summary>
        /// <param name="assemblies">The Assemblies which will be added to the cluster client.</param>
        /// <returns>Returns the <see cref="IClusterClientBuilder"/> so that more methods can be chained.</returns>
        IClusterClientBuilder WithAssemblies(params Assembly[] assemblies);

        /// <summary>
        /// Sets the Cluster settings using a <see cref="ClusterOptions"/> action.
        /// </summary>
        /// <returns>Returns the <see cref="IClusterClientBuilder"/> so that more methods can be chained.</returns>
        IClusterClientBuilder WithClusterOptions(Action<ClusterOptions> optionsAction);

        /// <summary>
        /// Sets the Cluster settings using an <see cref="IConfigurationSection"/>.
        /// </summary>
        /// <returns>Returns the <see cref="IClusterClientBuilder"/> so that more methods can be chained.</returns>
        IClusterClientBuilder WithClusterOptions(IConfigurationSection configurationSection);

        /// <summary>
        /// Sets the Cluster settings by retrieving a section from <see cref="IConfiguration"/>
        /// </summary>
        /// <returns>Returns the <see cref="IClusterClientBuilder"/> so that more methods can be chained.</returns>
        IClusterClientBuilder WithClusterOptions(IConfiguration configuration);

        /// <summary>
        /// Builds the cluster client.
        /// </summary>
        /// <returns>A <see cref="IClusterClientBuilder"/> ready to be used.</returns>
        IClusterClient Build();

        /// <summary>
        /// Configures the client to connect to a silo on the localhost.
        /// </summary>
        /// <returns>A <see cref="IClusterClientBuilder"/> ready to be used.</returns>
        IClusterClientBuilder WithLocalhostClustering();

        /// <summary>
        /// Registers the built <see cref="IClusterClientBuilder"/>.
        /// </summary>
        void Register();
    }
}