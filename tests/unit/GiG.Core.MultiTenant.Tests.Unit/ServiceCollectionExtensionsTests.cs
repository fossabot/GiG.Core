using GiG.Core.MultiTenant.Web.Extensions;
using System;
using Xunit;
using Xunit.Sdk;

// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.MultiTenant.Tests.Unit
{
    [Trait("Category", "Unit")]
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void AddTenantAccessor_ServiceCollectionIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => ServiceCollectionExtensions.AddTenantAccessor(null));
            Assert.Equal("services", exception.ParamName);
        }

        [Fact]
        public void AddActivityTenantAccessor_ServiceCollectionIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => Activity.Extensions.ServiceCollectionExtensions.AddActivityTenantAccessor(null));
            Assert.Equal("services", exception.ParamName);
        }
    }
}
