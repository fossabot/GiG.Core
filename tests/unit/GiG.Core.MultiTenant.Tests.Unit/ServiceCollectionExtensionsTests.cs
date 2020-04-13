using System;
using Xunit;
// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.MultiTenant.Tests.Unit
{
    [Trait("Category", "Unit")]
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void AddActivityTenantAccessor_ServiceCollectionIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => Activity.Extensions.ServiceCollectionExtensions.AddActivityTenantAccessor(null));
            Assert.Equal("services", exception.ParamName);
        }
    }
}
