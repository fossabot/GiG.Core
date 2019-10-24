using System;
using System.Net.Http;

namespace GiG.Core.Http
{
    /// <summary>
    /// Factory Class for HttpClient.
    /// </summary>
    public static class HttpClientFactory
    {
        /// <summary>
        /// Creates an instance of <see cref="HttpClient"/>.
        /// </summary>
        /// <param name="configureHttpClientBuilder">A delegate that is used to configure an <see cref="HttpClientBuilder"/>.</param>
        /// <returns>An <see cref="HttpClient"/>.</returns>
        public static HttpClient CreateClient(Action<HttpClientBuilder> configureHttpClientBuilder = null)
        {
            var builder = new HttpClientBuilder();
            configureHttpClientBuilder?.Invoke(builder);

            var client = builder.DelegatingHandler != null
                ? new HttpClient(builder.DelegatingHandler)
                : new HttpClient();

            client.BaseAddress = builder.BaseAddress;

            return client;
        }
    }
}