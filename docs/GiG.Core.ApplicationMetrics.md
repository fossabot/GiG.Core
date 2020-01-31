# GiG.Core.ApplicationMetrics

This Library provides an API to register application metrics configuration options.

## Basic Usage

The below code needs to be added to the `Startup.cs`.  This will register the Metrics configuration options.
 
```csharp
public static void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
{
    var configuration = hostContext.Configuration;
    
    //Application Metrics
    services.ConfigureApplicationMetrics(configuration);
}
```

### Configuration

The below table outlines the valid Configurations used to configure the [ApplicationMetricsOptions](../src/GiG.Core.ApplicationMetrics.Abstractions/ApplicationMetricsOptions.cs).

| Configuration Name | Type    | Optional | Default Value            |
|:-------------------|:--------|:---------|:-------------------------|
| Url                | String  | Yes      | `/metrics`               |
| IsEnabled          | Boolean | Yes      | true                     |

#### Sample Configuration

```json
{
  "ApplicationMetrics": {
    "Url": "/metrics",
    "IsEnabled": true
  }
}
```