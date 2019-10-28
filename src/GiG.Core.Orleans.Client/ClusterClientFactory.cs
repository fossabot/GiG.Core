using GiG.Core.Orleans.Client.Abstractions;
using Orleans;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GiG.Core.Orleans.Client
{
    /// <inheritdoc />
    public sealed class ClusterClientFactory : IClusterClientFactory
    {
        private readonly Dictionary<string, IClusterClient> _clusterClients = new Dictionary<string, IClusterClient>();
        private bool _isDisposing;

        /// <inheritdoc />
        public void Add(string name, IClusterClient clusterClient)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            if (clusterClient == null) throw new ArgumentNullException(nameof(clusterClient));

            _clusterClients.Add(name, clusterClient);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            if (!_isDisposing)
            {
                _isDisposing = true;
                _clusterClients.Values.ToList().ForEach(x => x.Close().Wait());
            }                        
        }

        /// <inheritdoc />
        public IClusterClient Get(string name) 
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            if (!_clusterClients.ContainsKey(name)) throw new KeyNotFoundException($"No cluster client is registered with name [{name}]");

            return _clusterClients[name];
        }
    }
}