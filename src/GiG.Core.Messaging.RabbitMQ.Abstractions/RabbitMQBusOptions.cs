namespace GiG.Core.Messaging.RabbitMQ.Abstractions
{
    /// <summary>
    /// RabbitMQ Bus Options.
    /// </summary>
    public class RabbitMQBusOptions
    {
        /// <summary>
        /// RabbitMQ default section name.
        /// </summary>
        public const string DefaultSectionName = "RabbitMQ";

        /// <summary>
        /// The RabbitMQ host to connect to (should be a valid hostname).
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// The RabbitMQ port to connect.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// The virtual host for the connection.
        /// </summary>
        public string VirtualHost { get; set; }

        /// <summary>
        /// The Username for connecting to the host.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The password for connection to the host.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// The heartbeat interval (in seconds) to keep the host connection alive.
        /// </summary>
        public ushort Heartbeat { get; set; }

        /// <summary>
        /// Indicates whether SSL is enabled or not.
        /// </summary>
        public bool SslEnabled { get; set; }
        
        /// <summary>
        /// The name of the connection.
        /// </summary>
        public string ConnectionName { get; set; }
    }
}