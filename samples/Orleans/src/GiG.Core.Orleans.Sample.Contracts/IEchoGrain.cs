using Orleans;
using System.Threading.Tasks;

namespace GiG.Core.Orleans.Sample.Contracts
{
    public interface IEchoGrain : IGrainWithStringKey
    {
        Task<string> Ping();
    }
}