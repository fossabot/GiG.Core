using System;

namespace GiG.Core.Security.Cryptography
{
    /// <inheritdoc />
    public class HashProviderFactory : IHashProviderFactory
    {
        private readonly Func<string, IHashProvider> _signatureProviderFactory;

        /// <inheritdoc />
        public HashProviderFactory(Func<string, IHashProvider> signatureProviderFactory)
        {
            _signatureProviderFactory = signatureProviderFactory;
        }

        /// <inheritdoc />
        public IHashProvider GetHashProvider(string hashAlgorithm)
        {
            var signatureProvider = _signatureProviderFactory(hashAlgorithm);

            if (signatureProvider == null)
            {
                throw new NotImplementedException($"{hashAlgorithm} not implemented");
            }

            return signatureProvider;
        }
    }
}
