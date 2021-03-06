﻿# GiG.Core.DistributedTracing.MassTransit

This Library provides an API to register Distributed Tracing for a MassTransit Consumer.

## Basic Usage

The below code needs to be added to the `Startup.cs`. This will register the Correlation Id context accessor and add the Consumer Observer to the MassTransit Consumer. 

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddMassTransitContext();
    services.AddCorrelationAccessor();

    services.AddMassTransit(x =>
    {
        x.AddConsumer<MyConsumer>();
        x.AddBus(provider => Bus.Factory.CreateUsingInMemory(cfg =>
        {
            cfg.Host.AddDefaultConsumerObserver(provider);

            cfg.ReceiveEndpoint(typeof(MyConsumer).FullName, e =>
            {
                e.Consumer<MyConsumer>(provider);
            });

        }));
    });
}
```

## Activity

The below code needs to be added to the `Startup.cs` of the MassTransit Producer in order to inject the Activity Id in the message headers.

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddMassTransit(x =>
    {
        x.AddBus(provider=> Bus.Factory.CreateUsingInMemory(cfg =>
        {
            cfg.ConfigurePublish(x => x.UseActivityFilter());
        }));
    });
}
```

The below code needs to be added to the `Startup.cs` of the Consumer. This will register the Activity Consumer Observer to the MassTransit Consumer. 

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddMassTransit(x =>
    {
        x.AddConsumer<MyConsumer>();
        x.AddBus(provider => Bus.Factory.CreateUsingInMemory(cfg =>
        {
            cfg.Host.AddActivityConsumerObserver();

            cfg.ReceiveEndpoint(typeof(MyConsumer).FullName, e =>
            {
                e.Consumer<MyConsumer>(provider);
            });

        }));
    });
}
```

## Telemetry

N.B. In order to have Telemetry information for RabbitMQ a Telemetry provider needs to be enabled. For more details refer to [GiG.Core.DistributedTracing.OpenTelemetry.Exporters.Jaeger](GiG.Core.DistributedTracing.OpenTelemetry.Exporters.Jaeger.md)

### Producer
The below code needs to be added to the `Startup.cs` of the MassTransit Producer in order to have Telemetry information.

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddMassTransit(x =>
    {
        x.AddBus(provider=> Bus.Factory.CreateUsingInMemory(cfg =>
        {
            cfg.ConfigurePublish(x => x.UseActivityFilterWithTracing(provider));
        }));
    });
}
```

### Consumer

The below code needs to be added to the `Startup.cs` of the Consumer in order to have Telemetry information.

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddMassTransit(x =>
    {
        x.AddConsumer<MyConsumer>();
        x.AddBus(provider => Bus.Factory.CreateUsingInMemory(cfg =>
        {
            cfg.Host.AddActivityConsumerObserverWithTracing(provider);

            cfg.ReceiveEndpoint(typeof(MyConsumer).FullName, e =>
            {
                e.Consumer<MyConsumer>(provider);
            });

        }));
    });
}
```