using System.Threading.Tasks;
using GiG.Core.Orleans.Sample.HelloWorld.Contracts;
using Orleans;

namespace GiG.Core.Orleans.Samples.HelloWorld.Grains
{
    public class HelloGrain : Grain, IHelloGrain
    {
        public Task<string> SayHelloAsync()
        {
            return Task.FromResult("Hello!");
        }
    }
}
