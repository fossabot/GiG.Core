using System.Threading.Tasks;

namespace GiG.Core.Orleans.Sample.Hubs
{
    public interface INotificationsHub
    {
        Task SubscribeAsync(string playerId);
        Task UnsubscribeAsync(string playerId);
    }
}