# GiG.Core.Validation.FluentValidation.Web

This Library provides an API to register the MediatR Pipeline Behaviour which uses Fluent Validation.

## Basic Usage

The below code needs to be added to the `Startup.cs` class. This will add the MediatR Pipeline Behaviour.

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddValidationPipelineBehavior();
}
```