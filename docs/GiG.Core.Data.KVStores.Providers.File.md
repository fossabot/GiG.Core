# GiG.Core.Data.KVStores.Providers.File

This Library provides an API to register data providers which will read data from file.

## Basic Usage

The below code needs to be added to the `Startup.cs`. Making use of `FromFile` will register a data provider which will read from file. By default, the file will be parsed from JSON to a model and store in the registered `IDataStore<T>`.
 
```csharp
public static void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
{
    var configuration = hostContext.Configuration;
    
    services
        .AddKVStores<IEnumerable<MyModel>>(x =>
          x.FromFile(configuration, "MyConfigSection"));
}
```

### Configuration

The below table outlines the valid Configurations used to configure the [FileProviderOptions](../src/GiG.Core.Data.KVStores.Providers.File/Abstractions/FileProviderOptions.cs). The options do not define a default configuration section, that needs to be provided when registering a file provider.

| Configuration Name | Type   | Optional | Default Value            |
|:-------------------|:-------|:---------|:-------------------------|
| Path               | String | No       |                          |

#### Sample Configuration

```json
{
  "MyConfigSection": {
    "Path": "languages.json"
  }
}
```