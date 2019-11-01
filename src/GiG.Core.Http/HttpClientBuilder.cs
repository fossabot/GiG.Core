using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace GiG.Core.Http
{
    /// <summary>
    /// Http Client Builder. 
    /// </summary>
    public class HttpClientBuilder
    {
        private readonly List<DelegatingHandler> _delegatingHandlers = new List<DelegatingHandler>();

        internal DelegatingHandler[] DelegatingHandlers => _delegatingHandlers.ToArray();
        internal HttpMessageHandler MessageHandler { get; private set; } = new HttpClientHandler();
        
        /// <summary>
        /// The Base Address.
        /// </summary>
        public HttpClientOptionsBuilder Options { get; } = new HttpClientOptionsBuilder();

        /// <summary>
        /// Adds Delegating Handler.
        /// </summary>
        /// <param name="handler">The <see cref="DelegatingHandler"/>.</param>
        /// <returns>The <see cref="HttpClientBuilder"/>.</returns>
        public HttpClientBuilder AddDelegatingHandler([NotNull] DelegatingHandler handler)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));
            
            _delegatingHandlers.Add(handler);
            
            return this;
        }
        
        /// <summary>
        /// Adds HTTP Message Handler.
        /// </summary>
        /// <param name="handler">The <see cref="HttpMessageHandler"/>.</param>
        /// <returns>The <see cref="HttpClientBuilder"/>.</returns>
        public HttpClientBuilder WithMessageHandler([NotNull] HttpMessageHandler handler)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));
            
            MessageHandler = handler;
            
            return this;
        }
    }
}