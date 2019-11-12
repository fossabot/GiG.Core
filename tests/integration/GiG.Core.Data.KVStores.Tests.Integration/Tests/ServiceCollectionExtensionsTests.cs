using GiG.Core.Data.KVStores.Extensions;
using System;
using Xunit;
// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.Data.KVStores.Tests.Integration.Tests
{
    [Trait("Category", "Integration")]
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void AddKVStores_ServiceCollectionIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(
                () => ServiceCollectionExtensions.AddKVStores<object>(null));
        }
    }
}
