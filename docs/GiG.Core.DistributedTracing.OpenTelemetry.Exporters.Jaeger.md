# GiG.Core.DistributedTracing.OpenTelemetry.Exporters.Jaeger

N.B. This Library is still not stable for release. Please do not use on production. 

This Library provides an API to register Tracing using Open Telemetry to capture distributed traces from your application and export them to Jaeger Exporter.

## Basic Usage

The below code needs to be added to the `Startup.cs`.

```csharp

public void ConfigureServices(IServiceCollection services)
{
    services.AddTracing(x => x.RegisterJaeger(), _configuration);
}

```

### Configuration

The below table outlines the valid Configurations under the Config section `Tracing:Exporters:Jaeger`.

| Configuration Name | Type    | Optional | Default Value |
|:-------------------|:--------|:---------|:--------------|
| IsEnabled          | Boolean | Yes      | false         |
| ServiceName        | String  | No       |               |
| AgentHost          | String  | Yes      | 'localhost'   |
| AgentPort          | Int     | Yes      | 6831          |
| MaxPacketSize      | Int     | Yes      | 65000         |


#### Sample Configuration

```json
  {
      "Tracing": {
        "IsEnabled": true,
        "Exporters": {
          "Jaeger": {
            "IsEnabled": true,
            "ServiceName": "sample-web",
            "AgentHost": "localhost", 
            "MaxPacketSize": 256
          }
        }
      }
  }
```