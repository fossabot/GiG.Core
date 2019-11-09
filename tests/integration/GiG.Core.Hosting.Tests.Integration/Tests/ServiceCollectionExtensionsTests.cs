using GiG.Core.Hosting.Extensions;
using Microsoft.Extensions.Hosting;
using System;
using Xunit;
// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.Hosting.Tests.Integration.Tests
{
    [Trait("Category", "Integration")]
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void ConfigureInfoManagement_ServiceCollectionIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(
                () => ServiceCollectionExtensions.ConfigureInfoManagement(null, null));

            Assert.Throws<ArgumentNullException>(
                () => ServiceCollectionExtensions.ConfigureInfoManagement(null, configuration: null));
        }

        [Fact]
        public void ConfigureInfoManagement_ConfigurationIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(
                () => Host.CreateDefaultBuilder().ConfigureServices(x =>
            {
                x.ConfigureInfoManagement(configuration: null);
            }).Build());
        }

        [Fact]
        public void ConfigureInfoManagement_ConfigurationSectionIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(
                () => Host.CreateDefaultBuilder().ConfigureServices(x =>
                {
                    x.ConfigureInfoManagement(configurationSection: null);
                }).Build());        }
    }
}
