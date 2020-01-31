# GiG.Core.HealthChecks.AspNetCore

This Library provides an API to register Health Check endpoints using the EndpointRouteBuilder.

## Basic Usage

The below code needs to be added to the `Startup.cs` to register Health Check endpoints.

```csharp
public void Configure(IApplicationBuilder app)
{           
	 app.UseEndpoints(endpoints => { 
        endpoints.MapHealthChecks();
    });       
}
```

### Configuration

For Configuration details refer to [GiG.Core.HealthChecks](GiG.Core.HealthChecks.md).