using Confluent.Kafka;
using System;
using System.Runtime.CompilerServices;

[assembly:InternalsVisibleTo("GiG.Core.Messaging.Kafka")]

namespace GiG.Core.Messaging.Kafka.Abstractions.Exceptions
{
    /// <inheritdoc />
    public class KafkaProducerException : SystemException 
    {
    }
}