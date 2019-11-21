using Orleans.Configuration;

namespace GiG.Core.Orleans.Silo.Abstractions
{
    /// <summary>
    /// Orleans Silo Options.
    /// </summary>
    public class SiloOptions
    {
        /// <summary>
        /// The configuration default section name.
        /// </summary>
        public const string DefaultSectionName = "Orleans:Silo";

        /// <summary>
        /// The port this silo uses for silo-to-silo communication.
        /// </summary>
        public int SiloPort { get; set; } = EndpointOptions.DEFAULT_SILO_PORT;

        /// <summary>
        /// The port this silo uses for silo-to-client (gateway) communication. Specify 0 to disable gateway functionality.
        /// </summary>
        public int GatewayPort { get; set; } = EndpointOptions.DEFAULT_GATEWAY_PORT;
    }
}