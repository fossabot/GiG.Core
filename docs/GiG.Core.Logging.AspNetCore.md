# GiG.Core.Logging.AspNetCore

This Library provides an API to register the `HttpRequestResponseLoggingMiddleware`. It is used to log Http Requests and Http Responses.

## Basic Usage

The below code needs to be added to the `Startup.cs`.
 
```csharp
private readonly IConfiguration _configuration;

public void ConfigureServices(IServiceCollection services)
{
    services.ConfigureHttpRequestResponseLogging(_configuration);
}

```csharp
public void Configure(IApplicationBuilder app)
{   
    app.UseRouting();
    app.UseHttpRequestResponseLogging();
    app.UseEndpoints(endpoints =>
	{
		endpoints.MapControllers();
	});
}
```