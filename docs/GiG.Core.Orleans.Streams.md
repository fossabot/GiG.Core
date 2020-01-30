# GiG.Core.Orleans.Streams

This Library provides an API to register an Orleans Stream Factory.

## Basic Usage

### Startup

The below code needs to be added to the `Startup.cs`. This will register the Stream Factory.

```csharp
public static void ConfigureServices(IServiceCollection services)
{
    services.AddStream();
}    
```

### Usage within a Grain

The below code shows how to inject `IStreamFactory` in a class.

```csharp
public class WalletGrain : Grain, IWalletGrain
{
    public WalletGrain(IStreamFactory streamFactory)
    {
        _streamFactory = streamFactory;
    }

    //Other Code...
}
```

The below code shows how to get an instance of the `Stream` class from the `StreamFactory`.

```csharp
public override async Task OnActivateAsync()
{
    var streamProvider = GetStreamProvider("SMSProvider");
    _stream = _streamFactory.GetStream<WalletTransaction>(streamProvider, this.GetPrimaryKey(), "WalletTransactions");
}
```
 
Calling the 'PublishAsync' method will publish messages on the `Stream` class instance.

```csharp
await _stream.PublishAsync(new WalletTransaction());
```