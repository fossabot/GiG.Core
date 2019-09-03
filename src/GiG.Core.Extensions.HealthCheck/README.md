# GiG.Core.Extensions.HealthCheck

This Library provides an API to register Health Checks for your application.


## Basic Usage

Add the below to your Startup class and this will register the Live and Ready Health Check Endpoints.

```csharp
	public void ConfigureServices(IServiceCollection services)
	{
		services.AddHealthChecks();
	}

	public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
	{
		app.UseHealthChecks();
	}
```

## Cached Health Check

You can implement a Cached Health Check by inheriting the [CachedHealthCheck](../GiG.Core.Abstractions.HealthCheck/CachedHealthCheck.cs) class.

```csharp
	public void ConfigureServices(IServiceCollection services)
	{
		services.AddCachedHealthChecks().AddCachedCheck<DummyCachedHealthCheck>(nameof(DummyCachedHealthCheck));
	}
```