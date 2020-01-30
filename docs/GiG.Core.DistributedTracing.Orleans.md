# GiG.Core.DistributedTracing.Orleans

This Library provides an API to register Distributed Tracing for an Orleans Client.

## Basic Usage - Silo

The below code needs to be added to the `Program.cs`. Make use of `AddCorrelationAccessor()` when creating an `IHostBuilder` and this will register the Correlation Id context accessor.

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
            .ConfigureLogging()
            .ConfigureServices(Startup.ConfigureServices)
            .UseOrleans(Startup.ConfigureOrleans);
}
```

## Basic Usage - Client

Add the below to your Startup class and this will add the Correlation Id Grain call filter.

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