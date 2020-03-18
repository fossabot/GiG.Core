using System;
using System.Threading;
using System.Threading.Tasks;
using GiG.Core.Orleans.Streams.Abstractions;
using GiG.Core.Orleans.Streams.Abstractions.Models;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Streams;

namespace GiG.Core.Orleans.Streams
{
    /// <inheritDoc />
    public class CommandDispatcher<TCommand, TSuccess, TFailure> : ICommandDispatcher<TCommand, TSuccess, TFailure>
        where TCommand : class
        where TSuccess : class
        where TFailure : FailedEventBase
    {
        private readonly ILogger<CommandDispatcher<TCommand, TSuccess, TFailure>> _logger;
        private readonly IStreamProvider _streamProvider;
        private readonly Guid _streamId;
        
        private SemaphoreSlim _semaphore;
        private IAsyncStream<TCommand> _commandStream;
        private IAsyncStream<TSuccess> _successStream;
        private IAsyncStream<TFailure> _failureStream;
        private StreamSubscriptionHandle<TSuccess> _successStreamHandle;
        private StreamSubscriptionHandle<TFailure> _failureStreamHandle;

        private TCommand _command;
        private TSuccess _success;
        private TFailure _failure;

        private bool _isDisposing;
        private bool _isReleased;
        private const string TimeoutError = "timeout";
        private const string SubscribeAsyncNotCalledError= "Not Subscribed to Success or Failure Event";

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="clusterClient">The Orleans <see cref="IClusterClient"/>.</param>
        /// <param name="logger">The
        ///     <see>
        ///         <cref>ILogger{ICommandDispatcher{TCommand, TSuccess, TFailure}}</cref>.
        ///     </see>
        ///  </param>
        /// <param name="streamId">The stream identifier.</param>
        /// <param name="streamProviderName">The stream namespace.</param>
        internal CommandDispatcher(IClusterClient clusterClient,
            ILogger<CommandDispatcher<TCommand, TSuccess, TFailure>> logger, Guid streamId, string streamProviderName)
        {
            _logger = logger;
            _streamProvider = clusterClient.GetStreamProvider(streamProviderName);    
            _streamId = streamId;
        }

        /// <inheritdoc />
        public ICommandDispatcher<TCommand, TSuccess, TFailure> WithCommand(TCommand command, string commandNamespace)
        {
            _command = command;
            _commandStream = _streamProvider.GetStream<TCommand>(_streamId, commandNamespace);

            return this;
        }

        /// <inheritdoc />
        public ICommandDispatcher<TCommand, TSuccess, TFailure> WithSuccessEvent(string successEventNamespace)
        {
            _successStream = _streamProvider.GetStream<TSuccess>(_streamId, successEventNamespace);

            return this;
        }

        /// <inheritdoc />
        public ICommandDispatcher<TCommand, TSuccess, TFailure> WithFailureEvent(string failureEventNamespace)
        {
            _failureStream = _streamProvider.GetStream<TFailure>(_streamId, failureEventNamespace);

            return this;
        }

        /// <inheritdoc />
        public async Task SubscribeAsync()
        {
            if (_successStream != null)
            {
                _successStreamHandle = await _successStream.SubscribeAsync(SuccessHandler);
            }
            
            if (_failureStream != null)
            {
                _failureStreamHandle = await _failureStream.SubscribeAsync(FailureHandler);
            }
        }

        /// <inheritdoc />
        public async Task<CommandDispatcherResponse<TSuccess>> DispatchAsync(int timeoutInMilliseconds, CancellationToken cancellationToken = default)
        {
            try
            {
                if (_successStreamHandle == null || _failureStreamHandle == null)
                {
                    throw new InvalidOperationException(SubscribeAsyncNotCalledError);
                }
                
                _semaphore = new SemaphoreSlim(0, 1);
                
                await _commandStream.OnNextAsync(_command);

                await _semaphore.WaitAsync(timeoutInMilliseconds, cancellationToken);

                switch (_success)
                {
                    case null when _failure == default(TFailure):
                        return new CommandDispatcherResponse<TSuccess>()
                        {
                            ErrorCode = TimeoutError
                        };
                    case null:
                        return new CommandDispatcherResponse<TSuccess>()
                        {
                            ErrorCode = _failure.ErrorCode,
                            ErrorMessage = _failure.ErrorMessage
                        };
                    default:
                        return new CommandDispatcherResponse<TSuccess>()
                        {
                            Data = _success
                        };
                }
            }
            catch (Exception e)
            {
                _logger.LogWarning(e, e.Message);
                throw;
            }
        }

        private Task SuccessHandler(TSuccess data, StreamSequenceToken token = null)
        {
            if (!_isReleased)
            {
                _success = data;
                _semaphore.Release();
                _isReleased = true;
            }
            
            return Task.CompletedTask;
        }

        private Task FailureHandler(TFailure data, StreamSequenceToken token = null)
        {
            if (!_isReleased)
            {
                _failure = data;
                _semaphore.Release();
                _isReleased = true;
            }
            
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public async ValueTask DisposeAsync()
        {
            if (_isDisposing) return;

            _isDisposing = true;
         
            if (_successStreamHandle != null) await _successStreamHandle.UnsubscribeAsync();
            if (_failureStreamHandle != null) await _failureStreamHandle.UnsubscribeAsync();
            
            _semaphore?.Dispose();
        }
    }
}