# GiG.Core.Configuration

This Library provides an API to add external configuration files which should be 'json' files located under './configs' folder and Environment variables.

## Basic Usage

The below code needs to be added to the `Program.cs`. Make use of `ConfigureExternalConfiguration()` when creating an `IHostBuilder`. Logging requires configuration.

```csharp
static class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureExternalConfiguration()
            .ConfigureServices(Startup.ConfigureServices);
}
```