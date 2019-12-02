using Bogus;
using System;

namespace GiG.Core.Messaging.Kafka.Sample.Models
{
    public class Person
    {
        private Guid Id { get; set; }
        private string Name { get; set; }
        private string Surname { get; set; }
        private int Age { get; set; }
        private Address Address { get; set; }
        
        public override string ToString() => $"{nameof(Id)}: {Id}, {nameof(Name)}: {Name}, {nameof(Surname)}: {Surname}, {nameof(Age)}: {Age}, {nameof(Address)}: {Address}";

        
        public static Person Generate
        {
            get
            {
                var fakePerson = new Faker().Person;
                return new Person
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