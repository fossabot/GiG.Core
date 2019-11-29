using System;

namespace GiG.Core.Messaging.Kafka.Models
{
    public class Person
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }
        public Address Address { get; set; }
        
        public override string ToString() => $"{nameof(Id)}: {Id}, {nameof(Name)}: {Name}, {nameof(Surname)}: {Surname}, {nameof(Age)}: {Age}, {nameof(Address)}: {Address}";

        public static Person Default => new Person
        {
            Id = Guid.NewGuid(),
            Name = "John",
            Surname = "Doe",
            Age = 35,
            Address = new Address
            {
                Street = "830 Prairie Ave.",
                State = "California",
                City = "Los Angeles",
                Zip = "CA 90034"
            }
        };
    }
}