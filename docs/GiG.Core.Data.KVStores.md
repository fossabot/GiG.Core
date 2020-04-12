# GiG.Core.Data.KVStores

This Library provides an API to register the required services needed by the KV Stores Data Providers.

## Basic Usage

The below code needs to be added to the `Startup.cs`. This will register defaults such as a memory store as an `IDataStore<T>`, an `IDataRetriever<T>` and an `IDataWriter<T>`. Also, if the serialisation is not specified, JSON will be registered as default.

The `IDataRetriever<T>` can be used to get the data required from store.
The `IDataWriter<T>` can be used to store the data required from store using built-in locking mechanism.
 
```csharp
public void ConfigureServices(IServiceCollection services)
{
    services
        .AddKVStores<IEnumerable<MyModel>>(x =>
            x.FromFile(configuration, "MyConfigSection")
                .WithEagerLoading());
}
```