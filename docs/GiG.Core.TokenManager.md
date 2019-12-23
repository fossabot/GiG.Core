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

### Configuration

The below table outlines the valid Configurations used to configure the [TokenManagerOptions](..\src\GiG.Core.TokenManager\Models\TokenManagerOptions.cs). 
The default configuration section is set to 'TokenManager'.


| Configuration Name | Type                                                                            | Optional | Default Value |
|:-------------------|:--------------------------------------------------------------------------------|:---------|:--------------|
| Username           | String                                                                          | No       |               |
| Password           | String                                                                          | No       |               |
| Client             | [TokenClientOptions](..\src\GiG.Core.TokenManager\Models\TokenClientOptions.cs) | No       |               |
