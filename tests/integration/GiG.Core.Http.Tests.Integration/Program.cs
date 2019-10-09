using GiG.Core.DistributedTracing.Abstractions;
using GiG.Core.Http;
using GiG.Core.Http.DistributedTracing;
using GiG.Core.Http.MultiTenant;
using Refit;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GiG.Core.Http.Tests.Integration
{
    internal static class Program
    {
        private static async Task Main()
        {
            var client = HttpClientFactory.Create(x =>
            {
                x.AddHttpMessageHandler(new LoggingDelegatingHandler());
                x.AddHttpMessageHandler(new CorrelationIdDelegatingHandler(new CorrelationContextAccessor()));
                x.AddHttpMessageHandler(new TenantDelegatingHandler(new TenantAccessor()));
                x.BaseAddress = new Uri("http://pi-k3s-demo/");
            });
            
            var service = RestService.For<IWalletRestClient>(client);
            
            var result = await service.GetBalance();
            result.Headers.TryGetValues(Constants.Header, out var correlation);

            Console.WriteLine(correlation.First());        
        }
    }
}