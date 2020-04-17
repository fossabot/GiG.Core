# GiG.Core.Logging.AspNetCore

This Library provides an API to register the `HttpRequestResponseLoggingMiddleware`. It is used to log Http Requests and Http Responses.

## Basic Usage

The below code needs to be added to the `Startup.cs`.
 
```csharp
public void Configure(IApplicationBuilder app)
{   
    app.UseHttpRequestResponseLogging();
}
```