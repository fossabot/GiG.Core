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

## Tenant ID Middleware

The below TenantID middleware can be registered in a web application to transfer values in the 'X-Tenant-ID' header coming from the http context to the Activity Baggage.
Add the below the the 'Configure' method inside the 'Startup.cs' class.

```csharp
public void Configure(IApplicationBuilder app)
{
    app.UseTenantIdMiddleware();
}
```