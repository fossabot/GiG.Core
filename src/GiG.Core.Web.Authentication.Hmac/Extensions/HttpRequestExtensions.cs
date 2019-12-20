using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using GiG.Core.Web.Authentication.Hmac.Internal;

namespace GiG.Core.Web.Authentication.Hmac.Extensions
{
    /// <summary>
    /// HttpRequest extension methods for <see cref="HmacAuthenticationHandler"/>.
    /// </summary>
    public static class HttpRequestExtensions
    {
        /// <summary>
        /// Gets the <see cref="HttpRequest"/> body.
        /// </summary>
        /// <param name="httpRequest">The <see cref="HttpRequest"/>.</param>
        /// <returns>The <see cref="HttpRequest"/> body.</returns>
        public static async Task<string> GetBodyAsync(this HttpRequest httpRequest)
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

                    body = await new StreamReader(bodyStream).ReadToEndAsync(); //StreamReader cannot be disposed because it would dispose the HttpRequest Stream

                    bodyStream.Seek(0, SeekOrigin.Begin);
                    httpRequest.Body = bodyStream;
                    break;

                default:
                    body = "";
                    break;
            }
            return body;
        }
    }
}
