# GiG.Core.Hosting.AspNetCore

This Library provides an API to register an information endpoint using the EndpointRouteBuilder.

### Basic Usage

The below code needs to be added to the `Startup.cs` to register an information endpoint.

```csharp
public void Configure(IApplicationBuilder app)
{           
	 app.UseEndpoints(endpoints => { 
        endpoints.MapInfoManagement();
    });       
}
```

### Configuration

For Configuration details refer to [GiG.Core.Hosting](GiG.Core.Hosting.md).