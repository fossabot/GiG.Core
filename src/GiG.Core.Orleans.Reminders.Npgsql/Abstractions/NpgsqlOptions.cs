namespace GiG.Core.Orleans.Reminders.Npgsql.Abstractions
{
    /// <summary>
    /// Npgsql Options for Reminders Storage.
    /// </summary>
    public class NpgsqlOptions
    {
        /// <summary>
        /// The configuration default section name.
        /// </summary>
        public const string DefaultSectionName = "Orleans:Reminders";

        /// <summary>
        /// The Database Connection string.
        /// </summary>
        public string ConnectionString { get; set; }
    }
}