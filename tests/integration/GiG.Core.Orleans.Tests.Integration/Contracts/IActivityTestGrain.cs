using Orleans;
using System.Threading.Tasks;

namespace GiG.Core.Orleans.Tests.Integration.Contracts
{
    public interface IActivityTestGrain : IGrainWithGuidKey
    {
        Task<ActivityResponse> GetActivityAsync();

        Task<ActivityResponse> GetStreamActivityAsync();
    }
}