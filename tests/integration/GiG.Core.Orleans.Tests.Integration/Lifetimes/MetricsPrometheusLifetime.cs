using GiG.Core.Orleans.Tests.Integration.Mocks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace GiG.Core.Orleans.Tests.Integration.Lifetimes
{
    public class MetricsPrometheusLifetime : IAsyncLifetime
    {
        internal HttpClient HttpClient;
        private IHost _host;

        public async Task InitializeAsync()
        {
            _host = new HostBuilder()
                .ConfigureAppConfiguration(a => a.AddJsonFile("appsettings.json"))
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseTestServer();
                    webBuilder.UseStartup<MockMetricsStartup>();
                })
                .UseOrleans(MockMetricsStartup.ConfigureOrleans)
                .Build();

            await _host.StartAsync();

            HttpClient = _host.GetTestClient();
            
        }

        public async Task DisposeAsync()
        {
            if (_host != null)
            {
                await _host.StopAsync();
                await _host.WaitForShutdownAsync();
                _host.Dispose();
            }
        }
    }
}