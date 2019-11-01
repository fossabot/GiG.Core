using System;
using System.Security.Cryptography;
using System.Text;
using GiG.Core.Security.Cryptography;
using Xunit;

namespace GiG.Security.Cryptography.Tests.Unit
{
    [Trait("Category","Unit")]
    public class SHA256HashProviderTests
    {
        [Fact]
        public void SHA256SignatureProvider_HashString_ReturnsCorrectHash()
        {
            //Arrange
            var hashedString = "";
            using (var hash = SHA256.Create())
            {
                hashedString = Convert.ToBase64String(hash.ComputeHash(Encoding.UTF8.GetBytes("test")));
            }
            var hashProvider  = new SHA256HashProvider();

            //Act
            var signature = hashProvider.Hash("test");

            //Assert
            Assert.Equal(hashedString, signature);
        }
    }
}
