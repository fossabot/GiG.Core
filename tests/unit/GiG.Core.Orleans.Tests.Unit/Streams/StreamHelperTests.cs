using GiG.Core.Orleans.Streams.Abstractions;
using GiG.Core.Orleans.Streams.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace GiG.Core.Orleans.Tests.Unit.Streams
{
    [Trait("Category", "Unit")]
    public class StreamHelperTests
    {
        [Fact]
        public void GetNamespace_WithConfiguration_ReturnsNamespace()
        {
            // Act
            new ServiceCollection().ConfigureStream(new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build());
            
            // Assert
            Assert.Equal("test-prefix", StreamHelper.NamespacePrefix);
            Assert.Equal("test-prefix.param1", StreamHelper.GetNamespace("param1"));
            Assert.Equal("test-prefix.orleans.param1.param2.v1", StreamHelper.GetNamespace("param1", "param2"));
            Assert.Equal("test-prefix.orleans.param1.param2.v4", StreamHelper.GetNamespace("param1", "param2", 4));
            Assert.Equal("test-prefix.orleans.param1.param2", StreamHelper.GetNamespace("param1", "param2", null));
        }
        
        [Fact]
        public void GetNamespace_WithNullNamespacePrefix_ReturnsNamespace()
        {
            // Act
            new ServiceCollection().ConfigureStream(new ConfigurationBuilder().Build());

            // Assert
            Assert.Null(StreamHelper.NamespacePrefix);
            Assert.Equal("param1", StreamHelper.GetNamespace("param1"));
            Assert.Equal("orleans.param1.param2.v1", StreamHelper.GetNamespace("param1", "param2"));
            Assert.Equal("orleans.param1.param2.v4", StreamHelper.GetNamespace("param1", "param2", 4));
            Assert.Equal("orleans.param1.param2", StreamHelper.GetNamespace("param1", "param2", null));
        }
    }
}