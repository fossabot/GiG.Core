﻿using GiG.Core.DistributedTracing.Orleans.Extensions;
using System;
using Xunit;
// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.DistributedTracing.Tests.Unit.Orleans
{
    [Trait("Category", "Unit")]
    public class SiloBuilderExtensionsTests
    {
        [Fact]
        public void AddActivityIncomingFilter_SiloBuilderIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => SiloBuilderExtensions.AddActivityIncomingFilter(null));
            Assert.Equal("builder", exception.ParamName);
        }
        
        [Fact]
        public void AddActivityOutgoingFilter_SiloBuilderIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => SiloBuilderExtensions.AddActivityOutgoingFilter(null));
            Assert.Equal("builder", exception.ParamName);
        }
        
        [Fact]
        public void AddActivityFilters_SiloBuilderIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => SiloBuilderExtensions.AddActivityFilters(null));
            Assert.Equal("builder", exception.ParamName);
        }
    }
}
