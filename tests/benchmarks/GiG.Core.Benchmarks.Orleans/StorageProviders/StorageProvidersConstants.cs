namespace GiG.Core.Benchmarks.Orleans.StorageProviders
{
    public static class StorageProvidersConstants
    {
        //Storage Providers
        public const string InMemory = "Memory";
        public const string MongoDb = "MongoDb";
        public const string DynamoDb = "DynamoDb";
        public const string Postgres = "Postgres";
        public const string Redis = "Redis";
        
        //States
        public const string PlayerState = "player";
        
        //Databases
        public const string DatabaseName = "grainstorage";
    }
}