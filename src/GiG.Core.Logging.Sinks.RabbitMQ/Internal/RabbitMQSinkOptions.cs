namespace GiG.Core.Logging.Sinks.RabbitMQ.Internal
{
    internal class RabbitMQSinkOptions
    {
        public string DeliveryMode { get; set; } = "NonDurable";
        public string Exchange { get; set; } = "Logging";
        public string ExchangeType { get; set; } = "direct";
        public ushort Heartbeat { get; set; } = 30;
        public string Password { get; set; } = "guest";
        public int Port { get; set; } = 5672;
        public string Hostname { get; set; } = "localhost";
        public string Username { get; set; } = "guest";
        public string VHost { get; set; } = "/";
        public int BatchPostingLimit { get; set; } = 5;
        public int PeriodInSeconds { get; set; } = 5;
    }
}
