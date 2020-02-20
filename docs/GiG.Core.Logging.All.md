# GiG.Core.Logging.All

This Library provides an API to register Logging using Serilog and multiple Sinks and Enrichers for an application. When using this Library, the application will log to Console, File and Fluentd, and will also enrich your logs with Application Metadata and Correlation ID.

## Basic Usage

Make use of `ConfigureLogging()` when Creating an `IHostBuilder`. Logging requires configuration.

```csharp
using GiG.Core.Logging.All.Extensions;

static class Program
{
    public static void Main()
    {
        CreateHostBuilder().Build().Run();
    }

    private static IHostBuilder CreateHostBuilder()
    {
        return Host
            .CreateDefaultBuilder()
            .ConfigureLogging();
    }
}
```

### Configuration

The below table outlines the valid Configurations under the Config section `Logging`.

| Configuration Name | Type                                    | Optional |
|:-------------------|:----------------------------------------|:---------|
| MinimumLevel       | String or Object (to specify overrides) | No       |
| Sinks              | List of Sinks configuration             | No       |

#### Sample Configuration

The following sample includes a basic usage with a minimum log levels that applies to all namespaces and categories.


```json
{
  "Logging": {
    "MinimumLevel": "Information",
    "Sinks": {
      "Console": {
        "IsEnabled": true
      }
    }
  }
}
```

The following sample also includes a list of overrides to configure minimum log level for a specified namespace or category.

```json
{
  "Logging": {
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning"
      }
    },
    "Sinks": {
      "Console": {
        "IsEnabled": true
      }
    }
  }
}
```