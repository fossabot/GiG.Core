using GiG.Core.Orleans.Sample.Contracts;
using Orleans;
using System;
using System.Threading.Tasks;

namespace GiG.Core.Orleans.Sample.Grains
{
    public class EchoGrain : Grain, IEchoGrain
    {
        public async Task<string> Ping()
        {
            var grainName = Environment.GetEnvironmentVariable("Echo_Grain_Name");

            return await Task.FromResult(string.Format("Hello from {0}", grainName));
        }
    }
}