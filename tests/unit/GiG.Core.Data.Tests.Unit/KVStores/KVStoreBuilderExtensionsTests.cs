using GiG.Core.Data.KVStores.Extensions;
using GiG.Core.Data.KVStores.Providers.FileProviders.Extensions;
using Microsoft.Extensions.Hosting;
using System;
using System.Configuration;
using Xunit;
using FileProvidersKVStoreBuilderExtensions = GiG.Core.Data.KVStores.Providers.FileProviders.Extensions.KVStoreBuilderExtensions;
using EtcdKVStoreBuilderExtensions = GiG.Core.Data.KVStores.Providers.Etcd.Extensions.KVStoreBuilderExtensions;
using KVStoreBuilderExtensions = GiG.Core.Data.KVStores.Extensions.KVStoreBuilderExtensions;
using GiG.Core.Data.KVStores.Providers.Etcd.Extensions;
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

        #region FromFile

        [Fact]
        public void FromFile_KVStoreBuilderIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => FileProvidersKVStoreBuilderExtensions.FromFile<object>(null, null, ""));
            Assert.Equal("builder", exception.ParamName);
        }

        [Fact]
        public void FromFileWithConfigurationSection_KVStoreBuilderIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => FileProvidersKVStoreBuilderExtensions.FromFile<object>(null, null));
            Assert.Equal("builder", exception.ParamName);
        }

        [Fact]
        public void FromFile_ConfigurationIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                Host.CreateDefaultBuilder()
                    .ConfigureServices(x =>
                        x.AddKVStores<object>().FromFile(null, ""))
                    .Build());
            Assert.Equal("configuration", exception.ParamName);
        }

        [Fact]
        public void FromFile_ConfigurationSectionIsNull_ThrowsConfigurationErrorsException()
        {
            var exception = Assert.Throws<ConfigurationErrorsException>(() =>
                Host.CreateDefaultBuilder()
                    .ConfigureServices(x =>
                        x.AddKVStores<object>().FromFile(null))
                    .Build());
            Assert.Equal("Configuration section '' is incorrect.", exception.Message);
        }

        [Fact]
        public void FromFile_ConfigurationSectionNameIsNullOrEmpty_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentException>(() =>
                Host.CreateDefaultBuilder()
                    .ConfigureServices((x, y) =>
                        y.AddKVStores<object>().FromFile(x.Configuration, ""))
                    .Build());
            Assert.Equal("'configurationSectionName' must not be null, empty or whitespace. (Parameter 'configurationSectionName')", exception.Message);
        }

        #endregion

        #region FromEtcd

        [Fact]
        public void FromEtcd_KVStoreBuilderIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => EtcdKVStoreBuilderExtensions.FromEtcd<object>(null, null, ""));
            Assert.Equal("builder", exception.ParamName);
        }

        [Fact]
        public void FromEtcd_WithConfigurationSection_KVStoreBuilderIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => EtcdKVStoreBuilderExtensions.FromEtcd<object>(null, null));
            Assert.Equal("builder", exception.ParamName);
        }

        [Fact]
        public void FromEtcd_ConfigurationIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                Host.CreateDefaultBuilder()
                    .ConfigureServices(x =>
                        x.AddKVStores<object>().FromEtcd(null, ""))
                    .Build());
            Assert.Equal("configuration", exception.ParamName);
        }

        [Fact]
        public void FromEtcd_ConfigurationSectionIsNull_ThrowsConfigurationErrorsException()
        {
            var exception = Assert.Throws<ConfigurationErrorsException>(() =>
                Host.CreateDefaultBuilder()
                    .ConfigureServices(x =>
                        x.AddKVStores<object>().FromEtcd(null))
                    .Build());
            Assert.Equal("Configuration section '' is incorrect.", exception.Message);
        }

        [Fact]
        public void FromEtcd_ConfigurationSectionNameIsNullOrEmpty_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentException>(() =>
                Host.CreateDefaultBuilder()
                    .ConfigureServices((x, y) =>
                        y.AddKVStores<object>().FromEtcd(x.Configuration, ""))
                    .Build());
            Assert.Equal("'configurationSectionName' must not be null, empty or whitespace. (Parameter 'configurationSectionName')", exception.Message);
        }

        #endregion

        [Fact]
        public void AddFileDataProvider_ServiceCollectionIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => FileProvidersKVStoreBuilderExtensions.AddFileDataProvider(null));
            Assert.Equal("services", exception.ParamName);
        }

        [Fact]
        public void WithJsonSerialization_KVStoreBuilderIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => KVStoreBuilderExtensions.WithJsonSerialization<object>(null));
            Assert.Equal("builder", exception.ParamName);
        }
    }
}
