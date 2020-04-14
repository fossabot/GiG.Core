using GiG.Core.Data.KVStores.Abstractions;

namespace GiG.Core.Performance.Data.KVStores.Providers.Etcd.Read
{
    public class StringSerializer : IDataSerializer<string>
    {
        public string GetFromString(string value)
        {
            return value;
        }

        public string ConvertToString(string value)
        {
            return value;
        }
    }
}