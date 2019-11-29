﻿namespace GiG.Core.Messaging.Kafka.Models
{
    public class Address
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }

        public override string ToString() => $"{nameof(Street)}: {Street}, {nameof(City)}: {City}, {nameof(State)}: {State}, {nameof(Zip)}: {Zip}";
    }
}