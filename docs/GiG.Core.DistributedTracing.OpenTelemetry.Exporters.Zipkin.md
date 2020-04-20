# GiG.Core.DistributedTracing.OpenTelemetry.Exporters.Zipkin

**Note**: This Library is still not stable for release. Please do not use on production. 

This Library provides an API to register Tracing using Open Telemetry to capture distributed traces from your application and export them to a Zipkin Exporter. 
If the default Configuration section name of Jaeger is not 'Zipkin' the custom Configuration section name can be included when calling `RegisterZipkin()` as an optional parameter.

## Basic Usage

The below code needs to be added to the `Startup.cs`.

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddTracing(x => x.RegisterZipkin(), _configuration);
}
```

### Configuration

The below table outlines the valid Configurations under the Config section `Tracing:Exporters:Zipkin`.

| Configuration Name | Type     | Optional | Default Value                      |
|:-------------------|:---------|:---------|:-----------------------------------|
| IsEnabled          | Boolean  | Yes      | false                              |
| ServiceName        | String   | No       |                                    |
| Endpoint           | Uri      | Yes      | http://localhost:9411/api/v2/spans |
| TimeoutSeconds     | Timespan | Yes      | 10                                 |
| UseShortTraceIds   | Boolean  | Yes      | false                              |


#### Sample Configuration

```json
  {
      "Tracing": {
        "IsEnabled": true,
        "Exporters": {
          "Zipkin": {
            "IsEnabled": true,
            "ServiceName": "sample-web",
            "Endpoint": "http://localhost:9411/api/v2/spans", 
            "TimeoutSeconds": 20,
            "UseShortTraceIds": true
          }
        }
      }
  }
```