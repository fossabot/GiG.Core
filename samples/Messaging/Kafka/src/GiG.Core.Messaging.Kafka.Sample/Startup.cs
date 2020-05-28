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

            services.AddKafkaProducer<string, CreatePerson>(options => options
                .WithJson()
                .FromConfiguration(configuration)
                .WithTopic("create-person"));

            services
                .AddKafkaConsumer<string, CreatePerson>(options => options
                    .WithJson()
                    .FromConfiguration(configuration)
                    .WithTopic("create-person"));
            
            services.AddKafkaProducer<string, PersonCreated>(options => options
                .WithJson()
                .FromConfiguration(configuration)
                .WithTopic("person"));

            services.AddHostedService<ProducerService>();
            services.AddHostedService<ConsumerService>();
        }
    }
}