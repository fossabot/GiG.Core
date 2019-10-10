using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace GiG.Core.Orleans.Sample.Tests.ApiTests
{
    public class OrleansSampleSignalRHelper
    {
        public readonly HubConnection Connection;
        
        public OrleansSampleSignalRHelper()
        {
            Connection = new HubConnectionBuilder()
                .WithUrl(SampleApiTestSettings.NotificationsUrl())
                .Build();
        }

        public async Task<decimal> ListenForNotification(string messageKey, string invokedMethodName, string argument, Action operation)
        {
            SemaphoreSlim semaphore = new SemaphoreSlim(0, 1);
            decimal message = 0;
            Connection.On<decimal>(messageKey, notification =>
            {
                message = notification;
                semaphore.Release();
            });

            await Connection.StartAsync();
            await Connection.InvokeAsync(invokedMethodName, argument);

            operation();
            semaphore.Wait(1500);

            await Connection.DisposeAsync();

            return message;
        }
    }
}
