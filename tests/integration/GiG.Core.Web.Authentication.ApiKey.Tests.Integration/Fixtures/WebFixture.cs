using GiG.Core.Web.Authentication.ApiKey.Tests.Integration.Mocks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using System;

namespace GiG.Core.Web.Authentication.ApiKey.Tests.Integration.Fixtures
{
    public class WebFixture : IDisposable
    {
        public readonly IHost Host;

        public WebFixture()
        {
            Host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(x =>
                {
                    x.UseStartup<MockStartup>();
                    x.UseTestServer();
                })
                .Build();
            
            Host.Start();
        }

        public void Dispose()
        {
            Host?.Dispose();
        }
    }
}