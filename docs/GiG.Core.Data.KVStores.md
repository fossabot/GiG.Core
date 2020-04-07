# GiG.Core.Data.KVStores

This Library provides an API to register the required services needed by the KV Stores Data Providers.

## Basic Usage

The below code needs to be added to the `Startup.cs`. This will register defaults such as a memory store as an `IDataStore<T>` and an `IDataRetriever<T>`. Also, if the serialisation is not specified, JSON will be registered as default.

The `IDataRetriever<T>` can be used to get the data required from store.
 
```csharp
public void ConfigureServices(IServiceCollection services)
{
    services
        .AddKVStores<IEnumerable<MyModel>>(x =>
            x.FromFile(configuration, "MyConfigSection")
                .WithEagerLoading());
}
```