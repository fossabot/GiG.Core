using GiG.Core.DistributedTracing.Abstractions;
using GiG.Core.Http;
using Refit;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GiG.Core.Samples.Http
{
    public interface IWalletRestClient
    {
        [Get("/wallets/balance")]
        Task<HttpResponseMessage> GetBalance();
    }
    
    public class CorrelationIdDelegatingHandler : DelegatingHandler
    {
        public CorrelationIdDelegatingHandler()
        {
            InnerHandler = new SocketsHttpHandler();
        }
        
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            if (!request.Headers.Contains(Constants.Header))
            {
                request.Headers.Add(Constants.Header, Guid.Empty.ToString());
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
    
    public class LoggingDelegatingHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            Console.WriteLine(request.RequestUri);

            return base.SendAsync(request, cancellationToken);
        }
    }

    internal static class Program
    {
        private static async Task Main()
        {
            var client = HttpClientFactory.Create(x =>
            {
                x.AddHttpMessageHandler(new LoggingDelegatingHandler());
                x.AddHttpMessageHandler(new CorrelationIdDelegatingHandler());
                x.BaseAddress = new Uri("http://pi-k3s-demo/");
            });
            
            var service = RestService.For<IWalletRestClient>(client);
            
            var result = await service.GetBalance();
            result.Headers.TryGetValues(Constants.Header, out var correlation);

            Console.WriteLine(correlation.First());        
        }
    }
}