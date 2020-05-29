using GiG.Core.Messaging.Kafka.Abstractions.Interfaces;
using GiG.Core.Messaging.Kafka.Sample.Interfaces;
using GiG.Core.Messaging.Kafka.Sample.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GiG.Core.Messaging.Kafka.Sample
{
    public class ConsumerService : BackgroundService
    {
        private readonly IKafkaConsumer<string, CreatePerson> _kafkaConsumer;
        private readonly ILogger<ConsumerService> _logger;
        private readonly ICreatePersonService _createPersonService;

        public ConsumerService(IKafkaConsumer<string, CreatePerson> kafkaConsumer, ILogger<ConsumerService> logger, ICreatePersonService createPersonService)
        {
            _kafkaConsumer = kafkaConsumer;
            _logger = logger;
            _createPersonService = createPersonService;
        }

        /// <inheritdoc />
        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            await RunConsumer(cancellationToken);
        }

        /// <inheritdoc />
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _kafkaConsumer.Dispose();
            return Task.CompletedTask;
        }

        private async Task RunConsumer(CancellationToken token = default)
        {
            var count = 0;

            try
            {
                while (!token.IsCancellationRequested)
                {
                    try
                    {
                        var message = _kafkaConsumer.Consume(token);
                        await HandleMessageAsync(message);

                        if (count++ % 10 == 0)
                        {
                            _kafkaConsumer.Commit(message);
                            count = 0;
                        }
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, e.Message);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                _kafkaConsumer.Dispose();
            }
        }

        private async Task HandleMessageAsync(IKafkaMessage<string, CreatePerson> message)
        {
            await _createPersonService.HandleMessageAsync(message);
        }
    }
}