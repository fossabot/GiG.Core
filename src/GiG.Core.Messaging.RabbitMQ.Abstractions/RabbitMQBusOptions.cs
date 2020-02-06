namespace GiG.Core.Messaging.RabbitMQ.Abstractions
{
    /// <summary>
    /// RabbitMQ Bus Options.
    /// </summary>
    public class RabbitMQBusOptions
    {
        /// <summary>
        /// The configuration default section name.
        /// </summary>
        public const string DefaultSectionName = "RabbitMQ";

        /// <summary>
        /// The RabbitMQ host name to connect to.
        /// </summary>
        public string Host { get; set; } = "localhost";

        /// <summary>
        /// The RabbitMQ port to connect to.
        /// </summary>
        public int Port { get; set; } = 5672;

        /// <summary>
        /// The virtual host for the connection.
        /// </summary>
        public string VirtualHost { get; set; } = "/";

        /// <summary>
        /// The Username for connecting to the host.
        /// </summary>
        public string Username { get; set; } = "guest";

        /// <summary>
        /// The password for connecting to the host.
        /// </summary>
        public string Password { get; set; } = "guest";

        /// <summary>
        /// The heartbeat interval (in seconds) to keep the host connection alive.
        /// </summary>
        public ushort Heartbeat { get; set; }

        /// <summary>
        /// A value to indicate if SSL is enabled or not.
        /// </summary>
        public bool SslEnabled { get; set; }
        
        /// <summary>
        /// The connection name.
        /// </summary>
        public string ConnectionName { get; set; }
    }
}