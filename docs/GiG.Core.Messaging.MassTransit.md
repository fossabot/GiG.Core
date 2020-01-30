# GiG.Core.Messaging.MassTransit

This Library provides an API to register MassTransit related functionality to an application.

## Fault Address

### Basic Usage

The below code needs to be added in the MassTransit Producer Bus setup. This will setup the Fault Address for a message. 

```csharp
public static void AddMessagePublisher(this IServiceCollection services)
{
    services.AddMassTransit(x =>
        x.AddBus(provider => Bus.Factory.CreateUsingInMemory(cfg =>
        {
            cfg.UseFaultAddress<PaymentTransactionRequested>(new Uri("rabbitmq://localhost:15672/testdlx"));
        })
    ));
}       
```