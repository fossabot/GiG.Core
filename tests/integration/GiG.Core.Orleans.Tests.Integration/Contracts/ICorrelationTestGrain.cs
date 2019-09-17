using Orleans;
using System;
using System.Threading.Tasks;

namespace GiG.Core.Orleans.Tests.Integration.Contracts
{
    public interface ICorrelationTestGrain : IGrainWithStringKey
    {
        Task<Guid> GetCorrelationIdAsync();
    }
}