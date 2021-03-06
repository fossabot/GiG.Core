﻿using GiG.Core.DistributedTracing.Orleans.Extensions;
using System;
using Xunit;

// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.DistributedTracing.Tests.Unit.Orleans
{
    [Trait("Category", "Unit")]
    public class ClientBuilderExtensionsTests
    {
        [Fact]
        public void AddActivityOutgoingFilter_ClientBuilderIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => ClientBuilderExtensions.AddActivityOutgoingFilter(null));
            Assert.Equal("builder", exception.ParamName);
        }
    }
}
