namespace GiG.Core.Logging.Sinks.RabbitMQ.Internal
{
    internal class RabbitMQSinkSslOptions
    {
        public bool IsEnabled { get; set; }

        public string CertPath { get; set; }

        public string CertPassphrase { get; set; }

        public string ServerName { get; set; }
    }
}