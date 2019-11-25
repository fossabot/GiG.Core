using GiG.Core.Orleans.Clustering.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Configuration;
using Xunit;

namespace GiG.Core.Orleans.Tests.Integration.Helpers
{
    public abstract class AbstractMembershipProviderTests
    {
        protected IHostBuilder HostBuilder { get; set; }
        
        [Fact]
        public void UseMembershipProvider_GetsProviderName_ThrowsConfigurationErrorsException()
        {
            //Arrange
            HostBuilder.ConfigureAppConfiguration(x => x.AddInMemoryCollection(new Dictionary<string, string>
            {
                { "Orleans:MembershipProvider:Name", string.Empty }
            }));

            //Act & Assert
            var exception = Assert.Throws<ConfigurationErrorsException>(() => HostBuilder.Build());

            Assert.Equal($"No Orleans Membership Provider was specified in the configuration section {MembershipProviderOptions.DefaultSectionName}", exception.Message);
        }
         
        [Fact]
        public void UseMembershipProvider_GetsProviderName_ThrowsConfigurationExceptionProviderRegistrationNotFound()
        {
            //Arrange
            HostBuilder.ConfigureAppConfiguration(x => x.AddInMemoryCollection(new Dictionary<string, string>
            {
                { "Orleans:MembershipProvider:Name", "NoProviderName" }
            }));

            //Act & Assert
            var exception = Assert.Throws<ConfigurationErrorsException>(() => HostBuilder.Build());

            Assert.Equal($"No Orleans Membership Providers were registered from the configuration section {MembershipProviderOptions.DefaultSectionName}", exception.Message);
        }        
    }
}