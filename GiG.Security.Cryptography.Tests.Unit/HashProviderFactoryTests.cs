using GiG.Core.Security.Cryptography;
using Moq;
using System;
using Xunit;

namespace GiG.Security.Cryptography.Tests.Unit
{
    public class HashProviderFactoryTests
    {
        private readonly Mock<IHashProvider> _signatureProvider;
        private readonly Func<string, IHashProvider> _factoryFunction;

        public HashProviderFactoryTests()
        {
            _signatureProvider = new Mock<IHashProvider>();
            _factoryFunction = new Func<string, IHashProvider>(x =>
            {
                if (x.Equals("sha256"))
                    return _signatureProvider.Object;
                else
                    return null;
            });
        }

        [Fact]
        public void SignatureProviderFactory_ExistingHash_ReturnsProvider()
        {
            var signatureFactory = new HashProviderFactory(_factoryFunction);

            var signatureProvider = signatureFactory.GetHashProvider("sha256");

            Assert.Equal(_signatureProvider.Object, signatureProvider);
        }

        [Fact]        
        public void SignatureProviderFactory_ExistingHash_ThrowsNotImplementedException()
        {
            var signatureFactory = new HashProviderFactory(_factoryFunction);

            var exception = Assert.Throws<NotImplementedException>(() => signatureFactory.GetHashProvider("sha512"));

            Assert.Equal("sha512 not implemented", exception.Message);
        }
    }
}
