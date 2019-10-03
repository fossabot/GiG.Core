using Orleans.Streams;
using System;
using System.Threading.Tasks;

namespace GiG.Core.Orleans.Sample.Grains
{
    internal static class AsyncStreamExtensions
    {
        public static async Task SubscribeOrResumeAsync<T>(this IAsyncStream<T> stream, Func<T, StreamSequenceToken, Task> onNextAsync)
        {
            var subscriptionHandles = await stream.GetAllSubscriptionHandles();

            if (subscriptionHandles.Count > 0)
            {
                foreach (var subscriptionHandle in subscriptionHandles)
                {
                    await subscriptionHandle.ResumeAsync(onNextAsync);
                }
            }

            await stream.SubscribeAsync(onNextAsync);
        }
    }
}