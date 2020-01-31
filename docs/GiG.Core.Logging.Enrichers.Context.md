# GiG.Core.Logging.Enrichers.Context

This Library provides an API to register the Context Enricher for Logging when using Serilog. When using this Library your application will enrich logs with the IP Address of the originating request if available.

## Pre-requisites

The following package is required to consume this package:
 - GiG.Core.Logging
 - [GiG.Core.Context.Web](GiG.Core.Context.Web.md) or [GiG.Core.Context.Orleans](GiG.Core.Context.Orleans.md)
 
## Basic Usage

Make use of `EnrichWithRequestContext()` when configuring logging. The Enricher depends on 'GiG.Core.Context.Abstractions.IRequestContextAccessor'.

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
                x.AddRequestContextAccessor();
            })
            .ConfigureLogging(x =>
            {
                x.EnrichWithRequestContext();
            });
    }
}
```