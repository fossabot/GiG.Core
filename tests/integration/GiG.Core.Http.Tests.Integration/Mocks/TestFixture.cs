using GiG.Core.DistributedTracing.Abstractions;
using GiG.Core.MultiTenant.Abstractions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using Xunit;

namespace GiG.Core.Http.Tests.Integration.Mocks
{
    public class TestFixture : IAsyncLifetime
    {
        internal IActivityContextAccessor ActivityContextAccessor;
        internal IActivityTenantAccessor ActivityTenantAccessor;
        internal const string BaseUrl = "http://localhost:56125";
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

            ActivityContextAccessor = _host.Services.GetService<IActivityContextAccessor>();
            ActivityTenantAccessor = _host.Services.GetService<IActivityTenantAccessor>();
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