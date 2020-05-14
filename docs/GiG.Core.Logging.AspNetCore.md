# GiG.Core.Logging.AspNetCore

This Library provides an API to register the `HttpRequestResponseLoggingMiddleware`. It is used to log Http Requests and Http Responses.

## Basic Usage

The below code needs to be added to the `Startup.cs`.
 
```csharp
private readonly IConfiguration _configuration;

public void ConfigureServices(IServiceCollection services)
{
    services.ConfigureHttpRequestResponseLogging(_configuration);
}

```csharp
public void Configure(IApplicationBuilder app)
{   
    app.UseRouting();
    app.UseHttpRequestResponseLogging();
    app.UseEndpoints(endpoints =>
	{
		endpoints.MapControllers();
	});
}
```

### Configuration

The below table outlines the valid Configurations used to override the [HttpRequestReponseLoggingOptions](../src/GiG.Core.Logging.AspNetCore/Abstractions/HttpRequestResponseLoggingOptions.cs) under the Config section `Logging:HttpRequestReponse`.

| Configuration Name        | Type    | Required                  | Default Value |
|:--------------------------|:--------|:--------------------------|:--------------|
| IsEnabled                 | Boolean | No                        | `false`       |
| Request:IsEnabled         | Boolean | No                        | `true`        |
| Request:IncludeHeaders    | Boolean | No                        | `true`        |
| Request:IncludeBody       | Boolean | No                        | `true`        |
| Response:IsEnabled        | Boolean | No                        | `true`        |
| Response:IncludeHeaders   | Boolean | No                        | `true`        |
| Response:IncludeBody      | Boolean | No                        | `true`        |

#### Sample Configuration

```json
{
  "Logging": {     
    "HttpRequestResponse": {
      "IsEnabled" : true,
      "Request": {
        "IsEnabled": true,
        "IncludeHeaders": false
      },
      "Response": {
        "IsEnabled": false
      }
    }
  }
}
 ```