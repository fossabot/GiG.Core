using GiG.Core.Orleans.Reminders.Npgsql.Extensions;
using Microsoft.Extensions.Hosting;
using System;
using System.Configuration;
using Xunit;
// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.Orleans.Tests.Unit.Reminders
{
    [Trait("Category", "Unit")]
    public class SiloBuilderExtensionsTests
    {
        [Fact]
        public void UseNpgsqlReminderService_SiloBuilderIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => SiloBuilderExtensions.UseNpgsqlReminderService(null, null, configuration: null));
            Assert.Equal("builder", exception.ParamName);

            exception = Assert.Throws<ArgumentNullException>(() => SiloBuilderExtensions.UseNpgsqlReminderService(null, null, null));
            Assert.Equal("builder", exception.ParamName);

            exception = Assert.Throws<ArgumentNullException>(() => SiloBuilderExtensions.UseNpgsqlReminderService(null, null, null, null));
            Assert.Equal("builder", exception.ParamName);
        }

        [Fact]
        public void UseNpgsqlReminderService_ConfigurationIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                            Host.CreateDefaultBuilder().UseOrleans(sb => sb.UseNpgsqlReminderService(null, configuration: null)).Build());
            Assert.Equal("configuration", exception.ParamName);

            exception = Assert.Throws<ArgumentNullException>(() =>
                            Host.CreateDefaultBuilder().UseOrleans(sb => sb.UseNpgsqlReminderService(storageName: null, null, null)).Build());
            Assert.Equal("configuration", exception.ParamName);
        }

        [Fact]
        public void UseNpgsqlReminderService_ConfigurationSectionIsNull_ThrowsConfigurationErrorsException()
        {
            var exception = Assert.Throws<ConfigurationErrorsException>(() =>
                            Host.CreateDefaultBuilder().UseOrleans(sb => sb.UseNpgsqlReminderService(null, null)).Build());
            Assert.Equal("Configuration section '' is incorrect.", exception.Message);
        }

        [Fact]
        public void UseNpgsqlReminderService_StorageNameIsNull_ThrowsArgumentException()
        {
            var exception = Assert.Throws<ArgumentException>(() => Host.CreateDefaultBuilder()
                .UseOrleans((ctx, x) =>
                {
                    x.UseNpgsqlReminderService("", ctx.Configuration, "CustomStorageSection");
                }).Build());
            Assert.Equal("'storageName' must not be null, empty or whitespace. (Parameter 'storageName')", exception.Message);
        }

        [Fact]
        public void UseNpgsqlReminderService_SectionNameIsNull_ThrowsArgumentException()
        {
            var exception = Assert.Throws<ArgumentException>(() => Host.CreateDefaultBuilder()
                .UseOrleans((ctx, x) =>
                {
                    x.UseNpgsqlReminderService("testDB2", ctx.Configuration, "");
                }).Build());
            Assert.Equal("'sectionName' must not be null, empty or whitespace. (Parameter 'sectionName')", exception.Message);
        }
    }
}
