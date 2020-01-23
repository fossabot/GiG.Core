# GiG.Core.Data.KVStores.Providers.Etcd

This Library provides an API to register data providers which will read data from etcd.

## Basic Usage

The below code needs to be added to the `Startup.cs`. Making use of `FromEtcd` will register a data provider which will read from etcd. Using 'WithJsonSerialization' will parse from JSON to a model and store in the registered `IDataStore<T>`.
 
```csharp
	
public static void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
{
    var configuration = hostContext.Configuration;
    
    services
        .AddKVStores<IEnumerable<MyModel>>()
        .FromEtcd(configuration, "MyData")
        .WithJsonSerialization();
}

```

### Configuration

The below table outlines the valid Configurations used to configure the [EtcdProviderOptions](..\src\GiG.Core.Data.KVStores.Providers.Etcd\Abstractions\EtcdProviderOptions.cs). The options do not define a default configuration section, that needs to be provided when registering an etcd provider.

| Configuration Name | Type   | Optional | Default Value            |
|:-------------------|:-------|:---------|:-------------------------|
| ConnectionString   | String | Yes      | http://localhost:2379    |
| Key                | String | No       |                          |