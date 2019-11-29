using GiG.Core.Messaging.Kafka.Abstractions.Interfaces;
using GiG.Core.Messaging.Kafka.Models;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GiG.Core.Messaging.Kafka.Consumer.Sample
{
    public class ConsumerService : IHostedService
    {
        private readonly IKafkaConsumer<string, Person> _consumer;

        public ConsumerService(IKafkaConsumer<string, Person> consumer) => _consumer = consumer ?? throw new ArgumentNullException(nameof(consumer));

        /// <inheritdoc />
        public async Task StartAsync(CancellationToken cancellationToken = default) 
        {
            await Task.Run(() => RunConsumer(cancellationToken), cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken = default)
        {
            _consumer.Dispose();
            return null;
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
                        var message = _consumer.Consume(token);
                        HandleMessage(message);

                        if (count++ % 10 == 0)
                        {
                            _consumer.Commit(message);
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
                _consumer.Dispose();
            }
        }

        private static void HandleMessage(IKafkaMessage<string, Person> message)
        {
            var serializedValue = JsonConvert.SerializeObject(message.Value);
            Console.WriteLine($"Consumed message in service \nkey: '{ message.Key }' \nvalue: '{ serializedValue }'");

            foreach (var (key, value) in message.Headers)
            {
                Console.WriteLine($"Key: { key }\tValue: { value }");
            }
        }
    }
}