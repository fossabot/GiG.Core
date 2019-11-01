using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace GiG.Core.Web.Security.Hmac.Extensions
{
    /// <summary>
    /// HttpRequest extension methods for <see cref="HmacAuthenticationHandler"/>
    /// </summary>
    public static class HttpRequestExtensions
    {
        /// <summary>
        /// Gets the signature string to be used to HMAC hashing.
        /// </summary>
        /// <param name="httpRequest">The <see cref="HttpRequest"/>.</param>
        /// <param name="nonceHeader">The header key to the Nonce value.</param>
        /// <param name="secret">The secret to use for hashing.</param>
        /// <returns>The string to be used to HMAC hashing.</returns>
        public static async System.Threading.Tasks.Task<string> AsSignatureStringAsync(this HttpRequest httpRequest, string nonceHeader, string secret)
        {
            string body;
            switch (httpRequest.Method)
            {
                case "POST":
                case "PUT":
                case "PATCH":
                    var bodyStream = new MemoryStream();
                    await httpRequest.Body.CopyToAsync(bodyStream);
                    bodyStream.Seek(0, SeekOrigin.Begin);

                    body = await new StreamReader(bodyStream).ReadToEndAsync();

                    bodyStream.Seek(0, SeekOrigin.Begin);
                    httpRequest.Body = bodyStream;
                    break;

                default:
                    body = "";
                    break;
            }
            if (!httpRequest.Headers.TryGetValue(nonceHeader, out var nonceValue))
            {
                throw new ArgumentException("Nonce value is empty",nameof(nonceHeader));
            }
            return $"{secret}{nonceValue}{httpRequest.Method}{httpRequest.Path}{body}";
        }
    }
}
