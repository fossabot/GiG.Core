# GiG.Core.MultiTenant.Web

This Library provides an API to register Multi Tenancy for an application.

## Basic Usage

The below code needs to be added to the `Startup.cs`. This will register the Tenant Context Accessor.

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddTenantAccessor();
}
```