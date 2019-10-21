# GiG.Core.Configuration

This Library provides an API to add external configuration via JSON file and Environment variables.

## Basic Usage

Make use of `ConfigureExternalConfiguration()` when creating an `IHostBuilder`. Logging requires configuration.

```csharp

	public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
				.ConfigureExternalConfiguration()
                .ConfigureLogging()
				.ConfigureServices(Startup.ConfigureServices);
    }

```
