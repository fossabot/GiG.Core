using GiG.Core.DistributedTracing.Abstractions;
using GiG.Core.DistributedTracing.Activity.Tests.Integration.Mocks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace GiG.Core.DistributedTracing.Activity.Tests.Integration.Lifetimes
{
    public class MockWebFixture : IAsyncLifetime
    {
        private IHost _host;

        public async Task InitializeAsync()
        {
            _host = Host
                .CreateDefaultBuilder()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseTestServer();
                    webBuilder.UseStartup<MockStartup>();
                }).Build();

            await _host.StartAsync();
        }

        public async Task DisposeAsync()
        {
            if (_host != null)
            {
                await _host?.StopAsync();
                await _host?.WaitForShutdownAsync();
                _host.Dispose();
            }
        }

        internal HttpClient HttpClient => _host.GetTestClient();
    }
}