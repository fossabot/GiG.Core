using GiG.Core.Performance.Orleans.Streams.Contracts;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Streams;
using System;
using System.Threading.Tasks;
using Constants = GiG.Core.Performance.Orleans.Streams.Contracts.Constants;

namespace GiG.Core.Performance.Orleans.Streams.Consumer
{
    [ImplicitStreamSubscription(Constants.MessageNamespace)]
    public class ConsumerGrain : Grain, IConsumerGrain, IAsyncObserver<Message>
    {
        private readonly ILogger _logger;
        private int _counter;

        public ConsumerGrain(ILogger<ConsumerGrain> logger)
        {
            _logger = logger;
        }

        public Task OnCompletedAsync()
        {
            _logger.LogInformation("Stream is Complete");

            return Task.CompletedTask;
        }

        public Task OnErrorAsync(Exception ex)
        {
            _logger.LogError($"Stream has error: {ex.Message}");

            return Task.CompletedTask;
        }

        public Task OnNextAsync(Message item, StreamSequenceToken token = null)
        {
            _counter++;

            return Task.CompletedTask;
        }

        public Task<int> GetCountersync()
        {
            return Task.FromResult(_counter);
        }
    }
}