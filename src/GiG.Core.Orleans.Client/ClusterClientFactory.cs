using GiG.Core.Orleans.Client.Abstractions;
using JetBrains.Annotations;
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
        public void Add([NotNull] string name, [NotNull] IClusterClient clusterClient)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException($"'{nameof(name)}' must not be null, empty or whitespace.", nameof(name));
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
        public IClusterClient Get([NotNull] string name) 
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException($"'{nameof(name)}' must not be null, empty or whitespace.", nameof(name));

            if (!_clusterClients.TryGetValue(name, out var clusterClient))
            {
                throw new KeyNotFoundException($"No cluster client is registered with name [{name}]");
            }

            return clusterClient;
        }
    }
}