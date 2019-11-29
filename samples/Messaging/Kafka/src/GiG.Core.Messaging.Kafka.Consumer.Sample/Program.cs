using GiG.Core.Messaging.Kafka.Abstractions;
using GiG.Core.Messaging.Kafka.Extensions;
using GiG.Core.Messaging.Kafka.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace GiG.Core.Messaging.Kafka.Consumer.Sample
{
    class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                //.ConfigureLogging()
                .ConfigureServices((hostContext, services) =>
                {
                    var kafkaOptions = hostContext.Configuration
                        .GetSection(KafkaProviderOptions.DefaultSectionName)
                        .Get<KafkaProviderOptions>();

                    services
                        .AddOptions()
                        .AddScoped(sp => kafkaOptions);

                    services
                        .AddKafkaConsumer<string, Person>(options => options
                            .WithJson()
                            .FromConfiguration(hostContext.Configuration)
                            .WithTopic("new-person-topic"))
                        .AddHostedService<ConsumerService>();
                });
    }
}