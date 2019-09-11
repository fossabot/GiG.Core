using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace GiG.Core.Orleans.Client
{
    /// <inheritdoc />
    public class ClusterClientBuilder : IClusterClientBuilder
    {
        private const string ClusterOptionsDefaultSection = "Orleans:Cluster";

        private readonly IClientBuilder _clientBuilder;
        private readonly IServiceCollection _serviceCollection;
        private readonly Lazy<IClusterClient> _clusterClient;
        private bool _clusterOptionsSet = false;
        
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="serviceCollection"></param>
        public ClusterClientBuilder(IServiceCollection serviceCollection)
        {
            _clientBuilder = new ClientBuilder();
            _clusterClient = new Lazy<IClusterClient>(BuildInternal);
            _serviceCollection = serviceCollection;
        }

        /// <inheritdoc />
        public IClusterClientBuilder WithAssemblies(params Assembly[] assemblies)
        {
            _clientBuilder
                .ConfigureApplicationParts(parts =>
                {
                    foreach (var assembly in assemblies)
                    {
                        parts.AddApplicationPart(assembly).WithReferences();
                    }
                });

            return this;
        }

        /// <inheritdoc />
        public IClusterClientBuilder WithClusterOptions(Action<ClusterOptions> optionsAction)
        {
            _clusterOptionsSet = true;

            _clientBuilder.Configure(optionsAction);

            return this;
        }

        /// <inheritdoc />
        public IClusterClientBuilder WithClusterOptions(IConfigurationSection configurationSection)
        {
            _clusterOptionsSet = true;

            _clientBuilder.Configure<ClusterOptions>(configurationSection);

            return this;
        }

        /// <inheritdoc />
        public IClusterClientBuilder WithClusterOptions(IConfiguration configuration)
        {
            var configurationSection = configuration.GetSection(ClusterOptionsDefaultSection);
            if (configurationSection == null)
            {
                throw new InvalidOperationException($"Configuration section '{ClusterOptionsDefaultSection}' does not exist");
            }

            return WithClusterOptions(configurationSection);
        }

        /// <inheritdoc />
        public IClusterClientBuilder WithLocalhostClustering()
        {
            _clientBuilder.UseLocalhostClustering();
            return this;
        }

        /// <inheritdoc />
        public IClusterClient Build()
        {
            if(!_clusterOptionsSet)
                throw new InvalidOperationException("Cluster options have not been set.");

            return _clusterClient.Value;
        }

        private IClusterClient BuildInternal()
        {
            var serviceProvider = _serviceCollection.BuildServiceProvider();
            
            var logger = serviceProvider.GetService<ILogger>();

            var clusterClient = _clientBuilder.Build();
            
            clusterClient.Connect(CreateRetryFilter(logger)).GetAwaiter().GetResult();

            return clusterClient;
        }

        /// <inheritdoc />
        public void Register()
        {
            _serviceCollection.AddSingleton(_clusterClient.Value);
        }

        private static Func<Exception, Task<bool>> CreateRetryFilter(ILogger logger, int maxAttempts = 5)
        {
            var attempt = 0;
            return RetryFilter;

            async Task<bool> RetryFilter(Exception exception)
            {
                attempt++;
                logger?.LogWarning("Exception while attempting to connect to Orleans cluster: {Exception}", exception);
                if (attempt > maxAttempts)
                {
                    return false;
                }

                await Task.Delay(TimeSpan.FromSeconds(2));
                return true;
            }
        }
    }
}