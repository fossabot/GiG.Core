namespace GiG.Core.Benchmarks.Orleans.StorageProviders
{
    public static class StorageProvidersConstants
    {
        //Storage Providers
        public const string InMemory = "memory";
        public const string MongoDb = "mongodb";
        
        //States
        public const string PlayerState = "player";
        
        //Databases
        public const string DatabaseName = "grainstorage";
    }
    
    public static class ConnectionStrings
    {
        public static string MongoDb = "mongodb://root:example@localhost:27017";
    }
}