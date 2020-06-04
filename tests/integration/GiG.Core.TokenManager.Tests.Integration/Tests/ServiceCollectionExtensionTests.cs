using GiG.Core.TokenManager.Abstractions.Interfaces;
using GiG.Core.TokenManager.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace GiG.Core.TokenManager.Tests.Integration.Tests
{
    public class ServiceCollectionExtensionTests
    {
        [Fact]
        public void AddTokenManagerFactory_SuccessfulRegistration_TokenManagerFactoryResolved()
        {
            // Arrange 
            var serviceCollection = new ServiceCollection();

            // Act
            serviceCollection.AddTokenManagerFactory();

            var serviceProvider = serviceCollection.BuildServiceProvider();
            ITokenManagerFactory tokenManagerFactory = null;
            var thrownException = Record.Exception(() => tokenManagerFactory = serviceProvider.GetService<ITokenManagerFactory>());

            // Assert
            Assert.Null(thrownException);        // Ensure all of TokenManagerFactory's dependencies were registered
            Assert.NotNull(tokenManagerFactory); // Ensure TokenManagerFactory was registered
        }
    }
}
