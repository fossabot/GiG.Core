# GiG.Core.Web.Docs.Authentication.OAuth

This Library provides an API to configure OAuth2.0 as a protocol for Authentication when accessing API Documentation.

## Basic Usage

The below code needs to be added to the `Startup.cs` class. This will configure the Authentication protocol for Swagger as OAuth2.0.

```chsarp
public void ConfigureServices(IServiceCollection services)
{
    // Configure Api Behavior Options
    services.ConfigureOAuthAuthentication(_configuration)
        .AddApiDocsOAuthAuthentication();
}
```