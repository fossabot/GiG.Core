using System;

namespace GiG.Core.Orleans.Streams.Abstractions
{
    /// <summary>
    /// Command Dispatcher Factory.
    /// </summary>
    public interface ICommandDispatcherFactory<in TCommand, TSuccess, TFailure>
        where TCommand : class
        where TSuccess : class
        where TFailure : class
    {
        /// <summary>
        /// Returns an instance of <see cref="ICommandDispatcher{TCommand, TSuccess, TFailure}" /> given a Stream Provider Name.
        /// </summary>
        /// <param name="streamId">The stream identifier.</param>
        /// <param name="streamProviderName">The stream namespace.</param>
        /// <returns>The <see cref="ICommandDispatcher{TCommand, TSuccess, TFailure}" />.</returns>
        ICommandDispatcher<TCommand, TSuccess, TFailure> Create(Guid streamId, string streamProviderName);
    }
}