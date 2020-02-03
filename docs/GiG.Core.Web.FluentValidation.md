# GiG.Core.Web.FluentValidation

This Library provides an API to register the Fluent Validation Exception Middleware in an application.

## Basic Usage

The below code needs to be added to the `Startup.cs` class. This will register the Fluent Validation Exception Middleware.

**Note**: The `ConfigureApiBehaviorOptions` method call needs to be the last one in the `ConfigureServices` method.

```csharp
public void ConfigureServices(IServiceCollection services)
{
    // Configure Api Behavior Options
    services.ConfigureApiBehaviorOptions();
}

public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    app.UseFluentValidationMiddleware();
}
```