using GiG.Core.Orleans.Tests.Integration.Contracts;
using Orleans;
using System.Threading.Tasks;

namespace GiG.Core.Orleans.Tests.Integration.Grains
{
    public class EchoTestGrain : Grain, IEchoTestGrain
    {
        private int _value;

        public Task SetValueAsync(int value)
        {
            _value = value;

            return Task.CompletedTask;
        }

        public Task<int> GetValueAsync()
        {
            return Task.FromResult(_value);
        }
    }
}