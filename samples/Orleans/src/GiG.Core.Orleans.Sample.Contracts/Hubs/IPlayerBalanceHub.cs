using System.Threading.Tasks;

namespace GiG.Core.Orleans.Sample.Contracts.Hubs
{
    public interface IPlayerBalanceHub
    {
        Task SubscribeAsync(string playerId);
        Task UnsubscribeAsync(string playerId);
    }
}
