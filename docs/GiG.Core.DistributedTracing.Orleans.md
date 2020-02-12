# GiG.Core.DistributedTracing.Orleans

This Library provides an API to register Distributed Tracing for an Orleans Client.

## Activity
### Silo

The below code needs to be added to the `Program.cs`, to add the Activity Incoming Grain call filter. 
Make use of `AddActivityAccessor()` when creating an `IHostBuilder` and this will register the Activity context accessor.

**Note**: The `AddActivityAccessor` extension can be found in the nuget package [GiG.Core.DistributedTracing.Activity](GiG.Core.DistributedTracing.Activity.md)

**Note**: The `UseOrleans` extension can be found in the nuget package ```Microsoft.Orleans.Server```
```csharp
static class Program
{
    public static void Main()
    {
        CreateHostBuilder().Build().Run();
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((ctx, services) => 
            {
                services.AddActivityAccessor();
            })
            .ConfigureServices(Startup.ConfigureServices)
            .UseOrleans((hostBuilder, siloBuilder) =>
                siloBuilder.ConfigureCluster(hostBuilder.Configuration)
                    .ConfigureEndpoints(hostBuilder.Configuration)
                    .AddActivityIncomingFilter());
}
```


### Client

Add the below to your Startup class and this will add the Activity Outgoing Grain call filter.

**Note**: The `AddActivityAccessor` extension can be found in the nuget package [GiG.Core.DistributedTracing.Activity](GiG.Core.DistributedTracing.Activity.md)

**Note**: The `AddDefaultClusterClient` extension can be found in the nuget package [GiG.Core.Orleans.Client](GiG.Core.Orleans.Client.md)
```csharp
public void ConfigureServices(IServiceCollection services)
{
    // Activty Context Accessor
    services.AddActivityAccessor();

    // Orleans Client
    services.AddDefaultClusterClient((builder, sp) =>
    {
        builder.AddActivityOutgoingFilter(sp);
        builder.ConfigureCluster(_configuration);
        builder.AddAssemblies(typeof(IWalletGrain));
    });
}
```