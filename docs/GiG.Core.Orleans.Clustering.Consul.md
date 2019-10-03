# GiG.Core.Orleans.Clustering.Consul

This Library provides APIs to register Orleans Clients and Silos running on Consul.

## Basic Usage

### Registering an Orleans Client

Add the below to your Startup class and this will register an Orleans Client running on Consul.

```csharp

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddClusterClient((x, sp) =>
            {              
                x.ConfigureCluster(_configuration);
                x.ConfigureConsulClustering(_configuration);
                x.AddAssemblies(typeof(ITransactionGrain));
            });
	    }

```

### Registering an Orleans Silo

Add the below to your Program.cs and this will register an Orleans Silo running on Consul.

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
                .ConfigureConsulClustering(ctx.Configuration)
                .AddAssemblies(typeof(Grain));
        }
    }
        
```

### Configuration

You can change the default value for the Consul configuration by overriding the [ConsulOptions](..\src\GiG.Core.Orleans.Clustering.Consul\Configurations\ConsulOptions.cs) by adding the following configuration settings under section `Orleans:Consul`.

| Configuration Name | Type   | Optional | Default Value            |
|:-------------------|:-------|:---------|:-------------------------|
| Address            | String | Yes      | `http://localhost:8500"` |
| KvRootFolder       | String | Yes      | `dev`                    |
