namespace GiG.Core.Data.Migration.Abstractions
{
    public interface IDatabaseMigration
    {
        void Migrate(MigrationOptions options);
    }
}
