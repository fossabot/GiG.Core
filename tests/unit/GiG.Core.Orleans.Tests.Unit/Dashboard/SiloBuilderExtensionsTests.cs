using GiG.Core.Orleans.Silo.Dashboard.Extensions;
using Microsoft.Extensions.Hosting;
using System;
using Xunit;
// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.Orleans.Tests.Unit.Dashboard
{
    [Trait("Category", "Unit")]
    public class SiloBuilderExtensionsTests
    {
        [Fact]
        public void ConfigureDashboard_SiloBuilderIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => SiloBuilderExtensions.ConfigureDashboard(null, null));
            Assert.Equal("siloBuilder", exception.ParamName);
        }

        [Fact]
        public void ConfigureDashboard_ConfigurationIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => 
                            Host.CreateDefaultBuilder().UseOrleans(sb => sb.ConfigureDashboard(null)).Build());
            Assert.Equal("configuration", exception.ParamName);
        }
    }
}
