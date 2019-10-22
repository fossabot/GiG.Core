# GiG.Core.Orleans.Storage.Npgsql

This Library provides an API to register PostgreSQL as a Grain Storage Provider.

## Basic Usage

The below code needs to be added to the `Program.cs` when creating a new HostBuilder. This will register an Orleans Silo with PostgreSQL Grain Storage provider named 'NpgsqlProvider'.

**Note**: Multiple named Npgsql Grain Storage providers can be added as long as the Provider Name is unique.

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
            .ConfigureEndpoints()
            .AddAssemblies(typeof(Grain))
			 .AddNpgsqlGrainStorage("NpgsqlProvider", ctx.Configuration);
    }
}
      
```

### Configuration

The below table outlines the valid Configurations for [NpgsqlOptions](../src/GiG.Core.Orleans.Storage.Npgsql/Configurations/NpgsqlOptions.cs). By default the Configuration for the provider is expected to be under the section "Orleans:StorageProviders:{InstanceName}". 

| Configuration Name | Type   | Required | Default Value |
|:-------------------|:-------|:---------|:--------------|
| ConnectionString   | String | Yes      |               |
