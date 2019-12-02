using GiG.Core.Messaging.Kafka.Abstractions.Interfaces;
using GiG.Core.Messaging.Kafka.Sample.Models;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GiG.Core.Messaging.Kafka.Sample
{
    public class ConsumerService : IHostedService
    {
        private readonly IKafkaConsumer<string, Person> _kafkaConsumer;

        public ConsumerService(IKafkaConsumer<string, Person> consumer) => _kafkaConsumer = consumer ?? throw new ArgumentNullException(nameof(consumer));

        /// <inheritdoc />
        public async Task StartAsync(CancellationToken cancellationToken = default) 
        {
            await Task.Run(() => RunConsumer(cancellationToken), cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken = default)
        {
            _kafkaConsumer.Dispose();
            return Task.CompletedTask;
        }
        
        private void RunConsumer(CancellationToken token = default)
        {
            var count = 0;

            try
            {
                while (true)
                {
                    try
                    {
                        var message = _kafkaConsumer.Consume(token);
                        HandleMessage(message);

                        if (count++ % 10 == 0)
                        {
                            _kafkaConsumer.Commit(message);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Error occurred: { e.Message } ");
                    }
                }
            }
            catch (OperationCanceledException)
            {
                _kafkaConsumer.Dispose();
            }
        }

        private static void HandleMessage(IKafkaMessage<string, Person> message)
        {
            var serializedValue = JsonConvert.SerializeObject(message.Value);
            Console.WriteLine($"Consumed message in service [key: '{ message.Key }'] [value: '{ serializedValue }']");

            foreach (var (key, value) in message.Headers)
            {
                Console.WriteLine($"Header: { key }\tValue: { value }");
            }
        }
    }
}