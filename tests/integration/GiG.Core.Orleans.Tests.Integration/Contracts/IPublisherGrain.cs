using GiG.Core.Orleans.Tests.Integration.Mocks;
using Orleans;
using System;
using System.Threading.Tasks;

namespace GiG.Core.Orleans.Tests.Integration.Contracts
{
    public interface IPublisherGrain : IGrainWithIntegerKey
    {
        Task<Guid> PublishMessage(MockMessage mockMessage);
    }
}