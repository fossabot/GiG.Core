# GiG.Core.Web.Authentication.Hmac

This Library provides an API to register the `HmacAuthenticationHandler` in `IServiceCollection`. When using this Library, the library will authenticate the request using the Hmac header to the request.

## Basic Usage

The below code needs to be added to the `Startup.cs` class. This will register the HmacAuthenticationHandler.

```chsarp
public void ConfigureServices(IServiceCollection services)
{
    // Configure Api Behavior Options
    services.AddHmacAuthentication();
    services.ConfigureDefaultHmacOptionProvider(_configuration);
}

public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    app.UseAuthentication();
    app.UseAuthorization();
}
```