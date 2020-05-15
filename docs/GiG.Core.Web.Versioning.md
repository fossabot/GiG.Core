# GiG.Core.Web.Versioning

This Library provides an API to add API Versioning.

## Basic Usage

The below code needs to be added to the `Startup.cs` to register the API Versioning and API Versioned Explorer. 

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddApiExplorerVersioning();
}
```