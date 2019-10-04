namespace GiG.Core.Orleans.Storage.Npgsql.Configurations
{
    /// <summary>
    /// Npgsql Options for Grain Storage.
    /// </summary>
    public class NpgsqlOptions
    {
        /// <summary>
        /// Default Orleans Storage providers section name.
        /// </summary>
        public const string DefaultSectionName = "Orleans:StorageProviders";

        /// <summary>
        /// The Database Connection string.
        /// </summary>
        public string ConnectionString { get; set; }
    }
}
