# GiG.Core.HealthChecks

This Library provides an API to register Health Checks for an application.


## Basic Usage

The below code needs to be added to the `Startup.cs`. This will register the Live and Ready Health Check Endpoints.

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddHealthChecks();
}

public void Configure(IApplicationBuilder app)
{
    app.UseHealthChecks();
}
```

## Health Endpoints

The following are the exposed Health endpoints.  You can change the default values by overriding the [HealthChecksOptions](../src/GiG.Core.HealthChecks.Abstractions/HealthChecksOptions.cs) configuration options using the properties below.

| Type  | Default Endpoint       | Property Name |
|-------|------------------------|---------------|
| Ready | /actuator/health/ready | `ReadyUrl`    |
| Live  | /actuator/health/live  | `LiveUrl`     |
| Both  | /actuator/health       | `CombinedUrl` |

An extension method is provided to read the overrides from application settings.

```csharp
private readonly IConfiguration _configuration;
	
public void ConfigureServices(IServiceCollection services)
{
    services.ConfigureHealthChecks(_configuration);
}
```

### Ready Health Checks

These Health Checks are used to temporary disable traffic to the application until the health check returns 200. Add the below to your Startup class and this will register the Ready Health Check Endpoints.

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services
        .AddHealthChecks()
        .AddReadyCheck<DummyReadyHealthCheck>(nameof(DummyReadyHealthCheck));
}

public void Configure(IApplicationBuilder app)
{
    app.UseHealthChecks();
}
```

### Live Health Checks

These Health Checks are used to kill the application if health check fails.  Usually used to prevent deadlocks.  Add the below to your Startup class and this will register the Ready Live Check Endpoints.

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services
	    .AddHealthChecks()
	    .AddLiveCheck<DummyLiveHealthCheck>(nameof(DummyLiveHealthCheck));
}

public void Configure(IApplicationBuilder app)
{
    app.UseHealthChecks();
}
```

## Cached Health Check

You can implement a Cached Health Check by inheriting the [CachedHealthCheck](../src/GiG.Core.HealthChecks.Abstractions/CachedHealthCheck.cs) class.  Registration in Startup class is very similar to the above.

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services
	    .AddCachedHealthChecks()
	    .AddReadyCheck<DummyCachedReadyHealthCheck>(nameof(DummyCachedReadyHealthCheck))
	    .AddLiveCheck<DummyCachedLiveHealthCheck>(nameof(DummyCachedLiveHealthCheck));
}
```