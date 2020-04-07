using GiG.Core.Orleans.Streams.Extensions;
using System;
using Xunit;
// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.Orleans.Tests.Unit.Streams
{
    [Trait("Category", "Unit")]
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void AddStreamFactory_ServiceCollectionIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => ServiceCollectionExtensions.AddStream(null));
            Assert.Equal("services", exception.ParamName);
        }
        
        [Fact]
        public void AddCommonDispatcherFactory_ServiceCollectionIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => ServiceCollectionExtensions.AddCommandDispatcher(null));
            Assert.Equal("services", exception.ParamName);
        }

        [Fact]
        public void AddStreamProvider_ServiceCollectionIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => ServiceCollectionExtensions.AddStreamProvider(null));
            Assert.Equal("services", exception.ParamName);
        }
    }
}