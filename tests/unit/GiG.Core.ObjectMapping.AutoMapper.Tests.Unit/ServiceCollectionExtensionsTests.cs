using GiG.Core.ObjectMapping.AutoMapper.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;
// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.ObjectMapping.AutoMapper.Tests.Unit
{
    [Trait("Category", "Unit")]
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void AddObjectMapper_ServiceCollectionIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => ServiceCollectionExtensions.AddObjectMapper(null, assemblies: null));
            Assert.Equal("services", exception.ParamName);

            exception = Assert.Throws<ArgumentNullException>(() => ServiceCollectionExtensions.AddObjectMapper(null, profileAssemblyMarkerTypes: null));
            Assert.Equal("services", exception.ParamName);
        }

        [Fact]
        public void AddObjectMapper_AssembliesIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentException>(() => new ServiceCollection().AddObjectMapper(assemblies: null));
            Assert.Equal("Missing assemblies.", exception.Message);
        }

        [Fact]
        public void AddObjectMapper_ProfileAssemblyMarkerTypesIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentException>(() => new ServiceCollection().AddObjectMapper(profileAssemblyMarkerTypes: null));
            Assert.Equal("Missing profileAssemblyMarkerTypes.", exception.Message);
        }
    }
}
