using System;
using System.Security.Cryptography;
using System.Text;

namespace GiG.Core.Security.Cryptography
{
    public class SHA256HashProvider : IHashProvider
    {
        public string Name => "sha256";

        public string Hash(string message)
        {
            using (var hash = SHA256.Create())
            {
                return Convert.ToBase64String(hash.ComputeHash(Encoding.UTF8.GetBytes(message)));
            }
        }
    }
}
