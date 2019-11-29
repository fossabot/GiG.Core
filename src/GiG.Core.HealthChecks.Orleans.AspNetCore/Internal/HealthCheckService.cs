using GiG.Core.HealthChecks.Extensions;
using GiG.Core.HealthChecks.Orleans.Abstractions;
using GiG.Core.HealthChecks.Orleans.AspNetCore.Extensions;
using GiG.Core.HealthChecks.Orleans.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Orleans;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GiG.Core.HealthChecks.Orleans.AspNetCore.Internal
{
    internal class HealthCheckService : BackgroundService, IHostedService
    {
        private IHost _host;

        private readonly IConfiguration _configuration;
        private readonly IClusterClient _clusterClient; 
        private readonly ILogger<HealthCheckService> _logger;
        private readonly HealthChecksOptions _healthChecksOptions;

        public HealthCheckService(
            IConfiguration configuration,
            IClusterClient clusterClient,
            ILogger<HealthCheckService> logger,
            IOptions<HealthChecksOptions> healthChecksOptions)
        {
            _configuration = configuration;
            _clusterClient = clusterClient;
            _logger = logger;
            _healthChecksOptions = healthChecksOptions.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            if (_healthChecksOptions.HostSelf)
            {
                try
                {
                    _host = Host.CreateDefaultBuilder()
                            .ConfigureWebHostDefaults(webBuilder =>
                                webBuilder.ConfigureServices(services =>
                                {
                                    services.ConfigureOrleansHealthChecks(_configuration);
                                    services.AddHealthChecks().AddOrleansHealthCheck();
                                    services.TryAddSingleton(_clusterClient);
                                })
                                .Configure(app =>
                                {
                                    app.UseHealthChecks();
                                })
                                .UseKestrel()
                                .UseUrls($"http://{_healthChecksOptions.DomainFilter}:{_healthChecksOptions.Port}")
                            )
                            .Build();

                    await _host.StartAsync(cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message, ex);
                }

                _logger.LogInformation($"HealthCheck listening on {_healthChecksOptions.Port}");
            }
        }
       
        public override void Dispose()
        {
            try
            {
                _host.StopAsync();
                _host.WaitForShutdown();
                _host?.Dispose();
            }
            catch
            {
                /* NOOP */
            }
        }

    }
}
