namespace GiG.Core.Orleans.Abstractions.Configuration
{
    public class DashboardOptions
    {
        public const string DefaultConfigurationSection = "Orleans:Dashboard";
        public bool Enabled { get; set; }
        public int Port { get; set; } = 8181;
        public string Path { get; set; }
    }
}