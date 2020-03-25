# GiG.Core.MultiTenant.Activity

This Library provides an API to register Multi Tenancy for an application through key-value pair items in the Activity Baggage.


Please refer to [this link ](https://docs.microsoft.com/en-us/dotnet/api/system.diagnostics.activity?view=netcore-3.1) for more information on the .netcore implementation of Activity and Baggage.

## Basic Usage

The below code needs to be added to the `Startup.cs`. This will register the Activity Tenant Accessor.

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddActivityTenantAccessor();
}
```

The accessor provides a readonly access to the TenantID items in the activity baggage through the 'Values' property.

## Tenant ID Middleware

In web applications where the request headers may contain the TenantID, making use of the middleware documented [here](GiG.Core.MultiTenant.Web.md) allows transfer of TenantId values from the http context into the activity baggage.