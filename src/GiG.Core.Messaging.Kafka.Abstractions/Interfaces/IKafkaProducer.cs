using System;
using System.Threading.Tasks;

namespace GiG.Core.Messaging.Kafka.Abstractions.Interfaces
{
    public interface IKafkaProducer<TKey, TValue> : IDisposable
    {
        Task ProduceAsync(IKafkaMessage<TKey, TValue> kafkaMessage);
    }
}