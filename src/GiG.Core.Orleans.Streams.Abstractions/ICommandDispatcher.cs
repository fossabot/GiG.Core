using System;
using System.Threading;
using System.Threading.Tasks;
using GiG.Core.Orleans.Streams.Abstractions.Models;

namespace GiG.Core.Orleans.Streams.Abstractions
{
    /// <summary>
    /// The Command Dispatcher.
    /// </summary>
    /// <typeparam name="TCommand">The Command.</typeparam>
    /// <typeparam name="TSuccess">The Success Event.</typeparam>
    /// <typeparam name="TFailure">The Failure Event.</typeparam>
    public interface ICommandDispatcher<in TCommand, TSuccess, TFailure> : IAsyncDisposable
        where TCommand : class
        where TSuccess : class
        where TFailure : class
    {
        /// <summary>
        /// Appends a command to the instance of <see cref="ICommandDispatcher{TCommand, TSuccess, TFailure}" /> given a command namespace.
        /// </summary>
        /// <param name="command">The Command.</param>
        /// <param name="commandNamespace">The Command Namespace.</param>
        /// <returns>The <see cref="ICommandDispatcher{TCommand, TSuccess, TFailure}" />.</returns>
        ICommandDispatcher<TCommand, TSuccess, TFailure> WithCommand(TCommand command, string commandNamespace);

        /// <summary>
        /// Appends a success event to the instance of <see cref="ICommandDispatcher{TCommand, TSuccess, TFailure}" /> given a success event namespace and subscribes to the success stream.
        /// </summary>
        /// <param name="successEventNamespace">The Success Event Namespace.</param>
        /// <returns>The <see cref="ICommandDispatcher{TCommand, TSuccess, TFailure}" />.</returns>
        ICommandDispatcher<TCommand, TSuccess, TFailure> WithSuccessEvent(string successEventNamespace);

        /// <summary>
        /// Appends a failure event to the instance of <see cref="ICommandDispatcher{TCommand, TSuccess, TFailure}" /> given a failure event namespace and subscribes to the failure stream.
        /// </summary>
        /// <param name="failureEventNamespace">The Failure Event Namespace.</param>
        /// <returns>The <see cref="ICommandDispatcher{TCommand, TSuccess, TFailure}" />.</returns>
        ICommandDispatcher<TCommand, TSuccess, TFailure> WithFailureEvent(string failureEventNamespace);

        /// <summary>
        /// Dispatches the command and handles the respective responses.
        /// </summary>
        /// <param name="timeoutInMilliseconds">The Timeout in Milliseconds.</param>
        /// <param name="cancellationToken">The Cancellation Token.</param>
        /// <returns>The <see cref="CommandDispatcherResponse{TSuccess}" />.</returns>
        Task<CommandDispatcherResponse<TSuccess>> DispatchAsync(int timeoutInMilliseconds, CancellationToken cancellationToken = default);
    }
}