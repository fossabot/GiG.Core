using GiG.Core.Messaging.RabbitMQ.Abstractions;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Authentication;

namespace GiG.Core.Orleans.Sample.Web.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddMessagePublisher(this IServiceCollection services, IConfiguration configuration)
        {
            var rabbitConfiguration = configuration.GetSection(RabbitMQBusOptions.DefaultSectionName)
                .Get<RabbitMQBusOptions>();

            services.AddMassTransit(x =>
            {
                x.AddBus(provider=> Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    var host = cfg.Host(rabbitConfiguration.Host, (ushort)rabbitConfiguration.Port,
                        rabbitConfiguration.VirtualHost, rabbitConfiguration.ConnectionName, configurator =>
                        {
                            configurator.Username(rabbitConfiguration.Username);
                            configurator.Password(rabbitConfiguration.Password);
                            configurator.Heartbeat(rabbitConfiguration.Heartbeat);
                            configurator.PublisherConfirmation = true;

                            if (rabbitConfiguration.SslEnabled)
                            {
                                configurator.UseSsl(context => { context.Protocol = SslProtocols.Tls12; });
                            }
                        });
                    
                    cfg.ConfigureEndpoints(provider);
                }));
            });

            services.AddHostedService<BusHostedService>();
        }
    }
}