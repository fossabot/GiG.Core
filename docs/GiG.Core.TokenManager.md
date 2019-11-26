# GiG.Core.TokenManager

This Library provides an API to register required services for the Token Manager. Note that the Token Manager needs an Auth Server to support it as it does not 
act as an authority to generate tokens.

## Basic Usage

The below code needs to be added to the `Startup.cs`. This will register the two factories required for Token Manager functionality added.

```csharp

public void ConfigureServices(IServiceCollection services)
{
    services.AddTokenManager();
}

```