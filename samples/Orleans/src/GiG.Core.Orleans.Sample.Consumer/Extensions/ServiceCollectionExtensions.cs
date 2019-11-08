using GiG.Core.MassTransit;
using GiG.Core.Messaging.RabbitMQ.Abstractions;
using GreenPipes;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
                var contextFactory = x.Collection.BuildServiceProvider().GetRequiredService<IMassTransitContextFactory>();

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

                    host.ConnectConsumeObserver(new MassTransitObserver(contextFactory));

                    cfg.ReceiveEndpoint(host,
                        typeof(PaymentConsumer).FullName, e =>
                        {
                            e.Consumer<PaymentConsumer>(provider);
                        });

                    cfg.UseMessageRetry(r =>
                    {
                        r.Intervals(new int[] { 250, 500, 1000 });
                    });
                }));
            });

            services.AddHostedService<BusHostedService>();
        }
    }
}