namespace GiG.Core.Data.KVStores.Abstractions
{
    public interface IDataProviderOptions<T, TOptions>
    {
        TOptions Value { get; }
    }

    public class DataProviderOptions<T, TOptions> : IDataProviderOptions<T, TOptions>
    {
        
        public DataProviderOptions(TOptions options)
        {
            Value = options;
        }

        public TOptions Value { get; }
    }
}