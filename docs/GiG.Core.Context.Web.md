# GiG.Core.Context.Web

This Library provides an API to register the Request Context Accessor functionality for a web application.

## Basic Usage

The below code needs to be added to the `Startup.cs`. This will register the Request Context accessor.

```csharp

public void ConfigureServices(IServiceCollection services)
{
    services.AddRequestContextAccessor();
}

```