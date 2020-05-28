using System;

namespace GiG.Core.Messaging.Kafka.Sample.Models
{
    public class PersonCreated
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string DateOfBirth { get; set; }
        public Address Address { get; set; }
    }
}