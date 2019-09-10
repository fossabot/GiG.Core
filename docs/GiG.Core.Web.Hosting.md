# GiG.Core.Web.Hosting

This Library provides an API to configure BASE_PATH and configure Forwarded Headers.

## Path Base

Add the below to your Startup class and this will configure the Base Path.
 
```csharp
    public void Configure(IApplicationBuilder app)
    {   
        app.ConfigurePathBase();
    }
```
By default the Base Path will be retrieved from the configuration key `BASE_PATH`.  The name of the configuration key can be overridden by using the extension method which accepts the config key Name as a parameter.  

```csharp
    app.ConfigurePathBase("configKeyName");
```

##  Forwarded Headers

Add the below to your Startup class and this will configure the Forwarded Headers.
 
```csharp
    public void ConfigureServices(IServiceCollection services)
    {
        services.ConfigureForwardedHeaders();
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseForwardedHeaders();
    }
```

