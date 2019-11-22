using Moq;
using System;
using Xunit;

namespace GiG.Core.Security.Cryptography.Tests.Unit
{
    [Trait("Category", "Unit")]
    public class HashProviderFactoryTests
    {
        private readonly Mock<IHashProvider> _signatureProvider;
        private readonly Func<string, IHashProvider> _factoryFunction;

        public HashProviderFactoryTests()
        {
            _signatureProvider = new Mock<IHashProvider>();
            _factoryFunction = x => x.Equals("sha256") ? _signatureProvider.Object : null;
        }

        [Fact]
        public void SignatureProviderFactory_ExistingHash_ReturnsProvider()
        {
            //Arrange
            var signatureFactory = new HashProviderFactory(_factoryFunction);

            //Act
            var signatureProvider = signatureFactory.GetHashProvider("sha256");

            //Assert
            Assert.Equal(_signatureProvider.Object, signatureProvider);
        }

        [Fact]
        public void SignatureProviderFactory_ExistingHash_ThrowsNotImplementedException()
        {
            //Arrange
            var signatureFactory = new HashProviderFactory(_factoryFunction);

            //Act
            var exception = Assert.Throws<NotImplementedException>(() => signatureFactory.GetHashProvider("sha512"));

            //Assert
            Assert.Equal("sha512 not implemented", exception.Message);
        }
    }
}
