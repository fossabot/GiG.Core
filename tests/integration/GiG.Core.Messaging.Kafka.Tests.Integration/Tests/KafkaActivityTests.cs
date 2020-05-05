using GiG.Core.DistributedTracing.Abstractions;
using GiG.Core.DistributedTracing.Activity.Extensions;
using GiG.Core.Messaging.Kafka.Extensions;
using GiG.Core.Messaging.Kafka.Abstractions;
using GiG.Core.Messaging.Kafka.Abstractions.Interfaces;
using GiG.Core.Messaging.Kafka.Tests.Integration.Mocks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Constants = GiG.Core.Messaging.Kafka.Abstractions.Constants;
using Guid = System.Guid;

namespace GiG.Core.Messaging.Kafka.Tests.Integration.Tests
{
    [Trait("Category", "Integration")]
    public class KafkaActivityTests
    {
        private IHost _host;

        public KafkaActivityTests()
        {
            SetupServices();
        }
        
        [Fact]
        public async Task KafkaActivity_ProduceConsumeMessage_CorrectActivityReceived()
        {
            // Arrange
            var kafkaProducer = _host.Services.GetRequiredService<IKafkaProducer<string, MockMessage>>();
            var kafkaConsumer = _host.Services.GetRequiredService<IKafkaConsumer<string, MockMessage>>();
            var activityContext = _host.Services.GetRequiredService<IActivityContextAccessor>();
            var baggageKey = "testKey";
            
            activityContext.CurrentActivity.AddBaggage(baggageKey, "testing");

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
            var semaphore  = new SemaphoreSlim(0,1);
            var consumedMessage = new KafkaMessage<string, MockMessage>();
           
            // Act
            Task.Factory.StartNew(() =>
                {
                    while (!cts.IsCancellationRequested)
                    {
                        consumedMessage = (KafkaMessage<string, MockMessage>) kafkaConsumer.Consume(cts.Token);
                        semaphore.Release();
                    }
                }
            , TaskCreationOptions.LongRunning);
            
            await kafkaProducer.ProduceAsync(message);

            await semaphore.WaitAsync(30000);

            // Assert
            Assert.Equal(consumedMessage.Headers[Constants.CorrelationIdHeaderName], activityContext.CurrentActivity.ParentId);
            Assert.Equal(consumedMessage.Headers[baggageKey], activityContext.CurrentActivity.Baggage.FirstOrDefault(x => x.Key == baggageKey).Value);
        }

        private void SetupServices()
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
                .ConfigureAppConfiguration(appConfig => appConfig.AddJsonFile("appsettings.json"))
                .Build();
        }
    }
}