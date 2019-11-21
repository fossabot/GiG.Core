﻿using GiG.Core.Context.Orleans.Extensions;
using System;
using Xunit;
// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.Context.Tests.Unit.Orleans
{
    [Trait("Category", "Unit")]
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void AddRequestContextAccessor_ServiceCollectionIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => ServiceCollectionExtensions.AddRequestContextAccessor(null));
            Assert.Equal("services", exception.ParamName);
        }
    }
}
