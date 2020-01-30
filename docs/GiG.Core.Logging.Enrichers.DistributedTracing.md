# GiG.Core.Logging.Enrichers.DistributedTracing

This Library provides an API to register the Correlation Id Enricher for Logging when using Serilog. When using this Library your application will enrich logs with a CorrelationId property if an 'X-Correlation-ID' is present in the request headers.

## Basic Usage

Make use of `EnrichWithCorrelation()` when configuring logging. The Enricher depends on 'GiG.Core.DistributedTracing.Abstractions.ICorrelationContextAccessor'.

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
            .ConfigureServices(x => 
            {
			     x.AddCorrelationAccessor();
			 })
			 .ConfigureLogging(x =>
			 {
			     x.EnrichWithCorrelation();
			 });
    }
}
```