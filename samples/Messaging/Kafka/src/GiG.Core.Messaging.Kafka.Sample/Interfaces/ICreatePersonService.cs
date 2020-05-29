using GiG.Core.Messaging.Kafka.Abstractions.Interfaces;
using GiG.Core.Messaging.Kafka.Sample.Models;
using System.Threading.Tasks;

namespace GiG.Core.Messaging.Kafka.Sample.Interfaces
{
    public interface ICreatePersonService
    {
        Task HandleMessageAsync(IKafkaMessage<string, CreatePerson> consumerMessage);
    }
}
