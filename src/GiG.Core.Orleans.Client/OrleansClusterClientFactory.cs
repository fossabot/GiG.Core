using GiG.Core.Orleans.Client.Abstractions;
using Orleans;
using System.Collections.Generic;
using System.Linq;

namespace GiG.Core.Orleans.Client
{
    /// <inheritdoc />
    public class OrleansClusterClientFactory : IOrleansClusterClientFactory
    {
        private readonly Dictionary<string, IClusterClient> _clusterClients = new Dictionary<string, IClusterClient>();

        /// <inheritdoc />
        public void AddClusterClient(string name, IClusterClient clusterClient)
        {
            _clusterClients.Add(name, clusterClient);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _clusterClients.Values.ToList().ForEach(x => x.Close().Wait());
        }

        /// <inheritdoc />
        public IClusterClient GetClusterClient(string name) 
        {
            return _clusterClients[name];
        }
    }
}
