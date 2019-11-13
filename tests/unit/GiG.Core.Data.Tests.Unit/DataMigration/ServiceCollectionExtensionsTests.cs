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
            Assert.Throws<ArgumentNullException>(
                () => ServiceCollectionExtensions.AddDbMigration(null, null));
        }

        [Fact]
        public void AddDbMigration_DbConnectionIsNull__ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new ServiceCollection().AddDbMigration(null));
        }
    }
}
