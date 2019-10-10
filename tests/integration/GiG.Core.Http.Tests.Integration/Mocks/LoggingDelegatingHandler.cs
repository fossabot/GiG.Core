using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GiG.Core.Http.Tests.Integration.Mocks
{
    internal class LoggingDelegatingHandler : DelegatingHandler
    {
        public LoggingDelegatingHandler(HttpMessageHandler innerHandler)
        {
            InnerHandler = innerHandler;
        }
        
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            Console.WriteLine(request.RequestUri);

            return base.SendAsync(request, cancellationToken);
        }
    }
}
