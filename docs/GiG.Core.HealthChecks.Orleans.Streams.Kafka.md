# GiG.Core.HealthChecks.Orleans.Streams.Kafka

This Library provides an API to register Health Check for Orleans Kafka Streams.

## Basic Usage

The below code needs to be added to the `Startup.cs`. This will register the Kafka Health check in the Ready Health Check Endpoint. This depends on `GiG.Core.HealthChecks.AspNetCore`, `GiG.Core.Orleans.Streams.Kafka`.

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services
      .AddCachedHealthChecks()
      .AddKafkaStreams(tags: new [] { Constants.ReadyTag });
}

public void Configure(IApplicationBuilder app)
{           
	 app.UseEndpoints(endpoints => { 
        endpoints.MapHealthChecks();
    });       
}
```