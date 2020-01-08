using GiG.Core.DistributedTracing.Abstractions;
using GiG.Core.Messaging.MassTransit.Extensions;
using GiG.Core.Messaging.RabbitMQ.Abstractions;
using GiG.Core.Orleans.Sample.Contracts.Messages;
using MassTransit;
using MassTransit.Context;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
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
                var contextAccessor = services.BuildServiceProvider().GetService<ICorrelationContextAccessor>();
                MessageCorrelation.UseCorrelationId<PaymentTransactionRequested>(c => contextAccessor.Value);

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
                    cfg.UseFaultAddress<PaymentTransactionRequested>(new Uri(host.Address, "sampledlx"));
                    cfg.ConfigureEndpoints(provider);
                }));
            });

            services.AddHostedService<BusHostedService>();
        }
    }
}