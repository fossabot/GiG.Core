﻿using GiG.Core.Http.Authentication.Hmac.Extensions;
using GiG.Core.Security.Cryptography;
using GiG.Core.Authentication.Hmac.Abstractions;
using System;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;

namespace GiG.Core.Http.Authentication.Hmac
{
    /// <summary>
    /// <see cref="HmacDelegatingHandler"/> to handle Hmac header injection for Hmac Authentication.
    /// </summary>
    public class HmacDelegatingHandler : DelegatingHandler
    {
        private readonly IOptions<HmacOptions> _options;
        private readonly IHashProviderFactory _hashProviderFactory;
        private readonly IHmacSignatureProvider _signatureProvider;

        /// <summary>
        /// A <see cref="DelegatingHandler"/> that injects an HMAC Authorization Header into the request.
        /// </summary>
        /// <param name="options">The <see cref="IOptions{TOptions}"/> which should return <see cref="HmacOptions"/>.</param>
        /// <param name="hashProviderFactory"><see cref="IHashProviderFactory" /> that returns <see cref="IHashProvider" />.</param>
        /// <param name="signatureProvider">The <see cref="IHmacSignatureProvider" />.</param>
        public HmacDelegatingHandler(
            IOptions<HmacOptions> options,
            IHashProviderFactory hashProviderFactory,
            IHmacSignatureProvider signatureProvider)
        {
            _options = options;
            _hashProviderFactory = hashProviderFactory;
            _signatureProvider = signatureProvider;
        }

        /// <inheritdoc />
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var options = _options.Value;
            if (options == null)
            {
                throw new ConfigurationErrorsException("Options not set for HMAC.");
            }

            if (!request.Headers.TryGetValues(Constants.Nonce, out var nonceValue) || !nonceValue.Any())
            {
                throw new ArgumentException($"'{Constants.Nonce}' must not be null or empty.", Constants.Nonce);
            }

            var hashProvider = _hashProviderFactory.GetHashProvider(options.HashAlgorithm);
            var hmacHeaderClear = _signatureProvider.GetSignature(request.Method.ToString().ToUpper(), request.RequestUri.LocalPath, await request.GetBodyAsync(), nonceValue.First(), options.Secret);

            var hashedHmacHeader = hashProvider.Hash(hmacHeaderClear);
            request.Headers.Authorization = new AuthenticationHeaderValue("hmac", hashedHmacHeader);
            
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
