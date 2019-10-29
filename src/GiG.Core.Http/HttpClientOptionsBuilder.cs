﻿using System;

namespace GiG.Core.Http
{
    /// <summary>
    /// HTTP Client Options Builder.
    /// </summary>
    public class HttpClientOptionsBuilder
    {
        /// <summary>
        /// Get or Set BaseAddress.
        /// </summary>
        public Uri BaseAddress { get; private set; }

        /// <summary>
        /// <summary>Sets the BaseAddress based on the specified base URI and relative URI string.</summary>
        /// </summary>
        /// <param name="baseUri">The base URI.</param>
        /// <param name="relativeUri">The relative URI to add to the base URI.</param>
        /// <returns>An <see cref="HttpClientOptionsBuilder"/> configured with predefined actions.</returns>
        /// <exception cref="ArgumentNullException">.</exception>
        public HttpClientOptionsBuilder WithBaseAddress(string baseUri, string relativeUri)
        {
            if (string.IsNullOrWhiteSpace(relativeUri)) throw new ArgumentNullException(nameof(relativeUri));

            BaseAddress = string.IsNullOrEmpty(baseUri)
                ? new Uri(relativeUri)
                : new Uri(new Uri(baseUri), relativeUri);

            return this;
        }
        
        /// <summary>
        /// Configures an <see cref="HttpClientOptionsBuilder"/> with base address.
        /// </summary>
        /// <param name="baseUri">Base Url passed from configuration</param>
        /// <returns>An <see cref="HttpClientOptionsBuilder"/> configured with predefined actions.</returns>
        /// <exception cref="ArgumentNullException">.</exception>
        public HttpClientOptionsBuilder WithBaseAddress(string baseUri)
        {
            if (string.IsNullOrWhiteSpace(baseUri)) throw new ArgumentNullException(nameof(baseUri));

            BaseAddress = new Uri(baseUri);

            return this;
        }
        
        /// <summary>
        /// Configures an <see cref="HttpClientOptionsBuilder"/> with base address.
        /// </summary>
        /// <param name="baseUri">Base Url passed from configuration</param>
        /// <returns>An <see cref="HttpClientOptionsBuilder"/> configured with predefined actions.</returns>
        /// <exception cref="ArgumentNullException">.</exception>
        public HttpClientOptionsBuilder WithBaseAddress(Uri baseUri)
        {
            if (baseUri == null) throw new ArgumentNullException(nameof(baseUri));

            BaseAddress = baseUri;

            return this;
        }
    }
}