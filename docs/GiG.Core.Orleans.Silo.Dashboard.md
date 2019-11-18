# GiG.Core.Orleans.Silo.Dashboard

This Library provides an API to register the Orleans Silo Dashboard in an application.

## Basic Usage

The below code needs to be added to the `Program.cs` when creating a new HostBuilder.

```csharp
public static void Main(string[] args)
{
    CreateHostBuilder(args).Build().Run();
}

public static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .UseOrleans((hostBuilder, siloBuilder) =>
            siloBuilder.ConfigureDashboard(hostBuilder.Configuration);

```

### Configuration

The below table outlines the valid Configurations used to override the [DashboardOptions](..\GiG.Core.Orleans.Abstractions\Configuration\DashboardOptions.cs) under the Config section `Dashboard`.

| Configuration Name | Type    | Required | Default Value |
|:-------------------|:--------|:---------|:--------------|
| IsEnabled          | Boolean | Yes      | `false`       |
| Port               | String  | Yes      | `8080`        |
| Path               | String  | No       | `dashboard`   |