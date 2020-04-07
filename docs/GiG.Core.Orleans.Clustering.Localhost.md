# GiG.Core.Orleans.Clustering.Localhost

This Library provides an API to configure **Development-Only** clustering and listen on localhost.

## Basic Usage

### Registering an Orleans Client

The below code needs to be added to the `Startup.cs`. This will register an Orleans Client running on the Localhost.

**Note**: The `ConfigureCluster` and `AddDefaultClusterClient` extensions can be found in the nuget package ```GiG.Core.Orleans.Client```

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddDefaultClusterClient((x, sp) =>
    {              
        x.ConfigureCluster(_configuration);
        x.UseMembershipProvider(_configuration, y => 
        {
            y.ConfigureLocalhostClustering(_configuration); 
        });
        x.AddAssemblies(typeof(ITransactionGrain));
    });
}
```

### Registering an Orleans Silo

The below code needs to be added to the `Program.cs`. This will register an Orleans Silo running on the Localhost.

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
            .UseMembershipProvider(ctx.Configuration, y =>
            {
                y.ConfigureLocalhostClustering(ctx.Configuration);
            });
            .AddAssemblies(typeof(Grain));
    }
}    
```

### Configuration

#### Silo

The `ConfigureLocalhostClustering` extension accepts the following parameters. You can also use the `IConfiguration` overload to load the following configuration from `appsettings.json` under the Config section `Orleans:Cluster` and `Orleans:Silo`. 

| Configuration Name | Type       | Required | Default Value |
|:-------------------|:-----------|:---------|:--------------|
| SiloPort           | Int        | No       | 11111         |
| GatewayPort        | Int        | No       | 30000         |
| PrimarySiloEndpoint| IPEndPoint | No       | null          |
| ClusterId          | String     | No       | `dev`         |
| ServiceId          | String     | No       | `dev`         |

#### Client

The `ConfigureLocalhostClustering` extension accepts the following parameters. You can also use the `IConfiguration` overload to load the following configuration from `appsettings.json` under the Config section `Orleans:Cluster` and `Orleans:Silo`.

| Configuration Name | Type   | Required | Default Value |
|:-------------------|:-------|:---------|:--------------|
| GatewayPort        | Int    | No       | 30000         |
| ClusterId          | String | No       | `dev`         |
| ServiceId          | String | No       | `dev`         |

**Note**:
The above configurations can be mapped from:
[ClusterOptions](https://github.com/dotnet/orleans/blob/master/src/Orleans.Core/Configuration/Options/ClusterOptions.cs)
and [EndpointOptions](https://github.com/dotnet/orleans/blob/master/src/Orleans.Runtime/Hosting/EndpointOptions.cs)
