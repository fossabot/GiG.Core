using System;
using System.Net.Http;

namespace GiG.Core.Http
{
    public static class HttpClientFactory
    {
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
