namespace GiG.Core.Orleans.Storage.Npgsql.Abstractions
{
    /// <summary>
    /// Npgsql Options for Grain Storage.
    /// </summary>
    public class NpgsqlOptions
    {
        /// <summary>
        /// The configuration default section name.
        /// </summary>
        public const string DefaultSectionName = "Orleans:StorageProviders";

        /// <summary>
        /// The Database Connection string.
        /// </summary>
        public string ConnectionString { get; set; }
    }
}