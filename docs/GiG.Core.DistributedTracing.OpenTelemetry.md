# GiG.Core.DistributedTracing.OpenTelemetry

This Library provides an API to register Tracing using Open Telemetry to capture distributed traces from your application.

## Basic Usage

The below code needs to be added to the `Startup.cs`.

```csharp

public void ConfigureServices(IServiceCollection services)
{
    services.AddTracing();
}

```