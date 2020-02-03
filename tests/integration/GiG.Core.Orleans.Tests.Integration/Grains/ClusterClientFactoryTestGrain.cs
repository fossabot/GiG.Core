using GiG.Core.Orleans.Tests.Integration.Contracts;
using Microsoft.Extensions.Options;
using Orleans;
using Orleans.Configuration;
using System.Threading.Tasks;

namespace GiG.Core.Orleans.Tests.Integration.Grains
{
    public class ClusterClientFactoryTestGrain : Grain, IClusterClientFactoryTestGrain
    {
        public Task<string> GetSiloNameAsync()
        {
            var siloOptions = ServiceProvider.GetService(typeof(IOptions<SiloOptions>)) as IOptions<SiloOptions>;
            return Task.FromResult(siloOptions?.Value.SiloName);
        }
    }
}