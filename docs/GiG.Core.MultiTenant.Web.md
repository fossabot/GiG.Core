# GiG.Core.MultiTenant.Web

This Library provides an API to register Multi Tenancy for your application.


## Basic Usage

Add the below to your Startup class and this will register the Tenant context accessor.


```csharp

	public void ConfigureServices(IServiceCollection services)
	{
		services.AddTenantAccessor();
	}

```