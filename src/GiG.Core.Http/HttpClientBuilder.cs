using System;
using System.Net.Http;

namespace GiG.Core.Http
{
    /// <summary>
    /// Http Client Builder. 
    /// </summary>
    public class HttpClientBuilder
    {
        internal DelegatingHandler DelegatingHandler { get; private set; }

        private DelegatingHandler _lastDelegatingHandler;
        
        /// <summary>
        /// The Base Address.
        /// </summary>
        public Uri BaseAddress { get; set; }

        /// <summary>
        /// Adds Http Message Handler.
        /// </summary>
        /// <param name="handler">The <see cref="System.Net.Http.DelegatingHandler"/>.</param>
        /// <returns>The <see cref="HttpClientBuilder"/>.</returns>
        public HttpClientBuilder AddHttpMessageHandler(DelegatingHandler handler)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));
            
            // Set Default Handler if not set
            if (handler.InnerHandler == null)
            {
                handler.InnerHandler = new HttpClientHandler();
            }

            // Combine Delegate Handlers
            if (DelegatingHandler == null)
            {
                _lastDelegatingHandler = DelegatingHandler = handler;
            }
            else
            {
                _lastDelegatingHandler.InnerHandler = handler;
            }

            _lastDelegatingHandler = handler;

            return this;
        }
    }
}