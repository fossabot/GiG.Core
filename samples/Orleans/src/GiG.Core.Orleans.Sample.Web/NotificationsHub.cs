using GiG.Core.Orleans.Sample.Contracts;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace GiG.Core.Orleans.Sample.Web
{
    public class NotificationsHub : Hub, INotificationsHub
    {
        public async Task SubscribeAsync(string playerId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, playerId);
        }

        public async Task UnsubscribeAsync(string playerId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, playerId);
        }
    }
}