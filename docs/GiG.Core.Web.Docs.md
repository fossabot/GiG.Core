# GiG.Core.Web.Docs

This Library provides an API to configure API Documentation.

## Basic Usage

The below code needs to be added to the `Startup.cs` to register the API Docs. 

```csharp
private readonly IConfiguration _configuration;

public void ConfigureServices(IServiceCollection services)
{
    services.ConfigureApiDocs(_configuration);
}

public void Configure(IApplicationBuilder app)
{
    app.UseApiDocs();
}
```

## Configuration

The below table outlines the valid Configurations used to override the [ApiDocsOptions](../src/GiG.Core.Web.Docs.Abstractions/ApiDocsOptions.cs).

| Configuration Name    | Type	  | Required | Default Value	|
|-----------------------|---------|----------|------------------|
| IsEnabled             | Boolean | No       | `true`           | 
| Url                   | String  | No       | `api-docs`      |
| Title                 | String  | No       | <null>           |
| Description           | String  | No       | <null>           | 
| IsForwardedForEnabled | Boolean | No       | `true`           |
