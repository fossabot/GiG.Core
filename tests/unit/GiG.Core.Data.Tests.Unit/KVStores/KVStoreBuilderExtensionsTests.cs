using GiG.Core.Data.KVStores.Extensions;
using GiG.Core.Data.KVStores.Providers.Etcd.Extensions;
using GiG.Core.Data.KVStores.Providers.File.Extensions;
using Microsoft.Extensions.Hosting;
using System;
using System.Configuration;
using Xunit;
using FileProviderKVStoreBuilderExtensions = GiG.Core.Data.KVStores.Providers.File.Extensions.KVStoreBuilderExtensions;
using EtcdProviderKVStoreBuilderExtensions = GiG.Core.Data.KVStores.Providers.Etcd.Extensions.KVStoreBuilderExtensions;
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
        public void FromFile_KVStoreBuilderIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => FileProviderKVStoreBuilderExtensions.FromFile<object>(null, null, ""));
            Assert.Equal("builder", exception.ParamName);
        }

        [Fact]
        public void FromFile_WithConfigurationSection_KVStoreBuilderIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => FileProviderKVStoreBuilderExtensions.FromFile<object>(null, null));
            Assert.Equal("builder", exception.ParamName);
        }

        [Fact]
        public void FromFile_ConfigurationIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                Host.CreateDefaultBuilder()
                    .ConfigureServices(x =>
                        x.AddKVStores<object>(p => p.FromFile(null, "")))
                    .Build());
            Assert.Equal("configuration", exception.ParamName);
        }

        [Fact]
        public void FromFile_ConfigurationSectionIsNull_ThrowsConfigurationErrorsException()
        {
            var exception = Assert.Throws<ConfigurationErrorsException>(() =>
                Host.CreateDefaultBuilder()
                    .ConfigureServices(x =>
                        x.AddKVStores<object>(p => p.FromFile(null)))
                    .Build());
            Assert.Equal("Configuration section '' is incorrect.", exception.Message);
        }

        [Fact]
        public void FromFile_ConfigurationSectionNameIsNullOrEmpty_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentException>(() =>
                Host.CreateDefaultBuilder()
                    .ConfigureServices((x, y) =>
                        y.AddKVStores<object>(p => p.FromFile(x.Configuration, "")))
                    .Build());
            Assert.Equal("'configurationSectionName' must not be null, empty or whitespace. (Parameter 'configurationSectionName')", exception.Message);
        }
        
         [Fact]
        public void FromEtcd_KVStoreBuilderIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => EtcdProviderKVStoreBuilderExtensions.FromEtcd<object>(null, null, ""));
            Assert.Equal("builder", exception.ParamName);
        }

        [Fact]
        public void FromEtcd_WithConfigurationSection_KVStoreBuilderIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => EtcdProviderKVStoreBuilderExtensions.FromEtcd<object>(null, null));
            Assert.Equal("builder", exception.ParamName);
        }

        [Fact]
        public void FromEtcd_ConfigurationIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                Host.CreateDefaultBuilder()
                    .ConfigureServices(x =>
                        x.AddKVStores<object>(p => p.FromEtcd(null, "")))
                    .Build());
            Assert.Equal("configuration", exception.ParamName);
        }

        [Fact]
        public void FromEtcd_ConfigurationSectionIsNull_ThrowsConfigurationErrorsException()
        {
            var exception = Assert.Throws<ConfigurationErrorsException>(() =>
                Host.CreateDefaultBuilder()
                    .ConfigureServices(x =>
                        x.AddKVStores<object>(p => p.FromEtcd(null)))
                    .Build());
            Assert.Equal("Configuration section '' is incorrect.", exception.Message);
        }

        [Fact]
        public void FromEtcd_ConfigurationSectionNameIsNullOrEmpty_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentException>(() =>
                Host.CreateDefaultBuilder()
                    .ConfigureServices((x, y) =>
                        y.AddKVStores<object>(p => p.FromEtcd(x.Configuration, "")))
                    .Build());
            Assert.Equal("'configurationSectionName' must not be null, empty or whitespace. (Parameter 'configurationSectionName')", exception.Message);
        }
    }
}
