# GiG.Core.Logging.Sinks.File

This Library provides an API to register Logging to File using Serilog for an application.

## Pre-requisites

The following package is required to consume this package:
 - GiG.Core.Logging
 
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

| Configuration Name     | Type    | Optional | Default Value           |
|:-----------------------|:--------|:---------|:------------------------|
| IsEnabled              | Boolean | Yes      | false                   |
| FilePath               | String  | Yes      | `logs\\log-.txt`        |
| RollingInterval        | String  | Yes      | `RollingInterval.Day`   |
| FileSizeLimitBytes     | Long    | Yes      | 1L * 1024 * 1024 * 1024 |
| RetainedFileCountLimit | Int     | Yes      | 31                      |
| RollOnFileSizeLimit    | Boolean | Yes      | true                    |

#### Sample Configuration

```json
 {
   "Logging": {
     "MinimumLevel": "Debug",
     "Sinks": {
      "File": {
        "IsEnabled": true, 
        "RollingInterval": "Infinite",
        "FilePath": "logs/logs.txt"
      }
     }
   }
 }
```
