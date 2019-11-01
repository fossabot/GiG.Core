# GiG.Core.Logging.File

This Library provides an API to register Logging to File using Serilog for an application.

## Basic Usage

Make use of `ConfigureLogging(x => x.WriteToFile())` when creating an `IHostBuilder`. Logging requires configuration.

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
            .ConfigureLogging(x => x.WriteToFile());
    }
}

```

### Configuration

The below table outlines the valid Configurations used to override the [FileSinkOptions](../src/GiG.Core.Logging.Sinks.File/Internal/FileSinkOptions.cs) under the Config section `Logging:Sinks:File`

| Configuration Name     | Type   | Optional | Default Value           |
|:-----------------------|:-------|:---------|:------------------------|
| Enabled                | bool   | Yes      | 'false'                 |
| FilePath               | string | Yes      | 'logs\\log-.txt'        |
| RollingInterval        | string | Yes      | 'RollingInterval.Day'   |
| FileSizeLimitBytes     | long   | Yes      | 1L * 1024 * 1024 * 1024 |
| RetainedFileCountLimit | int    | Yes      | 31                      |
| RollOnFileSizeLimit    | bool   | Yes      | 'true'                  |

