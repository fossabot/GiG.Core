using GiG.Core.DistributedTracing.Web.Extensions;
using GiG.Core.Messaging.Kafka.Abstractions;
using GiG.Core.Messaging.Kafka.Extensions;
using GiG.Core.Messaging.Kafka.Sample.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GiG.Core.Messaging.Kafka.Sample
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration) => _configuration = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            var kafkaOptions = _configuration
                .GetSection(KafkaProviderOptions.DefaultSectionName)
                .Get<KafkaProviderOptions>();

            services.AddCorrelationAccessor();
            
            services
                .AddOptions()
                .AddScoped(sp => kafkaOptions);

            services.AddKafkaProducer<string, Person>(options => options
                .WithJson()
                .FromConfiguration(_configuration)
                .WithTopic("new-person-topic"));

            services
                .AddKafkaConsumer<string, Person>(options => options
                    .WithJson()
                    .FromConfiguration(_configuration)
                    .WithTopic("new-person-topic"));

            services.AddHostedService<ProducerService>();
            services.AddHostedService<ConsumerService>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseCorrelation();
        }
    }
}