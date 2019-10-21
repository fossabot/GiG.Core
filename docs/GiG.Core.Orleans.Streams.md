# GiG.Core.Orleans.Streams

This Library provides an API to register the Stream Factory. Using Stream Factory you get instances of Streams in order to publish messages.

## Basic Usage

### Startup

The below code needs to be added to the `Startup.cs` to register the Stream Factory.

```csharp
		
    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddStreamFactory();
    }
              
```

### Usage within a Grain

Inject the IStreamFactory interface within your grains as show in the code below (using WalletGrain as an example).

```csharp

    public WalletGrain(IStreamFactory streamFactory)
    {
        _streamFactory = streamFactory;
    }

```

On Activation of the Grain within the 'OnActivateAsync()' method add the following to get an instance of the stream from the StreamFactory.

```csharp

     var streamProvider = GetStreamProvider("SMSProvider");
     _stream = _streamFactory.GetStream<WalletTransaction>(streamProvider, this.GetPrimaryKey(), "WalletTransactions");
           
```
 
Use the method 'PublishAsync' in order to publish messages on the Stream instance.

```csharp

    await _stream.PublishAsync(new WalletTransaction());

```