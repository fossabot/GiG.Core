using GiG.Core.Orleans.Tests.Integration.Mocks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orleans;
using Orleans.Hosting;
using System;
using System.Net.Http;

namespace GiG.Core.Orleans.Tests.Integration.Fixtures
{
    public class HealthCheckCoHostedFixture : IDisposable
    {
        internal readonly IHttpClientFactory HttpClientFactory;
        internal readonly int Port = 7777;

        private IHost _host;

        public HealthCheckCoHostedFixture()
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

            _host.StartAsync().GetAwaiter().GetResult();
            HttpClientFactory = _host.Services.GetService<IHttpClientFactory>();
        }

        public void Dispose()
        {
            _host?.StopAsync();
            _host?.WaitForShutdown();
        }
    }
}