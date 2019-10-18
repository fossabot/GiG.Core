# GiG.Core.Orleans.Streams

This Library provides an API to register the Stream Factory.

## Basic Usage

The below code needs to be added to the `Startup.cs` to register the Stream Factory.

```csharp
		
    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddStreamFactory();
    }
              
```

## Usage with a Grain

Inject the IStreamFactory interface within your grains as show in the code below (using WalletGrain as an example).

```csharp

    public WalletGrain(IStreamFactory streamFactory)
    {
        _streamFactory = streamFactory;
    }

```

On Activation of the Grain within the 'OnActivateAsync()' method add the following to get an instance of the stream from the StreamFactory.

```csharp

     _stream = _streamFactory.GetStream<WalletTransaction>(streamProvider, this.GetPrimaryKey(), "WalletTransactions");
           
```
