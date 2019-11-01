# GiG.Core.Logging.Fluentd

This Library provides an API to register Logging to Fluentd using Serilog for an application.

## Basic Usage

Make use of `ConfigureLogging(x => x.WriteToFluentd())` when creating an `IHostBuilder`. Logging requires configuration.

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
            .ConfigureLogging(x => x.WriteToFluentd());
    }
}

```

### Configuration

The below table outlines the valid Configurations used to override the [FluentdSinkOptions](../src/GiG.Core.Logging.Sinks.Fluentd/Internal/FluentdSinkOptions.cs) under the Config section `Logging:Sinks:Fluentd`

| Configuration Name | Type   | Optional | Default Value |
|:-------------------|:-------|:---------|:--------------|
| Enabled            | bool   | Yes      | false         |
| Hostname           | string | Yes      | 'localhost'   |
| Port               | int    | Yes      | 24224         |
