using GiG.Core.Hosting.Abstractions;
using GiG.Core.Hosting.Extensions;
using GiG.Core.Hosting.Tests.Integration.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xunit;

namespace GiG.Core.Hosting.Tests.Integration.Tests
{
    [Trait("Category", "Integration")]
    [CollectionDefinition("InfoManagementTests", DisableParallelization = true)]
    [Collection("InfoManagementTests")]
    public class ApplicationMetadataAccessorTests
    {
        private readonly IApplicationMetadataAccessor _applicationMetadataAccessor;

        public ApplicationMetadataAccessorTests()
        {
            var host = Host.CreateDefaultBuilder()
                           .UseApplicationMetadata()
                           .Build();

            _applicationMetadataAccessor = host.Services.GetRequiredService<IApplicationMetadataAccessor>();
        }

        [Fact]
        public void ApplicationMetadataAccessor_ApplicationName_ReturnsSameApplicationName()
        {
            //Act & Assert
            Assert.Equal(TestHelper.ApplicationName, ApplicationMetadata.Name);
            Assert.Equal(TestHelper.ApplicationName, _applicationMetadataAccessor.Name);
        }

        [Fact]
        public void ApplicationMetadataAccessor_Version_ReturnsSameVersion()
        {
            //Act & Assert
            Assert.Equal(TestHelper.Version, ApplicationMetadata.Version);
            Assert.Equal(TestHelper.Version, _applicationMetadataAccessor.Version);
        }

        [Fact]
        public void ApplicationMetadataAccessor_InformationalVersion_ReturnsSameInformationalVersion()
        {
            //Act & Assert
            Assert.Equal(TestHelper.InformationalVersion, ApplicationMetadata.InformationalVersion);
            Assert.Equal(TestHelper.InformationalVersion, _applicationMetadataAccessor.InformationalVersion);
        }
        
        [Fact]
        public void ApplicationMetadataAccessor_Checksum_ReturnsSameChecksum()
        {
            //Act & Assert
            Assert.Equal(TestHelper.Checksum, ApplicationMetadata.Checksum);
            Assert.Equal(TestHelper.Checksum, _applicationMetadataAccessor.Checksum);
        }
    }
}