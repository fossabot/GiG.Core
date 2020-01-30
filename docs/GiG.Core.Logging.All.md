# GiG.Core.Logging.All

This Library provides an API to register Logging using Serilog and multiple Sinks and Enrichers for an application. When using this Library, the application will log to Console, File and Fluentd, and will also enrich your logs with Application Metadata and Correlation ID.

## Basic Usage

Make use of `ConfigureLogging()` when Creating an `IHostBuilder`. Logging requires configuration.

```csharp
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