using GiG.Core.Data.KVStores.Abstractions;
using System;

namespace GiG.Core.Data.KVStores
{
    public class MemoryDataStore<T> : IDataStore<T>
    {
        private T Data { get; set; }
        
        public T Get()
        {
            return Data;
        }

        public void Set(T model)
        {
            Data = model;
        }
    }
}