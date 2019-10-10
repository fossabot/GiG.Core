using System;
using System.Threading.Tasks;

namespace GiG.Core.Orleans.Sample.Tests.ApiTests
{
    public class SampleApiTestsFixture
    {
        public Guid PlayerId { get; }

        public  SampleApiTestsFixture()
        {
            PlayerId = Guid.NewGuid();
        }

        public async Task<decimal> GetPlayerBalanceNotification(Guid playerId, Action operation)
        {
            return await new OrleansSampleSignalRHelper().ListenForNotification("BalanceChanged", "SubscribeAsync", playerId.ToString(), operation);
        }
    }
}
