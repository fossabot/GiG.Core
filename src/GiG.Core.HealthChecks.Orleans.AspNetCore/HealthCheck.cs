using GiG.Core.HealthChecks.Orleans.Abstractions;
using GiG.Core.HealthChecks.Orleans.Extensions;
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

namespace GiG.Core.HealthChecks.Orleans.AspNetCore
{
    class HealthCheck : BackgroundService, IHostedService
    {
        private IWebHost _host;

        private readonly IConfiguration _configuration;
        private readonly IClusterClient _clusterClient; 
        private readonly ILogger<HealthCheck> _logger;
        private readonly HealthChecksOptions _healthChecksOptions;

        public HealthCheck(
            IConfiguration configuration,
            IClusterClient clusterClient,
            ILogger<HealthCheck> logger,
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
                    _host =
                        new WebHostBuilder()
                            .ConfigureServices(services =>
                            {
                                services.ConfigureHealthChecks(_configuration);
                                services.TryAddSingleton(_clusterClient);
                                services.AddHealthChecks().AddOrleansHealthCheck();
                            })
                            .Configure(app =>
                            {
                                app.UseHealthChecks();
                            })
                            .UseKestrel()
                            //.UseKestrel(x => x.ListenAnyIP(5555, listenOptions =>
                            //{
                            //    listenOptions.Protocols = HttpProtocols.Http1;
                            //}))
                            .UseUrls($"http://{_healthChecksOptions.DomainFilter}:{_healthChecksOptions.Port}")
                            .Build();

                    await _host.StartAsync(cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(10001, ex.ToString());
                }

                _logger.LogInformation($"Dashboard listening on {_healthChecksOptions.Port}");
            }
        }
       
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            try
            {
                await _host.StopAsync();
                _host?.Dispose();
            }
            catch
            {
                /* NOOP */
            }
        }

    }
}
