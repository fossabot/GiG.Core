using GiG.Core.Orleans.Storage.Npgsql.Extensions;
using Microsoft.Extensions.Hosting;
using System;
using System.Configuration;
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
            var exception = Assert.Throws<ArgumentNullException>(() => SiloBuilderExtensions.AddNpgsqlGrainStorage(null, null, configuration: null));
            Assert.Equal("siloBuilder", exception.ParamName);

            exception = Assert.Throws<ArgumentNullException>(() => SiloBuilderExtensions.AddNpgsqlGrainStorage(null, null, null));
            Assert.Equal("siloBuilder", exception.ParamName);

            exception = Assert.Throws<ArgumentNullException>(() => SiloBuilderExtensions.AddNpgsqlGrainStorage(null, null, null, null));
            Assert.Equal("siloBuilder", exception.ParamName);
        }

        [Fact]
        public void AddNpgsqlGrainStorage_ConfigurationIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                            Host.CreateDefaultBuilder().UseOrleans(sb => sb.AddNpgsqlGrainStorage(null, configuration: null)).Build());
            Assert.Equal("configuration", exception.ParamName);

            exception = Assert.Throws<ArgumentNullException>(() =>
                            Host.CreateDefaultBuilder().UseOrleans(sb => sb.AddNpgsqlGrainStorage(storageName: null, null, null)).Build());
            Assert.Equal("configuration", exception.ParamName);
        }

        [Fact]
        public void AddNpgsqlGrainStorage_ConfigurationSectionIsNull_ThrowsConfigurationErrorsException()
        {
            var exception = Assert.Throws<ConfigurationErrorsException>(() =>
                            Host.CreateDefaultBuilder().UseOrleans(sb => sb.AddNpgsqlGrainStorage(null, null)).Build());
            Assert.Equal("Configuration section '' is incorrect.", exception.Message);
        }

        [Fact]
        public void AddNpgsqlGrainStorage_StorageNameIsNull_ThrowsArgumentException()
        {
            var exception = Assert.Throws<ArgumentException>(() => Host.CreateDefaultBuilder()
                .UseOrleans((ctx, x) =>
                {
                    x.AddNpgsqlGrainStorage("", ctx.Configuration, "CustomStorageSection");
                }).Build());
            Assert.Equal("'storageName' must not be null, empty or whitespace. (Parameter 'storageName')", exception.Message);
        }

        [Fact]
        public void AddNpgsqlGrainStorage_SectionNameIsNull_ThrowsArgumentException()
        {
            var exception = Assert.Throws<ArgumentException>(() => Host.CreateDefaultBuilder()
                .UseOrleans((ctx, x) =>
                {
                    x.AddNpgsqlGrainStorage("testDB2", ctx.Configuration, "");
                }).Build());
            Assert.Equal("'sectionName' must not be null, empty or whitespace. (Parameter 'sectionName')", exception.Message);
        }
    }
}
