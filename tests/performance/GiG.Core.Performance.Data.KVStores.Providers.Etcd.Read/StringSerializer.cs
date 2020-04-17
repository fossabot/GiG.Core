using GiG.Core.Data.KVStores.Abstractions;
using GiG.Core.Data.Serializers.Abstractions;

namespace GiG.Core.Performance.Data.KVStores.Providers.Etcd.Read
{
    public class StringSerializer : IDataSerializer<string>
    {
        public string Deserialize(string value)
        {
            return value;
        }

        public string Serialize(string value)
        {
            return value;
        }
    }
}