using GiG.Core.Data.KVStores.Extensions;
using GiG.Core.Data.KVStores.Providers.File.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Xunit;

// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.Data.Tests.Unit.KVStores
{
    [Trait("Category", "Unit")]
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void AddKVStores_ServiceCollectionIsNull_ThrowsArgumentNullException()
        {
            var exception =
                Assert.Throws<ArgumentNullException>(() => ServiceCollectionExtensions.AddKVStores<object>(null, null));
            Assert.Equal("services", exception.ParamName);
        }

        [Fact]
        public void AddKVStores_ActionIsNull_ThrowsArgumentNullException()
        {
            var exception =
                Assert.Throws<ArgumentNullException>(() => new ServiceCollection().AddKVStores<object>(null));
            Assert.Equal("configureKVStore", exception.ParamName);
        }

        [Fact]
        public void AddKVStores_DataProvidersNotRegistered_ThrowsApplicationException()
        {
            var exception =
                Assert.Throws<ApplicationException>(() => new ServiceCollection().AddKVStores<object>(x => { }));
            Assert.Equal("No data providers were registered for System.Object.", exception.Message);
        }

        [Fact]
        public void AddKVStores_MultipleDataProvidersRegistered_ThrowsApplicationException()
        {
            // Arrange
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string> {{"test:Path", "test"}})
                .Build();

            // Act
            var exception = Assert.Throws<ApplicationException>(() =>
                new ServiceCollection().AddKVStores<object>(x =>
                    x.FromFile(configuration.GetSection("test"))
                        .FromFile(configuration.GetSection("test"))));

            // Assert
            Assert.Equal("Data Provider for System.Object has already been registered.", exception.Message);
        }
    }
}