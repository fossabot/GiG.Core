# GiG.Core.Data.KVStores.Providers.Etcd

This Library provides an API to register data providers which will read data from etcd.

## Basic Usage

The below code needs to be added to the `Startup.cs`. Making use of `AddJsonEtcd` will register a data provider which will read from etcd, parse from JSON to a model and store in the registered `IDataStore<T>`.
 
```csharp
	
public static void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
{
    var configuration = hostContext.Configuration;
    
    services
        .AddKVStores<IEnumerable<MyModel>>()
        .FromJsonEtcd(configuration, "MyData");
}

```

### Configuration

The below table outlines the valid Configurations used to configure the [EtcdProviderOptions](..\src\GiG.Core.Data.KVStores.Providers.Etcd\Abstractions\EtcdProviderOptions.cs). The options do not define a default configuration section, the needs to be provided when registering an etcd provider.

| Configuration Name | Type   | Optional | Default Value            |
|:-------------------|:-------|:---------|:-------------------------|
| ConnectionString   | String | No       |                          |
| Key                | String | No       |                          |