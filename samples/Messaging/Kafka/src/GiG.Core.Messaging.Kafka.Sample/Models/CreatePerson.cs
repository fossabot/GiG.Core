using Bogus;
using System;

namespace GiG.Core.Messaging.Kafka.Sample.Models
{
    public class CreatePerson
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }
        public Address Address { get; set; }
        
        public override string ToString() => $"{nameof(Id)}: {Id}, {nameof(Name)}: {Name}, {nameof(Surname)}: {Surname}, {nameof(Age)}: {Age}, {nameof(Address)}: {Address}";

        
        public static CreatePerson Generate
        {
            get
            {
                var fakePerson = new Faker().Person;
                return new CreatePerson
                {
                    Id = Guid.NewGuid(),
                    Name = fakePerson.FirstName,
                    Surname = fakePerson.LastName,
                    Age = 35,
                    Address = new Address
                    {
                        Street = fakePerson.Address.Street,
                        State = fakePerson.Address.State,
                        City = fakePerson.Address.City,
                        Zip = fakePerson.Address.ZipCode
                    }
                };
            }
        }
    }
}