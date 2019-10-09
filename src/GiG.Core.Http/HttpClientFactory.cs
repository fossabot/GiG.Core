using System;
using System.Net.Http;

namespace GiG.Core.Http
{
    /// <summary>
    /// Factory for HttpClient.
    /// </summary>
    public static class HttpClientFactory
    {
        /// <summary>
        /// Creates a new <see cref="HttpClient"/> with predefined actions.
        /// </summary>
        /// <param name="configureClient">A delegate that is used to configure an <see cref="HttpClientBuilder"/>.</param>
        /// <returns>An <see cref="HttpClient"/> configured with predefined actions.</returns>
        public static HttpClient Create(Action<HttpClientBuilder> configureClient = null)
        {
            var builder = new HttpClientBuilder();
            configureClient?.Invoke(builder);

            var client = builder.DelegatingHandler != null
                ? new HttpClient(builder.DelegatingHandler)
                : new HttpClient();

            client.BaseAddress = builder.BaseAddress;

            return client;
        }
    }
}
