using System;
using System.Net.Http;

namespace GiG.Core.Http
{
    public class HttpClientBuilder
    {
        internal DelegatingHandler DelegatingHandler { get; private set; }

        private DelegatingHandler _lastDelegatingHandler;
        
        public Uri BaseAddress { get; set; }
        
        public HttpClientBuilder AddHttpMessageHandler(DelegatingHandler handler)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));

            if (handler.InnerHandler == null)
            {
                handler.InnerHandler = new HttpClientHandler();
            }

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