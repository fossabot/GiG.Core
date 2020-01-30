# GiG.Core.HealthChecks.Orleans

This Library provides an API to register Health Checks for an Orleans Silo.


## Basic Usage

The below code needs to be added to the `Startup.cs`. This will register the Live and Ready Health Check Endpoints.

```csharp
public void ConfigureServices(IServiceCollection services)
{
     services.AddOrleansHealthChecksSelfHosted(ctx.Configuration);
}

public static void ConfigureOrleans(HostBuilderContext ctx, ISiloBuilder builder)
{
    builder.ConfigureCluster(ctx.Configuration)
        .ConfigureEndpoints(ctx.Configuration)
        .AddHealthCheckDependencies();
}
```

The HealthChecks can be reached via the configured endpoints as per [GiG.Core.HealthChecks](GiG.Core.HealthChecks.md). 

### Configuration

The below table outlines the valid Configurations used to override the [HealthChecksOptions](../src/GiG.Core.HealthChecks.Orleans/Abstractions/HealthChecksOptions.cs) under the Config section `Orleans:HealthChecks`.

| Configuration Name | Type    | Optional | Default Value |
|:-------------------|:--------|:---------|:--------------|
| DomainFilter       | String  | Yes      | `*`           |
| Port               | Int     | Yes      | 5555          |
| HostSelf           | Boolean | Yes      | true          |
