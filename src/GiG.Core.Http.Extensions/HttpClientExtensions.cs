using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using System;
using System.Configuration;
using System.Net.Http;

namespace GiG.Core.Http.Extensions
{
    ///<summary>
    /// Http Client Extensions.
    /// </summary>
    public static class HttpClientExtensions
    {
        /// <summary>
        /// Use HttpClientOptions from configuration section.
        /// </summary>
        /// <param name="client">The <see cref="HttpClient"/>.</param>
        /// <param name="baseUri">Base Url of default HTTP client.</param>
        /// <param name="configurationSection">The <see cref="IConfigurationSection"/>.</param>
        /// <returns>An <see cref="HttpClient"/> configured with predefined actions.</returns>
        /// <exception cref="ArgumentNullException">.</exception>
        /// <exception cref="ConfigurationErrorsException">.</exception>
        public static HttpClient FromConfiguration([NotNull] this HttpClient client, string baseUri,
            [NotNull] IConfigurationSection configurationSection)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));
            if (configurationSection?.Exists() != true) throw new ConfigurationErrorsException($"Configuration Section '{configurationSection?.Path}' is incorrect.");
            if (string.IsNullOrWhiteSpace(baseUri)) throw new ArgumentException($"'{nameof(baseUri)}' must not be null, empty or whitespace.", nameof(baseUri));

            var httpClientOptions = configurationSection.Get<HttpClientOptions>();
            if (httpClientOptions == null)
            {
                throw new ConfigurationErrorsException($"Configuration section '{configurationSection?.Path}' does not exist.");
            }

            var options = new HttpClientOptionsBuilder();
            options.WithBaseAddress(baseUri, httpClientOptions.Url);
            client.BaseAddress = options.BaseAddress;

            return client;
        }

        /// <summary>
        /// Use HttpClientOptions from configuration.
        /// </summary>
        /// <param name="client">The <see cref="HttpClient"/>.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/>.</param>
        /// <param name="sectionName">Section Name for HttpClient Provider.</param>
        /// <returns>An <see cref="HttpClient"/> configured with predefined actions.</returns>
        public static HttpClient FromConfiguration([NotNull] this HttpClient client,
            [NotNull] IConfiguration configuration, string sectionName)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            if (string.IsNullOrWhiteSpace(sectionName)) throw new ArgumentException($"'{nameof(sectionName)}' must not be null, empty or whitespace.", nameof(sectionName));

            var defaultClientOptions =
                configuration.GetSection(DefaultClientOptions.DefaultSectionName).Get<DefaultClientOptions>() ??
                new DefaultClientOptions();

            return client.FromConfiguration(defaultClientOptions.BaseUrl, configuration.GetSection(sectionName));
        }
    }
}