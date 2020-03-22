﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;
// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.DistributedTracing.OpenTelemetry.Tests.Unit.Tests
{
    [Trait("Category", "Unit")]
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void AddTracing_ServiceCollectionIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => ServiceCollectionExtensions.AddTracing(null, null, null));
            Assert.Equal("services", exception.ParamName);
        }
        
        [Fact]
        public void AddTracing_ConfigurationIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new ServiceCollection().AddTracing(null, null));
            Assert.Equal("configuration", exception.ParamName);
        }
        
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void AddTracing_SectionNameIsInvalid_ThrowsArgumentException(string sectionName)
        {
            var exception = Assert.Throws<ArgumentException>(() => new ServiceCollection().AddTracing(null, new ConfigurationBuilder().Build(), sectionName));
            Assert.Equal("sectionName", exception.ParamName);
        }
    }
}
