# GiG.Core.Logging.Enrichers.MultiTenant

This Library provides an API to register Tenant Id Enricher for Logging when using Serilog. When using this Library your application will enrich logs with a TenantId property if an 'X-Tenant-ID' is present in request headers.

## Basic Usage

Make use of `EnrichWithTenant()` when configuring logging. The Enricher depends on 'GiG.Core.MultiTenant.Abstractions.ITenantAccessor'.

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
                x.AddTenantAccessor();
			})
			.ConfigureLogging(x =>
			{
			    x.EnrichWithTenant();
			});
    }
}

```