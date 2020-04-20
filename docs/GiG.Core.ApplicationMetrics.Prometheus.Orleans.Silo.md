# GiG.Core.ApplicationMetrics.Prometheus.Orleans.Silo

This Library provides an API to register a telemetry consumer to ship metrics to Prometheus.

## Basic Usage

The below code needs to be added to the `Program.cs` when creating a new HostBuilder.
**Note**: The `UseOrleans` extension can be found in the nuget package ```Microsoft.Orleans.Server```


```csharp
public static void Main(string[] args)
{
    CreateHostBuilder(args).Build().Run();
}

public static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .UseOrleans((hostBuilder, siloBuilder) =>
            siloBuilder.AddPrometheusTelemetry(hostBuilder.Configuration));
```

### Configuration

The below table outlines the valid Configurations used to override the [OrleansMetricsOptions](../src/GiG.Core.ApplicationMetrics.Prometheus.Orleans.Silo/Abstractions/OrleansMetricsOptions.cs) under the Config section `Orleans:Metrics`.

| Configuration Name      | Type    | Required                  | Default Value |
|:------------------------|:--------|:--------------------------|:--------------|
| IsEnabled               | Boolean | No                        | `true`        |

#### Sample Configuration

```json
{
   "Orleans": {     
      "Metrics":{
        "IsEnabled": true
      }
  }
}
 ```