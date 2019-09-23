# GiG.Core.Orleans.Clustering.Consul.Silo

This Library provides an API to register an Orleans Silo running on Consul.

## Basic Usage

Add the below to your Startup class and this will register an Orleans Silo running on Consul.

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

You can change the default value for the Consul configuration by overriding the [ConsulOptions](..\src\GiG.Core.Orleans.Clustering.Consul.Silo\Configurations\ConsulOptions.cs) by adding the following configuration settings under section `Orleans:Consul`.

| Configuration Name | Type   | Optional | Default Value            |
|:-------------------|:-------|:---------|:-------------------------|
| Address            | String | Yes      | `http://localhost:8500"` |
| KvRootFolder       | String | Yes      | `dev`                    |
