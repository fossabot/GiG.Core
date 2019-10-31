# GiG.Core.Data.KVStores.Providers.FileProviders

This Library provides an API to register data providers which will read data from file.

## Basic Usage

The below code needs to be added to the `Startup.cs`. Making use of `AddJsonFile` will register a data provider which will read from file, parse from JSON to a model and store in the registered `IDataStore<T>`.
 
```csharp
	
public static void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
{
    var configuration = hostContext.Configuration;
    
    services
        .AddKVStores<IEnumerable<MyModel>>()
        .FromJsonFile(configuration, "MyData");
}

```

### Configuration

The below table outlines the valid Configurations used to configure the [FileProviderOptions](..\src\GiG.Core.Data.KVStores.Providers.FileProviders.Abstractions\FileProviderOptions.cs). The options do not define a default configuration section, the needs to be provided when registering a file provider.

| Configuration Name | Type   | Optional | Default Value            |
|:-------------------|:-------|:---------|:-------------------------|
| Path               | String | No       |                          |