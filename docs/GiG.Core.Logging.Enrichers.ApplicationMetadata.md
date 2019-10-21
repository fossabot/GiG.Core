# GiG.Core.Logging.Enrichers.ApplicationMetadata

This Library provides an API to register and ApplicationMetadata Enricher for Logging when using Serilog.
When using this Library your application will enrich logs with the Application's Name and Version properties.

## Basic Usage

Make use of `EnrichWithApplicationMetadata()` when configuring logging.

```csharp

static class Program
{
    public static void Main()
    {
        CreateHostBuilder().Build().Run();
    }

    private static IHostBuilder CreateHostBuilder()
    {
        return Host.CreateDefaultBuilder()
		    .ConfigureLogging(x =>
			{
			    x.EnrichWithApplicationMetadata();
			});
    }
}

```