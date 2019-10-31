using System;
using System.IO;
using System.Linq;
using System.Net.Http;

namespace GiG.Core.Http.Security.Hmac.Extensions
{
    /// <summary>
    /// HttpRequestMessage extension methods for <see cref="HmacDelegatingHandler"/>
    /// </summary>
    public static class HttpRequestMessageExtensions
    {
        /// <summary>
        /// Gets the signature string to be used for HMAC hashing.
        /// </summary>
        /// <param name="httpRequest">the http request.</param>
        /// <param name="nonceHeader">header key to the Nonce value.</param>
        /// <param name="secret">the secret to use for hashing.</param>
        /// <returns>string to be used to HMAC hashing.</returns>
        public static async System.Threading.Tasks.Task<string> AsSignatureStringAsync(this HttpRequestMessage httpRequest, string nonceHeader, string secret)
        {
            var body = "";
            switch (httpRequest.Method.Method.ToUpper())
            {
                case "POST":
                case "PUT":
                case "PATCH":
                    body = await httpRequest.Content.ReadAsStringAsync();
                    break;

                default:
                    body = "";
                    break;
            }
            if (!httpRequest.Headers.TryGetValues(nonceHeader, out var nonceValue))
            {
                throw new ArgumentException("Nonce value is empty", "nonceHeader");
            }
            return $"{secret}{nonceValue.FirstOrDefault()}{httpRequest.Method.ToString().ToUpper()}{httpRequest.RequestUri.LocalPath}{body}";
        }
    }
}
