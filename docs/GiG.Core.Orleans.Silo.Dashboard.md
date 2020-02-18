# GiG.Core.Orleans.Silo.Dashboard

This Library provides an API to register the Orleans Silo Dashboard in an application.

## Basic Usage

The below code needs to be added to the `Program.cs` when creating a new HostBuilder.
**Note**: The `UseOrleans` extension can be found in the nuget package ```Microsoft.Orleans.Server```

```csharp
public static void Main(string[] args)
{
    CreateHostBuilder(args).Build().Run();
}

public static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .UseOrleans((hostBuilder, siloBuilder) =>
            siloBuilder.ConfigureDashboard(hostBuilder.Configuration));
```

### Configuration

The below table outlines the valid Configurations used to override the [DashboardOptions](../src/GiG.Core.Orleans.Silo/Abstractions/DashboardOptions.cs) under the Config section `Dashboard`.

| Configuration Name      | Type    | Required                  | Default Value |
|:------------------------|:--------|:--------------------------|:--------------|
| IsEnabled               | Boolean | No                        | `true`        |
| Port                    | String  | Yes (If HostSelf is true) | `8080`        |
| Path                    | String  | No                        | `/dashboard`  |
| HostSelf                | Boolean | No                        | `false`       |
| CounterUpdateIntervalMs | Int     | No                        | 10_000        |
| Username                | String  | No                        |               |
| Password                | String  | Yes (If Username is true) |               |
| HideTrace               | Boolean | No                        | `false`       |

#### Sample Configuration

```json
{
  "Dashboard": {
    "HostSelf": true
  }
}
 ```

### Co-Hosting the Dashboard in Web Application

The Orleans Dashboard can be hosted in its own web server using Kestrel or co-hosted in the same host of the application.
This can be controlled using the `HostSelf` option. When set to 'false', the below code enables the dashboard to be configured on the application.

```csharp
public void Configure(IApplicationBuilder app)
{         
    app.UseDashboard();              
}
```