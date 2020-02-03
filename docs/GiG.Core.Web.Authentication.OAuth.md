# GiG.Core.Web.Authentication.OAuth

This Library provides an API to configure OAuth2.0 as a protocol for Authentication.

## Basic Usage

The below code needs to be added to the `Startup.cs` class. This will configure the Authentication protocol as OAuth2.0.

```chsarp
public void ConfigureServices(IServiceCollection services)
{
    // Configure Api Behavior Options
    services.ConfigureOAuthAuthentication(_configuration);
}

public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    app.UseAuthentication();
    app.UseAuthorization();
}
```