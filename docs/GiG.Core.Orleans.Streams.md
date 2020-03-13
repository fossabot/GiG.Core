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
The `DispatchAsync()` dispatches the command and handle the respective responses.

```csharp
var grainId = Guid.NewGuid();
await using (var commandDispatcher = _commandDispatcherFactory.Create(grainId, "SMSProvider")
						 .WithCommand(new TestCommand(), TestCommand.TestCommandNamespace)
                		 .WithSuccessEvent(TestSuccessEvent.TestSuccessEventNamespace)
                     	 .WithFailureEvent(TestFailureEvent.TestFailureEventNamespace))
{
    var response = await commandDispatcher.DispatchAsync(5000);             
}
```
