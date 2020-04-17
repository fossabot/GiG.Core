using GiG.Core.DistributedTracing.Abstractions;
using GiG.Core.MultiTenant.Abstractions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using Xunit;

namespace GiG.Core.Http.Tests.Integration.Mocks
{
    public class TestFixture : IAsyncLifetime
    {
        internal IActivityContextAccessor ActivityContextAccessor;
        internal ITenantAccessor TenantAccessor;
        internal IHost Host;
        
        public async Task InitializeAsync()
        {
            Host = Microsoft.Extensions.Hosting.Host
                .CreateDefaultBuilder()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseTestServer();
                    webBuilder.UseStartup<MockStartup>();
                }).Build();

            await Host.StartAsync();

            ActivityContextAccessor = Host.Services.GetService<IActivityContextAccessor>();
            TenantAccessor = Host.Services.GetService<ITenantAccessor>();
        }

        public async Task DisposeAsync()
        {
            if (Host != null)
            {
                await Host?.StopAsync();
                await Host?.WaitForShutdownAsync();
                Host.Dispose();
            }
        }
    }
}