using GiG.Core.DistributedTracing.Activity.Extensions;
using GiG.Core.Messaging.Kafka.Abstractions;
using GiG.Core.Messaging.Kafka.Extensions;
using GiG.Core.Messaging.Kafka.Sample.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GiG.Core.Messaging.Kafka.Sample
{
    internal static class Startup
    {
        public static void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
        {
            var configuration = hostContext.Configuration;
            
            services.Configure<KafkaProviderOptions>(configuration.GetSection(KafkaProviderOptions.DefaultSectionName));

            services.AddActivityContextAccessor();

            services.AddKafkaProducer<string, Person>(options => options
                .WithJson()
                .FromConfiguration(configuration)
                .WithTopic("new-person-topic"));

            services
                .AddKafkaConsumer<string, Person>(options => options
                    .WithJson()
                    .FromConfiguration(configuration)
                    .WithTopic("new-person-topic"));

            services.AddHostedService<ProducerService>();
            services.AddHostedService<ConsumerService>();
        }
    }
}