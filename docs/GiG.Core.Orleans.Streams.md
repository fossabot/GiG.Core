# GiG.Core.Orleans.Streams

This Library provides an API to register an Orleans Stream Factory.

## Stream Helpers

### Basic Usage

#### Startup

The below code needs to be added to the `Startup.cs`. This will register the Stream Factory.

```csharp
public static void ConfigureServices(IServiceCollection services)
{
    services.AddStream();
}    
```

#### Usage within a Grain

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

## Command Dispatcher

### Basic Usage

#### Startup

The below code needs to be added to the `Startup.cs`. This will register the Command Dispatcher Factory.

```csharp
public static void ConfigureServices(IServiceCollection services)
{
    services.AddCommandDispatcher();
}    
```

The below code shows how to get an instance of the `CommandDispatcher` class from the `CommandDispatcherFactory`

```csharp
var grainId = Guid.NewGuid();
_commandDispatcherFactory.Create(grainId, "SMSProvider")
```

The below code shows how to setup and dispatch the command, and wait for Success or Failure response. 
The `Create()` creates a new instance of `ICommandDispatcher`.
The `WithCommand()` appends a command to the instance of `ICommandDispatcher` given a command namespace. 
The `WithSuccessEvent()` appends a success event to the instance of `ICommandDispatcher` given a success event namespace and subscribes to the success stream.
The `WithFailureEvent()` appends a failure event to the instance of `ICommandDispatcher` given a failure event namespace and subscribes to the failure stream.
The `SubscribeAsync()` subscribes to Success and Failure Events.
The `DispatchAsync()` dispatches the command and handle the respective responses.

```csharp
var grainId = Guid.NewGuid();
await using (var commandDispatcher = _commandDispatcherFactory.Create(grainId, "SMSProvider")
						 .WithCommand(new TestCommand(), TestCommand.TestCommandNamespace)
                		 .WithSuccessEvent(TestSuccessEvent.TestSuccessEventNamespace)
                     	 .WithFailureEvent(TestFailureEvent.TestFailureEventNamespace))
{
    await commandDispatcher.SubscribeAsync(); 
    var response = await commandDispatcher.DispatchAsync(5000);             
}
```

## Telemetry

The custom [Stream](../src/GiG.Core.Orleans.Streams/Stream.cs) implementation includes Telemetry on Publishing and Consuming from a Stream. 

N.B. In order to have Telemetry information from Streams a Telemetry provider needs to be enabled. For more details refer to [GiG.Core.DistributedTracing.OpenTelemetry.Exporters.Jaeger](GiG.Core.DistributedTracing.OpenTelemetry.Exporters.Jaeger.md)

### Publishing to a Stream

The below code shows how to Publish to a Stream via the custom [Stream](../src/GiG.Core.Orleans.Streams/Stream.cs) implementation.  

```csharp
var streamProvider = GetStreamProvider("SMSProvider");
_stream = _streamFactory.GetStream<WalletTransaction>(streamProvider, this.GetPrimaryKey(), "WalletTransactions");
var walletTransactionModel = new WalletTransaction
{
    Amount = context.Message.Amount,
};

await _stream.PublishAsync(transactionModel);
```

### Consuming from a Stream

The below code shows how to Subscribe to a Stream via the custom [Stream](../src/GiG.Core.Orleans.Streams/Stream.cs) implementation.  

N.B. The parameter to the SubscribeAsync method must be of the type IAsyncObserver<T>, so for the example below ```IAsyncObserver<WalletTransaction>```. 

```csharp
var streamProvider = GetStreamProvider("SMSProvider");
_stream = _streamFactory.GetStream<WalletTransaction>(streamProvider, this.GetPrimaryKey(), "WalletTransactions");
await _stream.SubscribeAsync(this);
```

## Namespace Prefix

### Configuration

You can change the default value for the Kafka configuration by overriding the [StreamOptions](../src/GiG.Core.Orleans.Streams.Abstractions/StreamOptions.cs) by adding the following configuration settings under section `Orleans:Streams`.

| Configuration Name | Type   | Optional | Default Value |
|--------------------|--------|----------|---------------|
| NamespacePrefix    | String | No       |               |

#### Sample Configuration

```
{
  "Orleans": {
    "Streams": {
      "NamespacePrefix": "dev"
    }
  }
}
```

### Stream Helper for Namespace

A helper can be used to construct the namespace including the `NamespacePrefix`.

#### Sample Usage

```
// dev.message-type
StreamHelper.GetNamespace("message-type");

// dev.domain.message-type.v1
StreamHelper.GetNamespace("domain", "message-type");

// dev.domain.message-type.v2
StreamHelper.GetNamespace("domain", "message-type", 2);
```

### Namespace Implicit Stream Subscription

The `NamespaceImplicitStreamSubscription` is used for implcit subscriptions using the specified stream namespace including the `NamespacePrefix`.

```
// Same as [ImplicitStreamSubscription("dev.domain.message-type.v1")]
[NamespaceImplicitStreamSubscription("domain", "message-type")]
public class MockStreamGrain : Grain, IMockStreamGrain, IAsyncObserver<MockRequest>
{
    public override Task OnActivateAsync()
    {
        var streamProvider = GetStreamProvider(Constants.StreamProviderName);
        var mockRequestStream = streamProvider.GetStream<MockRequest>(this.GetPrimaryKey(), StreamHelper.GetNamespace("domain", "message-type"));
        mockRequestStream.SubscribeAsync(this);
    }

    ...
}
```