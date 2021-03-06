﻿using GiG.Core.Hosting.Extensions;
using System;
using Xunit;
// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.Hosting.Tests.Unit
{
    [Trait("Category", "Unit")]
    public class ApplicationBuilderExtensionsTests
    {
        [Fact]
        public void UseInfoManagement_ApplicationBuilderIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => ApplicationBuilderExtensions.UseInfoManagement(null));
            Assert.Equal("builder", exception.ParamName);
        }
    }
}
