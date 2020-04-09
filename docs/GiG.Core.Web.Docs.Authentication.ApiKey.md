# GiG.Core.Web.Docs.Authentication.ApiKey

This Library provides an API to add ApiKey for Authentication when accessing API Documentation.

## Basic Usage

The below code needs to be added to the `Startup.cs` class.
**Note**: The `ConfigureOAuthAuthentication` extension can be found in the nuget package ```GiG.Core.Web.Docs```

```chsarp
public void ConfigureServices(IServiceCollection services)
{
    // Configure Api Behavior Options
   services.ConfigureApiKeyOptions(_configuration)
        .AddApiDocsApiKeyAuthentication()
		.AddApiKeyAuthentication();

}
```