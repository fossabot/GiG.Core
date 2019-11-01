using GiG.Core.Http.Security.Hmac.Extensions;
using GiG.Core.Security.Cryptography;
using GiG.Core.Security.Http;
using System;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GiG.Core.Http.Security.Hmac
{
    /// <summary>
    /// <see cref="HmacDelegatingHandler"/> to handle Hmac header injection for Hmac Authentication.
    /// </summary>
    public class HmacDelegatingHandler : DelegatingHandler
    {
        private readonly IHmacOptionsProvider _optionsProvider;
        private readonly IHashProviderFactory _hashProviderFactory;
        private readonly IHmacSignatureProvider _signatureProvider;

        /// <summary>
        /// A <see cref="DelegatingHandler"/> that injects an HMAC Authorization Header into the request.
        /// </summary>
        /// <param name="optionsProvider">The <see cref="IHmacOptionsProvider"/> which should return <see cref="HmacOptions"/>.</param>
        /// <param name="hashProviderFactory"><see cref="IHashProviderFactory" /> that returns <see cref="IHashProvider" />.</param>
        /// <param name="signatureProvider">The <see cref="IHmacSignatureProvider" />.</param>
        public HmacDelegatingHandler(
            IHmacOptionsProvider optionsProvider,
            IHashProviderFactory hashProviderFactory,
            IHmacSignatureProvider signatureProvider)
        {
            _optionsProvider = optionsProvider;
            _hashProviderFactory = hashProviderFactory;
            _signatureProvider = signatureProvider;
        }
        /// <inheritdoc />
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var options = _optionsProvider.GetHmacOptions();
            if (options == null)
            {
                throw new ConfigurationErrorsException("Options not set for HMAC.");
            }

            if(!request.Headers.TryGetValues(HmacConstants.NonceHeader,out var nonceValue) || nonceValue.Count() == 0)
            {
                throw new ArgumentException($"Missing {HmacConstants.NonceHeader}.");
            }

            var hashProvider = _hashProviderFactory.GetHashProvider(options.HashAlgorithm);
            var hmacHeaderClear = _signatureProvider.GetSignature(request.Method.ToString().ToUpper(), request.RequestUri.LocalPath, await request.GetBodyAsync(), nonceValue.First(), options.Secret);//await request.AsSignatureStringAsync(HmacConstants.NonceHeader, options.Secret);

            var hashedHmacHeader = hashProvider.Hash(hmacHeaderClear);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("hmac", hashedHmacHeader);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
