﻿using System;
using GiG.Core.Orleans.Streams.Abstractions;
using GiG.Core.Orleans.Streams.Abstractions.Models;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Orleans;

namespace GiG.Core.Orleans.Streams
{
    /// <inheritDoc />
    public class CommandDispatcherFactory<TCommand, TSuccess, TFailure> : ICommandDispatcherFactory<TCommand, TSuccess, TFailure>
        where TCommand : class
        where TSuccess : class
        where TFailure : FailedEventBase
    {
        private readonly IClusterClient _clusterClient;
        private readonly ILoggerFactory _loggerFactory;
        private readonly IStreamFactory _streamFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="clusterClient">The Cluster Client.</param>
        /// <param name="loggerFactory">The Logger Factory.</param>
        /// <param name="streamFactory">The Stream Factory.</param>
        public CommandDispatcherFactory(IClusterClient clusterClient, ILoggerFactory loggerFactory, IStreamFactory streamFactory)
        {
            _clusterClient = clusterClient;
            _loggerFactory = loggerFactory;
            _streamFactory = streamFactory;
        }

        /// <inheritdoc />
        public ICommandDispatcher<TCommand, TSuccess, TFailure> Create(Guid streamId, [NotNull] string streamProviderName)
        {
            if (string.IsNullOrEmpty(streamProviderName)) throw new ArgumentException($"'{nameof(streamProviderName)}' must not be null, empty or whitespace.", nameof(streamProviderName));
            
            var logger = _loggerFactory.CreateLogger<CommandDispatcher<TCommand, TSuccess, TFailure>>();

            return new CommandDispatcher<TCommand, TSuccess, TFailure>(_clusterClient, _streamFactory, logger, streamId, streamProviderName);
        }
    }
}