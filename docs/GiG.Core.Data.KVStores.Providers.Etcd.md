# GiG.Core.Data.KVStores.Providers.Etcd

This Library provides an API to register data providers which will read data from etcd.

## Basic Usage

The below code needs to be added to the `Startup.cs`. Making use of `FromEtcd` will register a data provider which will read from etcd. By default, the model will be parsed from JSON to a model and store in the registered `IDataStore<T>`.
 
```csharp
public static void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
{
    var configuration = hostContext.Configuration;
    
    services
        .AddKVStores<IEnumerable<MyModel>>(x =>
          x.FromEtcd(configuration, "MyConfigSection"));
}
```

### Configuration

The below table outlines the valid Configurations used to configure the [EtcdProviderOptions](../src/GiG.Core.Data.KVStores.Providers.Etcd/Abstractions/EtcdProviderOptions.cs) The options do not define a default configuration section, that needs to be provided when registering an etcd provider.

| Configuration Name | Type   | Optional | Default Value            |
|:-------------------|:-------|:---------|:-------------------------|
| ConnectionString   | String | Yes      | http://localhost:2379    |
| Port               | Integer| Yes      | 2379                     |
| Username           | String | Yes      | ''                       |
| Password           | String | Yes      | ''                       |
| CaCertificate      | String | Yes      | ''                       |
| ClientCertificate  | String | Yes      | ''                       |
| ClientKey          | String | Yes      | ''                       |
| IsPublicRootCa     | Boolean| Yes      | false                    |
| Key                | String | No       |                          |

#### Sample Configuration

```json
{
  "MyConfigSection": {
    "ConnectionString": "http://localhost:2379",
    "Key": "languages"
  }
}
```