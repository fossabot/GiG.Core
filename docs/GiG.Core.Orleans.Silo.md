# GiG.Core.Orleans.Silo

This Library provides an API to register an Orleans Silo in an application.

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
            siloBuilder.ConfigureCluster(hostBuilder.Configuration)
                .ConfigureEndpoints(hostBuilder.Configuration)
                .ConfigureConsulClustering(hostBuilder.Configuration)
                .AddAssemblies(typeof(TransactionGrain)));
```

### Configuration

#### Silo

The below table outlines the valid Configurations used to override the [SiloOptions](../src/GiG.Core.Orleans.Silo/Abstractions/SiloOptions.cs) under the Config section `Orleans:Silo`.

| Configuration Name | Type | Required | Default Value |
|:-------------------|:-----|:---------|:--------------|
| SiloPort           | Int  | No       | 11111         |
| GatewayPort        | Int  | No       | 30000         |

#### Cluster

The below table outlines the valid Configurations used to override the [ClusterOptions](https://github.com/dotnet/orleans/blob/master/src/Orleans.Core/Configuration/Options/ClusterOptions.cs) under the Config section `Orleans:Cluster`.

| Configuration Name | Type   | Required | Default Value |
|:-------------------|:-------|:---------|:--------------|
| ClusterId          | String | No       | `dev`         |
| ServiceId          | String | No       | `dev`         |