using GiG.Core.HealthChecks.Orleans.AspNetCore.Extensions;
using System;
using Xunit;
// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.HealthChecks.Tests.Unit.Orleans
{
    [Trait("Category", "Unit")]
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void AddHealthCheckService_ServiceCollectionIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => ServiceCollectionExtensions.AddOrleansHealthChecksSelfHosted(null, (Microsoft.Extensions.Configuration.IConfiguration)null));
            Assert.Equal("services", exception.ParamName);

            exception = Assert.Throws<ArgumentNullException>(() => ServiceCollectionExtensions.AddOrleansHealthChecksSelfHosted(null, (Microsoft.Extensions.Configuration.IConfigurationSection)null));
            Assert.Equal("services", exception.ParamName);
        }
    }
}
