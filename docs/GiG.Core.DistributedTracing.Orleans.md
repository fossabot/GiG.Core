# GiG.Core.DistributedTracing.Orleans

This Library provides an API to register Distributed Tracing for an Orleans Client.

## Basic Usage - Silo

The below code needs to be added to the `Program.cs`. Make use of `AddCorrelationAccessor()` when creating an `IHostBuilder` and this will register the Correlation Id context accessor.

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
                services.AddCorrelationAccessor();
            })
            .ConfigureServices(Startup.ConfigureServices)
            .UseOrleans(Startup.ConfigureOrleans);
}
```

## Basic Usage - Client

Add the below to your Startup class and this will add the Correlation Id Grain call filter.

**Note**: The `AddDefaultClusterClient` extension can be found in the nuget package [GiG.Core.Orleans.Client](GiG.Core.Orleans.Client.md)
```csharp
public void ConfigureServices(IServiceCollection services)
{
    // Orleans Client
    services.AddDefaultClusterClient((builder, sp) =>
    {
        builder.AddCorrelationOutgoingFilter(sp);
        builder.ConfigureCluster(_configuration);
        builder.AddAssemblies(typeof(IWalletGrain));
    });
}
```