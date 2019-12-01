# GiG.Core.Messaging.Kafka

This Library provides an API to register Kafka Producers, Consumers and their dependencies for an application.

## Producer Basic Usage

The below code needs to be added to the `Startup.cs` class. This will register the Producer together with the required Options.

```csharp

public void ConfigureServices(IServiceCollection services)
{
    // Configuration
    services.Configure<KafkaProviderOptions>(_configuration.GetSection(KafkaProviderOptions.DefaultSectionName));
    
    services.AddKafkaProducer<string, Person>(options => options
        .WithJson()
        .FromConfiguration(_configuration)
        .WithTopic("new-person-topic"));
}

```

To consume the Producer, inject the KafkaProducer you registered earlier.

```csharp
public class PersonService
{
    private readonly IKafkaProducer<string, Person> _kafkaProducer;
    
    public PersonService(IKafkaProducer<string, Person> kafkaProducer)
    {
        _kafkaProducer = kafkaProducer ?? throw new ArgumentNullException(nameof(kafkaProducer));
    }

    public async Task CreatePerson()
    {
        var person = Person.Default;
        var messageId = Guid.NewGuid().ToString();
        
        // insert Person to DB if needed.

        var message = new KafkaMessage<string, Person>
        {
            Key = "person",
            Value = person,
            MessageId = messageId,
            MessageType = "Person.Created"
        };

        await _kafkaProducer.ProduceAsync(message);
    }
}

```

## Consumer Basic Usage

The below code can be added to the `Program.cs` class. In this case we are registering it as an IHosted service.
This will register the Consumer together with the required Options and dependencies.

```csharp

private static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureServices((hostContext, services) =>
        {
            var kafkaOptions = hostContext.Configuration
                .GetSection(KafkaProviderOptions.DefaultSectionName)
                .Get<KafkaProviderOptions>();

            services
                .AddOptions()
                .AddScoped(sp => kafkaOptions);

            services
                .AddKafkaConsumer<string, Person>(options => options
                    .WithJson()
                    .FromConfiguration(hostContext.Configuration)
                    .WithTopic("new-person-topic"))
                .AddHostedService<ConsumerService>();
        });

```

To use the Consumer (as an IHostedService), inject the KafkaConsumer you registered earlier.

```csharp
public class ConsumerService : IHostedService
{
    private readonly IKafkaConsumer<string, Person> _consumer;

    public ConsumerService(IKafkaConsumer<string, Person> consumer) => _consumer = consumer ?? throw new ArgumentNullException(nameof(consumer));

    public async Task StartAsync(CancellationToken cancellationToken = default) 
    {
        await Task.Run(() => RunConsumer(cancellationToken), cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken = default)
    {
        _consumer.Dispose();
        return null;
    }
    
    private void RunConsumer(CancellationToken token = default)
    {
        var count = 0;

        try
        {
            while (true)
            {
                try
                {
                    var message = _consumer.Consume(token);
                    HandleMessage(message);

                    if (count++ % 10 == 0)
                    {
                        _consumer.Commit(message);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error occurred: { e.Message } ");
                }
            }
        }
        catch (OperationCanceledException)
        {
            _consumer.Dispose();
        }
    }

    private static void HandleMessage(IKafkaMessage<string, Person> message)
    {
        var serializedValue = JsonConvert.SerializeObject(message.Value);
        Console.WriteLine($"Consumed message in service \nkey: '{ message.Key }' \nvalue: '{ serializedValue }'");

        foreach (var (key, value) in message.Headers)
        {
            Console.WriteLine($"Key: { key }\tValue: { value }");
        }
    }
}

```

### Configuration

The below table outlines the valid Configurations used to override the [KafkaProviderOptions](..\src\GiG.Core.Messaging.Kafka.Abstractions\KafkaProviderOptions.cs) under the default Config section `EventProvider`.

| Configuration Name        | Type                          | Optional | Default Value               |
|:--------------------------|:------------------------------|:---------|:----------------------------|
| BootstrapServers          | string                        | Yes      | `http://localhost:9092`     |
| GroupId                   | string                        | Yes      | `default-group`             |
| Topic                     | string                        | Yes      | `default-topic`             |
| MessageTimeoutMs          | int                           | Yes      | `25000`                     |
| AutoOffsetReset           | AutoOffsetReset               | Yes      | `AutoOffsetReset.Latest`    |
| EnableAutoCommit          | bool                          | Yes      | `false`                     |
| SaslUsername              | string                        | Yes      |                             |
| SaslPassword              | string                        | Yes      |                             |
| SecurityProtocol          | SecurityProtocol              | Yes      | `SecurityProtocol.Plaintext`|
| SaslMechanism             | SaslMechanism                 | Yes      | `SaslMechanism.Plain`       |
| AdditionalConfiguration   | IDictionary<string, string>   | Yes      | `dev`                       |
| SchemaRegistry            | String                        | Yes      | `http://localhost:8081`     |