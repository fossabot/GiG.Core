using GiG.Core.HealthChecks.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace GiG.Core.Extensions.HealthCheck.Tests.Integration
{
    public class MockStartup
    {
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly HealthChecksOptions _healthChecksOptions;

        public MockStartup()
        {
            _configurationMock = new Mock<IConfiguration>();
            var configurationSection = new Mock<IConfigurationSection>();
            configurationSection.Setup(a => a.Value).Returns("testSection");
            _configurationMock.Setup(a => a.GetSection(HealthChecksOptions.DefaultSectionName))
                .Returns(configurationSection.Object);

            _healthChecksOptions = new HealthChecksOptions();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCachedHealthChecks(_configurationMock.Object);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseHealthChecks(_healthChecksOptions);
        }
    }
}
