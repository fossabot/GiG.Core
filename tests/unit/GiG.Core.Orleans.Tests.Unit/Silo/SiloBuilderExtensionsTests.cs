using GiG.Core.Orleans.Silo.Extensions;
using Microsoft.Extensions.Hosting;
using System;
using System.Configuration;
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
            Assert.Equal("siloBuilder", exception.ParamName);

            exception = Assert.Throws<ArgumentNullException>(() => SiloBuilderExtensions.AddAssemblies(null, types: null));
            Assert.Equal("siloBuilder", exception.ParamName);
        }

        [Fact]
        public void AddAssemblies_AssembliesIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => 
                            Host.CreateDefaultBuilder().UseOrleans(sb => sb.AddAssemblies(assemblies: null)).Build());
            Assert.Equal("assemblies", exception.ParamName);
        }

        [Fact]
        public void AddAssemblies_TypesIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => 
                            Host.CreateDefaultBuilder().UseOrleans(sb => sb.AddAssemblies(types: null)).Build());
            Assert.Equal("types", exception.ParamName);
        }

        [Fact]
        public void ConfigureCluster_SiloBuilderIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => SiloBuilderExtensions.ConfigureCluster(null, configuration: null));
            Assert.Equal("siloBuilder", exception.ParamName);

            exception = Assert.Throws<ArgumentNullException>(() => SiloBuilderExtensions.ConfigureCluster(null, null));
            Assert.Equal("siloBuilder", exception.ParamName);
        }

        [Fact]
        public void ConfigureCluster_ConfigurationIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => 
                            Host.CreateDefaultBuilder().UseOrleans(sb => sb.ConfigureCluster(configuration: null)).Build());
            Assert.Equal("configuration", exception.ParamName);
        }

        [Fact]
        public void ConfigureCluster_ConfigurationSectionIsNull_ThrowsConfigurationErrorsException()
        {
            var exception = Assert.Throws<ConfigurationErrorsException>(() => 
                            Host.CreateDefaultBuilder().UseOrleans(sb => sb.ConfigureCluster(null)).Build());
            Assert.Equal("Configuration section '' is incorrect.", exception.Message);
        }

        [Fact]
        public void ConfigureEndpoints_SiloBuilderIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => SiloBuilderExtensions.ConfigureEndpoints(null, configuration: null));
            Assert.Equal("siloBuilder", exception.ParamName);

            exception = Assert.Throws<ArgumentNullException>(() => SiloBuilderExtensions.ConfigureEndpoints(null, null));
            Assert.Equal("siloBuilder", exception.ParamName);
        }

        [Fact]
        public void ConfigureEndpoints_ConfigurationIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => 
                            Host.CreateDefaultBuilder().UseOrleans(sb => sb.ConfigureEndpoints(configuration: null)).Build());
            Assert.Equal("configuration", exception.ParamName);
        }
        
        [Fact]
        public void ConfigureDefaults_SiloBuilderIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => SiloBuilderExtensions.ConfigureDefaults(null, null));
            Assert.Equal("siloBuilder", exception.ParamName);
        }

        [Fact]
        public void ConfigureDefaults_ConfigurationIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                            Host.CreateDefaultBuilder().UseOrleans(sb => sb.ConfigureDefaults(null)).Build());
            Assert.Equal("configuration", exception.ParamName);
        }
    }
}
