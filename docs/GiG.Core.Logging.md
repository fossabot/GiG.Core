# GiG.Core.Logging.Console

This Library provides an API to register Logging using Serilog for an application.

## Basic Usage

Make use of `ConfigureLogging()` when creating an `IHostBuilder`. Logging requires configuration.

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

