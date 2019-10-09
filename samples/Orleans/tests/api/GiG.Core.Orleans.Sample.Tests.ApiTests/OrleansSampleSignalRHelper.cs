using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace GiG.Core.Orleans.Sample.Tests.ApiTests
{
    public class OrleansSampleSignalRHelper
    {
        private readonly HubConnection _connection;
        
        public OrleansSampleSignalRHelper()
        {
            _connection = new HubConnectionBuilder()
                .WithUrl(SampleApiTestSettings.NotificationsUrl())
                .Build();
        }

        public async Task<string> ListenForNotification(string messageKey, string invokedMethodName, string argument)
        {
            string message = string.Empty;
            _connection.On<string>(messageKey, notification =>
            {
                message = notification;
            });

            await _connection.StartAsync();
            await _connection.InvokeAsync(invokedMethodName, argument);

            await _connection.DisposeAsync();

            return message;
        }
    }
}
