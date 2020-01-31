# GiG.Core.Http.MultiTenant

This Library provides an API to register a TenantDelegatingHandler onto the HttpClient. When using this Library, the application will enrich the request with one or more X-Tenant-ID headers.

## Basic Usage

Make use of `TenantDelegatingHandler()` when configuring your HttpClientFactory. The Handler depends on 'GiG.Core.MultiTenant.Abstractions.ITenantAccessor'.

```csharp
var client = HttpClientFactory.Create(x =>
{
    x.AddDelegatingHandler(new TenantDelegatingHandler(new TenantAccessor()));
    x.Options.WithBaseAddress(new Uri("http://localhost"));
});
```

Make use of `AddTenantDelegatingHandler` when configuring your `HttpClient`.
**Note**: The `FromConfiguration` extension can be found in the nuget package ```GiG.Core.Http.Extensions```

```csharp
public static void ConfigureServices(HostBuilderContext ctx, IServiceCollection services)
{
    services
        .AddHttpClient("Payments", 
            client => 
            {
                client.FromConfiguration(ctx.Configuration, "Payments"); 
            })
        .AddTenantDelegatingHandler();
}
```