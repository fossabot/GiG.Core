# GiG.Core.Orleans.Storage.Npgsql

This Library provides an API to register Postgres as a Grain Storage Provider.

## Basic Usage

The below code needs to be added to the `Program.cs` when creating a new HostBuilder.

Add the below to your Program.cs and this will register an Orleans Silo with Postgres Grain Storage provider named 'NpgsqlProvider'.

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

You can register multiple named Npgsql Grain Storage provider, just make sure the storage provider name is unique.


### Configuration

By default the configuration for the provider is expected to be under the section "Orleans:StorageProviders:{InstanceName}". 

The below table outlines the valid Configurations for [NpgsqlOptions](../src/GiG.Core.Orleans.Storage.Npgsql/Configurations/NpgsqlOptions.cs).

| Configuration Name | Type   | Required | Default Value |
|:-------------------|:-------|:---------|:--------------|
| ConnectionString   | String | Yes      |               |
