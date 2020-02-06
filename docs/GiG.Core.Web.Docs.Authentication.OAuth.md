# GiG.Core.Web.Docs.Authentication.OAuth

This Library provides an API to configure OAuth2.0 as a protocol for Authentication when accessing API Documentation.

## Basic Usage

The below code needs to be added to the `Startup.cs` class. This will configure the Authentication protocol for Swagger as OAuth2.0.
**Note**: The `ConfigureOAuthAuthentication` extension can be found in the nuget package ```GiG.Core.Web.Authentication.OAuth```

```chsarp
public void ConfigureServices(IServiceCollection services)
{
    // Configure Api Behavior Options
    services.ConfigureOAuthAuthentication(_configuration)
        .AddApiDocsOAuthAuthentication();
}
```

### Configuration

The below table outlines the valid Configurations used to configure the [OAuthAuthenticationOptions](../src/GiG.Core.Web.Authentication.OAuth.Abstractions/OAuthAuthenticationOptions.cs).

| Configuration Name        | Type    | Optional | Default Value    |
|:--------------------------|:--------|:---------|:-----------------|
| IsEnabled                 | Boolean | Yes      | true             |
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
      "IsEnabled": true,
      "Authority": "http://localhost:7070",
      "ApiName": "sample-web",
      "RequireHttpsMetadata": false,
      "Scopes": "openid profile"
    }
  }
}
```