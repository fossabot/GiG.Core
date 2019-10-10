using System;

namespace GiG.Core.Messaging.MassTransit.RabbitMQ.Abstractions
{
    public class RabbitMQBusOptions
    {
        public const string DefaultSectionName = "RabbitMQ";
        public string Host { get; set; }
        public int Port { get; set; }
        public string VirtualHost { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public ushort Heartbeat { get; set; }
        public bool SslEnabled { get; set; }
        public string ConnectionName { get; set; }
    }
}