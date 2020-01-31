# GiG.Core.Hosting

This Library provides an API to register hosting related functionailty to an application.

## Application Metadata

### Basic Usage

The below code needs to be added to the `Program.cs`. This will register the application metadata accessor.

```csharp
static class Program
{
    public static void Main()
    {
        CreateHostBuilder().Build().Run();
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .UseApplicationMetadata();
}
```

### Configuration

The `ApplicationName` should be configured from the `appsettings.json`. By default it will use the Assembly's Entrypoint name

## Info Management Endpoint

### Basic Usage

The below code needs to be added to the `Startup.cs` to register an information endpoint.

```csharp
public void ConfigureServices(IServiceCollection services)
{
	services.ConfigureInfoManagement(_configuration);
}

public void Configure(IApplicationBuilder app)
{           
	app.UseInfoManagement();           
}
```

### Configuration

The below table outlines the valid Configurations.

| Configuration Name  | Type	| Optional | Default Value	   |
|:-------------------|:-------|:---------|:-----------------|
| Url				  | String  | No	   | `/actuator/info`  |
| Checksum            | [InfoManagementChecksumOptions](../src/GiG.Core.Hosting.Abstractions/InfoManagementChecksumOptions.cs) | Yes      |                          |

#### Sample Configuration

```json
{
  "InfoManagement": {
    "Url": "/info"       
  }
}
```