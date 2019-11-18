using GiG.Core.Orleans.Storage.Npgsql.Extensions;
using Microsoft.Extensions.Hosting;
using System;
using Xunit;
// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.Orleans.Tests.Unit.Storage
{
    [Trait("Category", "Unit")]
    public class SiloBuilderExtensionsTests
    {
        [Fact]
        public void AddNpgsqlGrainStorage_SiloBuilderIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => SiloBuilderExtensions.AddNpgsqlGrainStorage(null, storageName: null, configuration: null));
            Assert.Equal("builder", exception.ParamName);

            exception = Assert.Throws<ArgumentNullException>(() => SiloBuilderExtensions.AddNpgsqlGrainStorage(null, storageName: null, configurationSection: null));
            Assert.Equal("builder", exception.ParamName);

            exception = Assert.Throws<ArgumentNullException>(() => SiloBuilderExtensions.AddNpgsqlGrainStorage(null, storageName: null, configuration: null, sectionName: null));
            Assert.Equal("builder", exception.ParamName);
        }

        [Fact]
        public void AddNpgsqlGrainStorage_ConfigurationIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                            Host.CreateDefaultBuilder().UseOrleans(sb => sb.AddNpgsqlGrainStorage(storageName: null, configuration: null)).Build());
            Assert.Equal("configuration", exception.ParamName);

            exception = Assert.Throws<ArgumentNullException>(() =>
                            Host.CreateDefaultBuilder().UseOrleans(sb => sb.AddNpgsqlGrainStorage(storageName: null, configuration: null, sectionName: null)).Build());
            Assert.Equal("configuration", exception.ParamName);
        }

        [Fact]
        public void AddNpgsqlGrainStorage_ConfigurationSectionIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                            Host.CreateDefaultBuilder().UseOrleans(sb => sb.AddNpgsqlGrainStorage(storageName: null, configurationSection: null)).Build());
            Assert.Equal("configurationSection", exception.ParamName);
        }

        [Fact]
        public void AddNpgsqlGrainStorage_StorageNameIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentException>(() => Host.CreateDefaultBuilder()
                .UseOrleans((ctx, x) =>
                {
                    x.AddNpgsqlGrainStorage("", ctx.Configuration);
                }).Build());
            Assert.Equal("Missing storageName.", exception.Message);

            exception = Assert.Throws<ArgumentException>(() => Host.CreateDefaultBuilder()
                .UseOrleans((ctx, x) =>
                {
                    x.AddNpgsqlGrainStorage("", ctx.Configuration, "CustomStorageSection");
                }).Build());
            Assert.Equal("Missing storageName.", exception.Message);
        }

        [Fact]
        public void AddNpgsqlGrainStorage_SectionNameIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentException>(() => Host.CreateDefaultBuilder()
                .UseOrleans((ctx, x) =>
                {
                    x.AddNpgsqlGrainStorage("testDB2", ctx.Configuration, "");
                }).Build());
            Assert.Equal("Missing sectionName.", exception.Message);
        }
    }
}
