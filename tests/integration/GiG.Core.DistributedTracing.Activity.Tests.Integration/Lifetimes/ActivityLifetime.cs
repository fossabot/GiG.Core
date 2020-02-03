using GiG.Core.DistributedTracing.Abstractions;
using GiG.Core.DistributedTracing.Activity.Tests.Integration.Mocks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace GiG.Core.DistributedTracing.Activity.Tests.Integration.Lifetimes
{
    public class ActivityLifetime : IAsyncLifetime
    {
        internal IHttpClientFactory HttpClientFactory;
        internal IActivityContextAccessor ActivityContextAccessor;
        internal string BaseUrl = "http://localhost:56561";
        private IHost _host;
       
        public async Task InitializeAsync()
        {
            _host = Host
                    .CreateDefaultBuilder()
                    .ConfigureWebHostDefaults(webBuilder =>
                    {
                        webBuilder.UseKestrel().UseUrls(BaseUrl);
                        webBuilder.UseStartup<MockStartup>();
                    }).Build();

            await _host.StartAsync();
            HttpClientFactory = _host.Services.GetService<IHttpClientFactory>();
            ActivityContextAccessor = _host.Services.GetService<IActivityContextAccessor>();
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
    }
}