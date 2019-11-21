using GiG.Core.Orleans.Client.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;
// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.Orleans.Tests.Unit.Client
{
    [Trait("Category", "Unit")]
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void AddDefaultClusterClient_ServiceCollectionIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => ServiceCollectionExtensions.AddDefaultClusterClient(null, configureClientWithServiceProvider: null));
            Assert.Equal("services", exception.ParamName);

            exception = Assert.Throws<ArgumentNullException>(() => ServiceCollectionExtensions.AddDefaultClusterClient(null, configureClient: null));
            Assert.Equal("services", exception.ParamName);
        }

        [Fact]
        public void AddDefaultClusterClient_ConfigureClientIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new ServiceCollection().AddDefaultClusterClient(configureClientWithServiceProvider: null));
            Assert.Equal("configureClientWithServiceProvider", exception.ParamName);

            exception = Assert.Throws<ArgumentNullException>(() => new ServiceCollection().AddDefaultClusterClient(configureClient: null));
            Assert.Equal("configureClient", exception.ParamName);
        }

        [Fact]
        public void CreateClusterClient_ServiceCollectionIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => ServiceCollectionExtensions.CreateClusterClient(null, configureClientWithServiceProvider: null));
            Assert.Equal("services", exception.ParamName);

            exception = Assert.Throws<ArgumentNullException>(() => ServiceCollectionExtensions.CreateClusterClient(null, configureClient: null));
            Assert.Equal("services", exception.ParamName);
        }

        [Fact]
        public void CreateClusterClient_ConfigureClientIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new ServiceCollection().CreateClusterClient(configureClientWithServiceProvider: null));
            Assert.Equal("configureClientWithServiceProvider", exception.ParamName);

            exception = Assert.Throws<ArgumentNullException>(() => new ServiceCollection().CreateClusterClient(configureClient: null));
            Assert.Equal("configureClient", exception.ParamName);
        }

        [Fact]
        public void AddClusterClientFactory_ServiceCollectionIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => ServiceCollectionExtensions.AddClusterClientFactory(null));
            Assert.Equal("services", exception.ParamName);
        }
    }
}