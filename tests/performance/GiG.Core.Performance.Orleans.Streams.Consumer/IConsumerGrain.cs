using Orleans;
using System.Threading.Tasks;

namespace GiG.Core.Performance.Orleans.Streams.Consumer
{
    public interface IConsumerGrain : IGrainWithGuidKey
    {
        Task<int> GetCountersync();
    }
}