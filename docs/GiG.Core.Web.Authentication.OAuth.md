﻿# GiG.Core.Web.Authentication.OAuth

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
	app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    });
}
```

### Configuration

The below table outlines the valid Configurations used to configure the [OAuthAuthenticationOptions](../src/GiG.Core.Web.Authentication.OAuth.Abstractions/OAuthAuthenticationOptions.cs).

| Configuration Name        | Type    | Optional | Default Value    |
|:--------------------------|:--------|:---------|:-----------------|
| Authority                 | String  | No       |                  |
| ApiName                   | String  | Yes      |                  |
| ApiSecret                 | String  | Yes      |                  |
| Scopes                    | String  | Yes      |                  |
| SupportedTokens           | String  | Yes      | `JWT`            |
| RequireHttpsMetadata      | Boolean | Yes      | true             |
| LegacyAudienceValidation  | Boolean | Yes      |                  |

#### Sample Configuration

```json
{
  "Authentication": {
    "OAuth": {
      "Authority": "http://localhost:7070",
      "ApiName": "sample-web",
      "RequireHttpsMetadata": false,
      "Scopes": "openid profile"
    }
  }
}
```