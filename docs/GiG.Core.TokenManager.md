# GiG.Core.TokenManager

This Library provides an API to register required services for the Token Manager. Note that the Token Manager needs an Auth Server to support it as it does not 
act as an authority to generate tokens.

## Basic Usage

The below code needs to be added to the `Startup.cs`. This will register the two factories required for Token Manager functionality added.

```csharp
private readonly IConfiguration _configuration;

public void ConfigureServices(IServiceCollection services)
{
    services.AddTokenManager(_configuration);
}
```

It is also possible to create the [TokenManager](../src/GiG.Core.TokenManager/Implementation/TokenManager.cs) using the factory pattern through the [TokenManagerFactory](../src/GiG.Core.TokenManager/Implementation/TokenManagerFactory.cs). 
This functionality requires no configuration and is enabled by adding the following code to the `Startup.cs`.

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddTokenManagerFactory();
}
```

### Configuration

The below table outlines the valid Configurations used to configure the [TokenManagerOptions](../src/GiG.Core.TokenManager.Abstractions/Models/TokenManagerOptions.cs). 
The default configuration section is set to 'TokenManager'.

| Configuration Name | Type                                                                                         | Optional | Default Value |
|:-------------------|:---------------------------------------------------------------------------------------------|:---------|:--------------|
| Username           | String                                                                                       | No       |               |
| Password           | String                                                                                       | No       |               |
| Client             | [TokenClientOptions](../src/GiG.Core.TokenManager.Abstractions/Models/TokenClientOptions.cs) | No       |               |

#### Sample Configuration

```json
{
  "TokenManager": {
    "Client": {
      "AuthorityUrl": "https://dev.test/api/identity",
      "ClientId": "tokenmanager",
      "ClientSecret": "TokenManager123",
      "Scopes": "openid profile offline_access",
      "RequireHttps": false
    },
    "Username": "username",
    "Password": "password"
  }
}
```