# GiG.Core.Orleans.Client

This Library provides an API to register an Orleans Client in an application.

## Basic Usage

The below code needs to be added to the `Startup.cs`. This will register an Orleans Client.

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddDefaultClusterClient((x, sp) =>
    {
        x.ConfigureCluster(_configuration);              
        x.AddAssemblies(typeof(IGrain));
    });
}
```

### Configuration

The below table outlines the valid Configurations used to override the [ClusterOptions](https://github.com/dotnet/orleans/blob/master/src/Orleans.Core/Configuration/Options/ClusterOptions.cs) under the Config section `Orleans:Cluster`.

| Configuration Name | Type   | Optional | Default Value |
|:-------------------|:-------|:---------|:--------------|
| ClusterId          | String | Yes      | `dev`         |
| ServiceId          | String | Yes      | `dev`         |

## Cluster Client Factory

The [ClusterClientFactory](../src/GiG.Core.Orleans.Client/ClusterClientFactory.cs) can be used to register multiple named Orleans Cluster Clients.

The below code creates, sets up and registers an [ClusterClientFactory](../src/GiG.Core.Orleans.Client/ClusterClientFactory.cs).
Cluster Clients can be added to the factory either via a created instance or an anonymous Func.

```csharp
public void ConfigureServices(IServiceCollection services)
{
    var clusterB = services.CreateClusterClient((builder) =>
        {
            builder.ConfigureCluster(ctx.Configuration.GetSection("Orleans:ClusterB"));
        });

    services.AddClusterClientFactory()
        .AddClusterClient("ClusterA", () =>
        {
            return services.CreateClusterClient((builder) =>
            {
                builder.ConfigureCluster(ctx.Configuration.GetSection("Orleans:ClusterA"));
            });
        })
        .AddClusterClient("ClusterB", clusterB);
}
```

The below code is an example of how the [ClusterClientFactory](../src/GiG.Core.Orleans.Client/ClusterClientFactory.cs) can be used.
A sample usage can also be found in the sample controller [EchoController](../samples/Orleans/src/GiG.Core.Orleans.MultiCluster.Client/Controllers/EchoController.cs).

```csharp
private readonly IClusterClientFactory _clusterClientFactory;

public async Task<string> PingAsync(string clusterName, string graindId)
{
    var clusterClient = _clusterClientFactory.Get(clusterName);
    var grain = clusterClient.GetGrain<IEchoGrain>(grainId); 

    return await grain.Ping();
}
```

### Configuration

If you use the below extension method to configure the Cluster Client, the Builder would expect the Cluster Configuration section to be `Orleans:Cluster:{ClusterName}`.

```csharp
services.CreateClusterClient((builder) =>
{
    builder.ConfigureCluster("ClusterA", _configuration);
});
```

Sample Configuration:

```json
{
    "Orleans": {
        "Cluster": {
            "ClusterA": {
                "ClusterId": "dev1",
                "ServiceId": "sample1"
            },
            "ClusterB": {
                "ClusterId": "dev2",
                "ServiceId": "sample2"
            }
        }
    }
}   
```