using System.Threading.Tasks;

namespace GiG.Core.Orleans.Sample.Contracts
{
    public interface INotificationsHub
    {
        Task SubscribeAsync(string playerId);

        Task UnsubscribeAsync(string playerId);
    }
}