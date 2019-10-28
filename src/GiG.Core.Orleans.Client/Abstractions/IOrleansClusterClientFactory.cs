using Orleans;
using System;

namespace GiG.Core.Orleans.Client.Abstractions
{
    /// <summary>
    /// Orleans Cluster Client Factory.
    /// </summary>
    public interface IClusterClientFactory : IDisposable
    {
        /// <summary>
        /// Adds a Named Orleans Cluster Client.
        /// </summary>
        /// <param name="name">The Name of the Cluster.</param>
        /// <param name="clusterClient">The Orleans <see cref="IClusterClient"/>.</param>
        void Add(string name, IClusterClient clusterClient);

        /// <summary>
        /// Gets the specified Orleans Cluster Client.
        /// </summary>
        /// <param name="name">The Name of the Cluster.</param>
        /// <returns>The Orleans <see cref="IClusterClient"/>.</returns>
        IClusterClient Get(string name);
    }
}
