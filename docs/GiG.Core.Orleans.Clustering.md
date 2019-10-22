# GiG.Core.Orleans.Clustering

This Library provides Extension Methods to register Orleans Silo Membership Providers.

## Basic Usage

### Registering an Orleans Client

The below code needs to be added to the `Startup.cs`. This will register an Orleans Client running either on Consul or Kubernetes according to the configuration.

```csharp

public void ConfigureServices(IServiceCollection services)
{
    services.AddClusterClient((x, sp) =>
    {              
        x.ConfigureCluster(_configuration);
        x.UseMembershipProvider(_configuration, builder =>
        {
            builder.ConfigureConsulClustering(_configuration);
            builder.ConfigureKubernetesClustering(_configuration);
        });
        x.AddAssemblies(typeof(ITransactionGrain));
    });
}

```

### Registering an Orleans Silo

The below code needs to be added to the `Startup.cs`. This will register an Orleans Silo running either on Consul or Kubernetes according to the configuration.

```csharp

private static void ConfigureOrleans(HostBuilderContext ctx, ISiloBuilder builder)
{
    builder.ConfigureCluster(ctx.Configuration)                
        .ConfigureEndpoints()
        .UseMembershipProvider(ctx.Configuration, x =>
        {
            x.ConfigureConsulClustering(ctx.Configuration);
            x.ConfigureKubernetesClustering(ctx.Configuration);
        })
        .AddAssemblies(typeof(Grain));
}

```

### Configuration

The below table outlines the valid Configurations used to override the [MembershipProviderOptions](..\src\GiG.Core.Orleans.Clustering\MembershipProviderOptions.cs) under the Config section `Orleans:MembershipProvider`.

| Configuration Name | Type   | Optional | Default Value            |
|:-------------------|:-------|:---------|:-------------------------|
| Name               | String | No       | <null>                   |