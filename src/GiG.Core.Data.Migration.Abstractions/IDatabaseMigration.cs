namespace GiG.Core.Data.Migration.Abstractions
{
    /// <summary>
    /// Database Migration.
    /// </summary>
    public interface IDatabaseMigration
    {
        /// <summary>
        /// Migrate DB Script using  <see cref="MigrationOptions"/>.
        /// </summary>
        /// <param name="options">The Migration Options <see cref="MigrationOptions"/>.</param>
        void Migrate(MigrationOptions options);
    }
}