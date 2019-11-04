using System;
using System.Security.Cryptography;
using System.Text;

namespace GiG.Core.Security.Cryptography
{
    /// <summary>
    /// SHA256 implementation for <see cref="IHashProvider"/>.
    /// </summary>
    public class SHA256HashProvider : IHashProvider
    {
        /// <inheritdoc />
        public string Name => "sha256";

        /// <inheritdoc />
        public string Hash(string message)
        {
            using (var hash = SHA256.Create())
            {
                return Convert.ToBase64String(hash.ComputeHash(Encoding.UTF8.GetBytes(message)));
            }
        }
    }
}
