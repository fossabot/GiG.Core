using GiG.Core.Orleans.Silo.Extensions;
using Microsoft.Extensions.Hosting;
using System;
using Xunit;
// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.Orleans.Tests.Unit.Silo
{
    [Trait("Category", "Unit")]
    public class SiloBuilderExtensionsTests
    {
        [Fact]
        public void AddAssemblies_SiloBuilderIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => SiloBuilderExtensions.AddAssemblies(null, assemblies: null));
            Assert.Equal("builder", exception.ParamName);

            exception = Assert.Throws<ArgumentNullException>(() => SiloBuilderExtensions.AddAssemblies(null, types: null));
            Assert.Equal("builder", exception.ParamName);
        }

        [Fact]
        public void AddAssemblies_AssembliesIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => Host.CreateDefaultBuilder()
                .UseOrleans((ctx, sb) =>
                {
                    sb.AddAssemblies(assemblies: null);
                }).Build());
            Assert.Equal("assemblies", exception.ParamName);
        }

        [Fact]
        public void AddAssemblies_TypesIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => Host.CreateDefaultBuilder()
                .UseOrleans((ctx, sb) =>
                {
                    sb.AddAssemblies(types: null);
                }).Build());
            Assert.Equal("types", exception.ParamName);
        }
    }
}
