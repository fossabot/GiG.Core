using GiG.Core.Data.Migration.Evolve.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;
// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.Data.Tests.Unit.DataMigration
{
    [Trait("Category", "Unit")]
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void AddDbMigration_ServiceCollectionIsNull__ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => ServiceCollectionExtensions.AddDbMigration(null, null));
            Assert.Equal("services", exception.ParamName);
        }

        [Fact]
        public void AddDbMigration_DbConnectionIsNull__ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new ServiceCollection().AddDbMigration(null));
            Assert.Equal("dbConnection", exception.ParamName);
        }
    }
}
