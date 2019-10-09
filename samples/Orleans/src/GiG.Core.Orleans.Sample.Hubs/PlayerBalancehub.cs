using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace GiG.Core.Orleans.Sample.Hubs
{
    public class PlayerBalanceHub : Hub
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
