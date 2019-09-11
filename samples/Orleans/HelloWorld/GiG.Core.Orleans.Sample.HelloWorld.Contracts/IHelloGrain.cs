using System.Threading.Tasks;
using Orleans;

namespace GiG.Core.Orleans.Sample.HelloWorld.Contracts
{
    public interface IHelloGrain : IGrainWithStringKey
    {
        Task<string> SayHelloAsync();
    }
}