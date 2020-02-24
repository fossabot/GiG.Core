# GiG.Core.Orleans.Reminders.Npgsql

This Library provides an API to register PostgreSQL as a Reminder Storage Provider.

## Pre-requisites

The following packages are required to consume this package:
 - [GiG.Core.Orleans.Silo](GiG.Core.Orleans.Silo.md)
 - Microsoft.Orleans.Server
 
## Basic Usage

The below code needs to be added to the `Program.cs` when creating a new HostBuilder. This will register an Orleans Silo with PostgreSQL Reminder Storage provider named 'NpgsqlProvider'.

**Note**: Multiple named Npgsql Reminder Storage providers can be added as long as the Provider Name is unique.

**Note**: The `UseOrleans` extension can be found in the nuget package ```Microsoft.Orleans.Server```
**Note**: The `ConfigureCluster` and `ConfigureEndpoints` extensions can be found in the nuget package ```GiG.Core.Orleans.Silo```

```csharp
public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)                                
            .ConfigureServices(Startup.ConfigureServices)
            .UseOrleans(ConfigureOrleans);

    public static void ConfigureOrleans(HostBuilderContext ctx, ISiloBuilder builder)
    {
        builder.ConfigureCluster(ctx.Configuration)
            .ConfigureEndpoints(ctx.Configuration)
            .AddAssemblies(typeof(Grain))
			 .UseNpgsqlReminderService("SampleDb", ctx.Configuration);
    }
}
```

### Configuration

The below table outlines the valid Configurations for [NpgsqlOptions](../src/GiG.Core.Orleans.Reminders.Npgsql/Abstractions/NpgsqlOptions.cs). By default the Configuration for the provider is expected to be under the section "Orleans:Reminders:{InstanceName}". 

| Configuration Name | Type   | Required | Default Value |
|:-------------------|:-------|:---------|:--------------|
| ConnectionString   | String | Yes      |               |

#### Sample Configuration

```json
{
  "Orleans": {
    "Reminders": {
      "SampleDb": {
        "ConnectionString": "Host=localhost;Username=postgres;Password=postgres;Database=sample"
      }
    }
  }
}
```