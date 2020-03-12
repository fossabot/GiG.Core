# GiG.Core.Orleans.Clustering.Consul

This Library provides an API to use Consul as a Membership Provider for Orleans Silos.

## Basic Usage

### Registering an Orleans Client

The below code needs to be added to the `Startup.cs`. This will register an Orleans Client running on Consul.
**Note**: The `ConfigureCluster` and `AddDefaultClusterClient` extensions can be found in the nuget package ```GiG.Core.Orleans.Client```

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddDefaultClusterClient((x, sp) =>
    {              
        x.ConfigureCluster(_configuration);
        x.UseMembershipProvider(_configuration, y =>
        {
            y.ConfigureConsulClustering(_configuration);
        });
        x.AddAssemblies(typeof(ITransactionGrain));
    });
}
```

### Registering an Orleans Silo

The below code needs to be added to the `Program.cs`. This will register an Orleans Silo running on Consul.
**Note**: The `ConfigureCluster` extension can be found in the nuget package ```GiG.Core.Orleans.Silo```
**Note**: The `UseOrleans` extension can be found in the nuget package ```Microsoft.Orleans.Server```

```csharp
static class Program
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
            .UseMembershipProvider(ctx.Configuration, x =>
            {
                x.ConfigureConsulClustering(ctx.Configuration);
            });
            .AddAssemblies(typeof(Grain));
    }
}    
```

### Configuration

The below table outlines the valid Configurations used to override the [ConsulOptions](../src/GiG.Core.Orleans.Clustering.Consul/Abstractions/ConsulOptions.cs) under the Config section `Orleans:Consul`.

| Configuration Name | Type   | Optional | Default Value            |
|:-------------------|:-------|:---------|:-------------------------|
| Address            | String | Yes      | `http://localhost:8500"` |
| KvRootFolder       | String | Yes      | `dev`                    |

#### Sample Configuration

```json
{
  "Orleans": {
    "MembershipProvider": {
      "Name": "Consul"
    }
  }
}
```