using GiG.Core.Data.KVStores.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;
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
            var exception = Assert.Throws<ArgumentNullException>(() => ServiceCollectionExtensions.AddKVStores<object>(null, null));
            Assert.Equal("services", exception.ParamName);
        }
        
        [Fact]
        public void AddKVStores_ActionIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new ServiceCollection().AddKVStores<object>( null));
            Assert.Equal("configureKVStore", exception.ParamName);
        }
    }
}
