using GiG.Core.DistributedTracing.Abstractions;
using GiG.Core.DistributedTracing.Activity.Extensions;
using GiG.Core.Messaging.Kafka.Extensions;
using GiG.Core.Messaging.Kafka.Abstractions;
using GiG.Core.Messaging.Kafka.Abstractions.Interfaces;
using GiG.Core.Messaging.Kafka.Tests.Integration.Mocks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Constants = GiG.Core.Messaging.Kafka.Abstractions.Constants;
using Guid = System.Guid;

namespace GiG.Core.Messaging.Kafka.Tests.Integration.Tests
{
    [Trait("Category", "IntegrationWithDependency")]
    public class KafkaActivityTests : IAsyncLifetime
    {
        private IHost _host;
        private IKafkaProducer<string, MockMessage> _kafkaProducer;
        private IKafkaConsumer<string, MockMessage> _kafkaConsumer;
        private IActivityContextAccessor _activityContextAccessor;
        private SemaphoreSlim _semaphore;

        public async Task InitializeAsync()
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureServices((ctx, x) =>
                    x.Configure<KafkaProviderOptions>(ctx.Configuration.GetSection(KafkaProviderOptions.DefaultSectionName))
                        .AddActivityContextAccessor()
                        .AddKafkaProducer<string, MockMessage>(options => options
                            .FromConfiguration(ctx.Configuration)
                            .WithJson()
                            .WithTopic("new-mock-message-topic"))
                        .AddKafkaConsumer<string, MockMessage>(options => options
                            .FromConfiguration(ctx.Configuration)
                            .WithJson()
                            .WithTopic("new-mock-message-topic"))
                )
                .Build();

            await _host.StartAsync();

            _kafkaProducer = _host.Services.GetRequiredService<IKafkaProducer<string, MockMessage>>();
            _kafkaConsumer = _host.Services.GetRequiredService<IKafkaConsumer<string, MockMessage>>();
            _activityContextAccessor = _host.Services.GetRequiredService<IActivityContextAccessor>();
            _semaphore = new SemaphoreSlim(0, 1);
        }

        [Fact]
        public async Task KafkaActivity_ProduceConsumeMessage_CorrectActivityReceived()
        {
            // Arrange
            var baggageKey = "testKey";
            _activityContextAccessor.CurrentActivity.AddBaggage(baggageKey, "testing");
            var mockMessage = new MockMessage();
            var messageId = Guid.NewGuid().ToString();

            var message = new KafkaMessage<string, MockMessage>
            {
                Key = "mock-message",
                Value = mockMessage,
                MessageId = messageId,
                MessageType = "mockMessage.Created"
            };

            var cts = new CancellationTokenSource();

            var consumedMessage = new KafkaMessage<string, MockMessage>();

            // Act
#pragma warning disable 4014
            Task.Factory.StartNew(() =>
#pragma warning restore 4014
            {
                while (!cts.IsCancellationRequested)
                {
                    consumedMessage = (KafkaMessage<string, MockMessage>) _kafkaConsumer.Consume(cts.Token);
                    _semaphore.Release();
                }
            }, TaskCreationOptions.LongRunning);

            await _kafkaProducer.ProduceAsync(message);

            await _semaphore.WaitAsync(30000);

            // Assert
            Assert.NotEmpty(consumedMessage.Headers[Constants.CorrelationIdHeaderName]);
            Assert.NotEmpty(consumedMessage.Headers[baggageKey]);
        }

        public async Task DisposeAsync()
        {
            await _host.StopAsync();
            _host.Dispose();
            _semaphore.Dispose();
            _kafkaProducer.Dispose();
            _kafkaConsumer.Dispose();
        }
    }
}