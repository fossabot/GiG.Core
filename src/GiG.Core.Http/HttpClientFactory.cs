using JetBrains.Annotations;
using System;
using System.Collections.Concurrent;
using System.Net.Http;

namespace GiG.Core.Http
{
    /// <summary>
    /// Factory Class for HttpClient.
    /// </summary>
    public class HttpClientFactory : IDisposable
    {
        private static readonly ConcurrentDictionary<string, HttpClient> Instances =
            new ConcurrentDictionary<string, HttpClient>();

        private bool _isDisposing;

        /// <summary>
        /// Creates an instance of <see cref="HttpClient"/>.
        /// </summary>
        /// <param name="configureHttpClientBuilder">A delegate that is used to configure an <see cref="HttpClientBuilder"/>.</param>
        /// <returns>An <see cref="HttpClient"/>.</returns>
        public static HttpClient Create(Action<HttpClientBuilder> configureHttpClientBuilder = null)
        {
            var builder = new HttpClientBuilder();
            configureHttpClientBuilder?.Invoke(builder);

            var client = System.Net.Http.HttpClientFactory.Create(builder.MessageHandler, builder.DelegatingHandlers);

            client.BaseAddress = builder.Options.BaseAddress;

            return client;
        }

        /// <summary>
        /// Creates or Gets an instance of <see cref="HttpClient"/> according to the name.
        /// </summary>
        /// <param name="name">Name of instance for the <see cref="HttpClient"/>.</param>
        /// <param name="configureHttpClientBuilder">A delegate that is used to configure an <see cref="HttpClientBuilder"/>.</param>
        /// <returns>An <see cref="HttpClient"/>.</returns>
        public static HttpClient GetOrAdd([NotNull] string name,
            Action<HttpClientBuilder> configureHttpClientBuilder = null)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException(nameof(name));

            return Instances.GetOrAdd(name, Create(configureHttpClientBuilder));
        }
        
        /// <summary>
        /// Creates or Gets an instance of <see cref="HttpClient"/> according to the type.
        /// </summary>
        /// <param name="configureHttpClientBuilder">A delegate that is used to configure an <see cref="HttpClientBuilder"/>.</param>
        /// <returns>An <see cref="HttpClient"/>.</returns>
        public static HttpClient GetOrAdd<T>(Action<HttpClientBuilder> configureHttpClientBuilder = null)
        {
            var type = typeof(T);

            return GetOrAdd($"AutoGenerated-{type.FullName ?? type.Name}", configureHttpClientBuilder);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (_isDisposing)
            {
                return;
            }

            _isDisposing = true;
            foreach (var client in Instances.Values)
            {
                client?.Dispose();
            }
        }
    }
}