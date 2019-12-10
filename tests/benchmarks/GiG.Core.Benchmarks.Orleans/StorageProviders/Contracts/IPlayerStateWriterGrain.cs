using Orleans;
using System.Threading.Tasks;

namespace GiG.Core.Benchmarks.Orleans.StorageProviders.Contracts
{
    public interface IPlayerStateWriterGrain : IGrainWithGuidKey
    {
        Task WritePlayerDetailAsync(string firstName, string lastName);
    }
}