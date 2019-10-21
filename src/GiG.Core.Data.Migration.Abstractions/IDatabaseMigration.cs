namespace GiG.Core.Data.Migration.Abstractions
{
    /// <summary>
    /// Database Migration.
    /// </summary>
    public interface IDatabaseMigration
    {
        /// <summary>
        /// Database Migration Script using <see cref="MigrationOptions"/>.
        /// </summary>
        /// <param name="options">The <see cref="MigrationOptions"/>.</param>
        void Migrate(MigrationOptions options);
    }
}