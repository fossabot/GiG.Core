using GiG.Core.Orleans.Tests.Integration.Mocks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace GiG.Core.Orleans.Tests.Integration.Lifetimes
{
    public class HealthCheckCoHostedLifetime : IAsyncLifetime
    {
        internal IHttpClientFactory HttpClientFactory;
        internal const int Port = 7777;

        private IHost _host;

        public async Task InitializeAsync()
        {
            _host = new HostBuilder()
                .ConfigureAppConfiguration(a => a.AddJsonFile("appsettings.json"))
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseKestrel(options => options.ListenAnyIP(Port));
                    webBuilder.UseStartup<MockStartUp>();
                })
               .UseOrleans(MockStartUp.ConfigureOrleans)
               .ConfigureServices(MockStartUp.ConfigureOrleansServices)
               .Build();

            await _host.StartAsync();
            HttpClientFactory = _host.Services.GetService<IHttpClientFactory>();
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