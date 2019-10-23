using Orleans;
using System.Threading.Tasks;

namespace GiG.Core.Benchmarks.Orleans.Streams.Contracts
{
    public interface IProducerGrain : IGrainWithGuidKey
    {
        Task ProduceAsync(string header, string body);
    }
}