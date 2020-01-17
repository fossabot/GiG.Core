using GiG.Core.Data.KVStores.Extensions;
using GiG.Core.Data.KVStores.Providers.FileProviders.Extensions;
using Microsoft.Extensions.Hosting;
using System;
using System.Configuration;
using Xunit;
using FileProvidersKVStoreBuilderExtensions = GiG.Core.Data.KVStores.Providers.FileProviders.Extensions.KVStoreBuilderExtensions;
using KVStoreBuilderExtensions = GiG.Core.Data.KVStores.Extensions.KVStoreBuilderExtensions;
// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.Data.Tests.Unit.KVStores
{
    [Trait("Category", "Unit")]
    public class KVStoreBuilderExtensionsTests
    {
        [Fact]
        public void AddMemoryDataStore_KVStoreBuilderIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => KVStoreBuilderExtensions.AddMemoryDataStore<object>(null));
            Assert.Equal("builder", exception.ParamName);
        }

        [Fact]
        public void FromJsonFile_KVStoreBuilderIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => FileProvidersKVStoreBuilderExtensions.FromFile<object>(null, null, ""));
            Assert.Equal("builder", exception.ParamName);
        }

        [Fact]
        public void FromJsonFileWithConfigurationSection_KVStoreBuilderIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => FileProvidersKVStoreBuilderExtensions.FromFile<object>(null, null));
            Assert.Equal("builder", exception.ParamName);
        }

        [Fact]
        public void FromJsonFile_ConfigurationIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                Host.CreateDefaultBuilder()
                    .ConfigureServices(x =>
                        x.AddKVStores<object>().FromFile(null, ""))
                    .Build());
            Assert.Equal("configuration", exception.ParamName);
        }

        [Fact]
        public void FromJsonFile_ConfigurationSectionIsNull_ThrowsConfigurationErrorsException()
        {
            var exception = Assert.Throws<ConfigurationErrorsException>(() =>
                Host.CreateDefaultBuilder()
                    .ConfigureServices(x =>
                        x.AddKVStores<object>().FromFile(null))
                    .Build());
            Assert.Equal("Configuration section '' is incorrect.", exception.Message);
        }

        [Fact]
        public void FromJsonFile_ConfigurationSectionNameIsNullOrEmpty_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentException>(() =>
                Host.CreateDefaultBuilder()
                    .ConfigureServices((x, y) =>
                        y.AddKVStores<object>().FromFile(x.Configuration, ""))
                    .Build());
            Assert.Equal("'configurationSectionName' must not be null, empty or whitespace. (Parameter 'configurationSectionName')", exception.Message);
        }

        [Fact]
        public void AddFileDataProvider_ServiceCollectionIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => FileProvidersKVStoreBuilderExtensions.AddFileDataProvider(null));
            Assert.Equal("services", exception.ParamName);
        }
    }
}
