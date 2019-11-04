using System.Net.Http;
using System.Threading.Tasks;

namespace GiG.Core.Http.Security.Hmac.Extensions
{
    /// <summary>
    /// HttpRequestMessage extension methods for <see cref="HmacDelegatingHandler"/>.
    /// </summary>
    public static class HttpRequestMessageExtensions
    {
        /// <summary>
        /// Gets the <see cref="HttpRequestMessage"/> body as string.
        /// </summary>
        /// <param name="httpRequest">The <see cref="HttpRequestMessage"/>.</param>
        /// <returns>The <see cref="HttpRequestMessage"/> body.</returns>
        public static async Task<string> GetBodyAsync(this HttpRequestMessage httpRequest)
        {
            if (httpRequest.Content == null)
            {
                return string.Empty;
            }

            return await httpRequest.Content.ReadAsStringAsync();
        }
    }
}
