# GiG.Core.HealthChecks

This Library provides an API to register Health Checks for your application.


## Basic Usage

Add the below to your Startup class and this will register the Live and Ready Health Check Endpoints.

```csharp
	private readonly IConfiguration _configuration;
	
	public void ConfigureServices(IServiceCollection services)
	{
		services.AddHealthChecks(_configuration);
	}

	public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
	{
		app.UseHealthChecks();
	}
```

## Cached Health Check

You can implement a Cached Health Check by inheriting the [CachedHealthCheck](../GiG.Core.HealthChecks.Abstractions/CachedHealthCheck.cs) class.

```csharp
	private readonly IConfiguration _configuration;
	
	public void ConfigureServices(IServiceCollection services)
	{
		services.AddCachedHealthChecks(_configuration).AddCachedCheck<DummyCachedHealthCheck>(nameof(DummyCachedHealthCheck));
	}
```