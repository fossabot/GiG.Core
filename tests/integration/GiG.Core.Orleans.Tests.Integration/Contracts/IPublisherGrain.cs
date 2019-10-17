using System;
using System.Threading.Tasks;
using GiG.Core.Orleans.Tests.Integration.Mocks;
using Orleans;

namespace GiG.Core.Orleans.Tests.Integration.Contracts
{
    public interface IPublisherGrain : IGrainWithIntegerKey
    {
        Task<Guid> PublishMessage(MockMessage mockMessage);
    }
}