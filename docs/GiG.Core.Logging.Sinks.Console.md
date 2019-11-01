# GiG.Core.Logging.Console

This Library provides an API to register Logging to Console using Serilog for an application.

## Basic Usage

Make use of `ConfigureLogging(x => x.WriteToConsole())` when creating an `IHostBuilder`. Logging requires configuration.

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
            .ConfigureLogging(x => x.WriteToConsole());
    }
}
```

### Configuration

The below table outlines the valid Configurations under the Config section `Logging:Sinks:Console`.

| Configuration Name | Type | Optional | Default Value |
|:-------------------|:-----|:---------|:--------------|
| IsEnabled          | bool | Yes      | false         |

#### Sample Configuration

```json
 {
   "Logging": {
     "MinimumLevel": "Debug",
     "Sinks": {
      "Console": {
        "IsEnabled": true
      }
     }
   }
 }
```