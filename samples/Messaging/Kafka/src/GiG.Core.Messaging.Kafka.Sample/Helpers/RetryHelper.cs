using System;
using System.Threading.Tasks;

namespace GiG.Core.Messaging.Kafka.Sample.Helpers
{
    public static class RetryHelper
    {
        private const byte MaxBackOffAmount = 5;
        private static byte _retryBackOffAmount;
        
        /// <summary>
        /// Retry On Exception with exponential time delay between retries.  If retryAttempts is set to -1 (default value) the method will keep trying until operation succeeds. 
        /// </summary>
        public static async Task RetryOnExceptionAsync<TException>(Func<Task> operation, int retryAttempts = -1 ) where TException : Exception
        {
            var attempts = 0;
            do
            {
                try
                {
                    attempts++;
                    await operation();
                    _retryBackOffAmount = 0;
                    break;
                }
                catch (TException ex)
                {
                    if (retryAttempts != -1 && attempts == retryAttempts)
                        throw;
                    
                    await CreateDelayForException(ex);
                }
            } while (true);
        }

        private static Task CreateDelayForException(Exception ex)
        {
            var jitter = new Random();
            var delay = TimeSpan.FromSeconds(Math.Pow(2, _retryBackOffAmount)) +
                        TimeSpan.FromMilliseconds(jitter.Next(0, 250));
            
            if (_retryBackOffAmount < MaxBackOffAmount)
            {
                _retryBackOffAmount++;
            }

            return Task.Delay(delay);
        }
    }
}