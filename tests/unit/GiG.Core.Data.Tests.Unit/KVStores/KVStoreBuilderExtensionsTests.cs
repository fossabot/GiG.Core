using GiG.Core.Data.KVStores.Extensions;
using GiG.Core.Data.KVStores.Providers.FileProviders.Extensions;
using Microsoft.Extensions.Hosting;
using System;
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
            Assert.Throws<ArgumentNullException>(
                () => KVStoreBuilderExtensions.AddMemoryDataStore<object>(null));
        }

        [Fact]
        public void FromJsonFile_KVStoreBuilderIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(
                () => FileProvidersKVStoreBuilderExtensions.FromJsonFile<object>(null, null, ""));
        }

        [Fact]
        public void FromJsonFileWithConfigurationSection_KVStoreBuilderIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(
                () => FileProvidersKVStoreBuilderExtensions.FromJsonFile<object>(null, null));
        }

        [Fact]
        public void FromJsonFile_ConfigurationIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                Host.CreateDefaultBuilder()
                    .ConfigureServices(x =>
                        x.AddKVStores<object>().FromJsonFile(null, ""))
                    .Build());
        }

        [Fact]
        public void FromJsonFile_ConfigurationSectionIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                Host.CreateDefaultBuilder()
                    .ConfigureServices(x =>
                        x.AddKVStores<object>().FromJsonFile(null))
                    .Build());
        }

        [Fact]
        public void FromJsonFile_ConfigurationSectionNameIsNullOrEmpty_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentException>(() =>
                Host.CreateDefaultBuilder()
                    .ConfigureServices((x, y) =>
                        y.AddKVStores<object>().FromJsonFile(x.Configuration, ""))
                    .Build());
        }

        [Fact]
        public void AddFileDataProvider_ServiceCollectionIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(
                () => FileProvidersKVStoreBuilderExtensions.AddFileDataProvider(null));
        }
    }
}
