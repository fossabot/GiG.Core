using GiG.Core.Messaging.MassTransit.Extensions;
using GiG.Core.Messaging.RabbitMQ.Abstractions;
using GreenPipes;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Security.Authentication;

namespace GiG.Core.Orleans.Sample.Consumer.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddMessageConsumer(this IServiceCollection services, IConfiguration configuration)
        {
            var rabbitConfiguration = configuration.GetSection(RabbitMQBusOptions.DefaultSectionName)
                .Get<RabbitMQBusOptions>();

            services.AddMassTransit(x =>
            {
                var serviceProvider = x.Collection.BuildServiceProvider();
                x.AddConsumer<PaymentConsumer>();
                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
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

                    host.AddDefaultConsumerObserver(serviceProvider);
                    cfg.ReceiveEndpoint(host,
                        typeof(PaymentConsumer).FullName, e =>
                        {
                            e.Consumer<PaymentConsumer>(provider);
                            e.UseMessageRetry(r =>
                            {
                                r.Ignore(new Type[] { typeof(NullReferenceException), typeof(ArgumentException), typeof(ArgumentNullException) });
                                r.Incremental(5, TimeSpan.FromMilliseconds(200), TimeSpan.FromMilliseconds(200));
                            });
                        });
                }));
            });

            services.AddHostedService<BusHostedService>();
        }
    }
}