using Orleans;
using System.Threading.Tasks;

namespace GiG.Core.Performance.Orleans.Streams.Producer
{
    public interface IProducerGrain : IGrainWithGuidKey
    {
        Task ProduceAsync(string header, string body);
    }
}