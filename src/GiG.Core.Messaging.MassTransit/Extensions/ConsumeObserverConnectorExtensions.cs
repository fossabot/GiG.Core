﻿using GiG.Core.Messaging.MassTransit.Internal;
using JetBrains.Annotations;
using MassTransit;
using MassTransit.Pipeline;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace GiG.Core.Messaging.MassTransit.Extensions
{
    /// <summary>
    /// Consumer Observer Connector Extensions.
    /// </summary>
    public static class ConsumeObserverConnectorExtensions
    {
        /// <summary>
        /// Adds the default Consumer Observer to a MassTransit Consumer.
        /// </summary>
        /// <param name="consumeObserverConnector">The <see cref="IConsumeObserverConnector"/> to which the <see cref="IConsumeObserver"/> will be added.</param>
        /// <param name="serviceProvider">The <see cref="IServiceProvider"/>.</param>
        /// <returns>The<see cref="IConsumeObserverConnector"/>. </returns>
        public static IConsumeObserverConnector AddDefaultConsumerObserver([NotNull] this IConsumeObserverConnector consumeObserverConnector, [NotNull] IServiceProvider serviceProvider)
        {
            if (consumeObserverConnector == null) throw new ArgumentNullException(nameof(consumeObserverConnector));
            if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));

            var contextFactory = serviceProvider.GetRequiredService<IMassTransitContextFactory>();
            consumeObserverConnector.ConnectConsumeObserver(new MassTransitConsumerObserver(contextFactory));
            return consumeObserverConnector;
        }


        /// <summary>
        /// Adds the Activity Consumer Observer to a MassTransit Consumer.
        /// </summary>
        /// <param name="consumeObserverConnector">The <see cref="IConsumeObserverConnector"/> to which the <see cref="IConsumeObserver"/> will be added.</param>
        /// <returns>The<see cref="IConsumeObserverConnector"/>. </returns>
        public static IConsumeObserverConnector AddActivityConsumerObserver([NotNull] this IConsumeObserverConnector consumeObserverConnector)
        {
            if (consumeObserverConnector == null) throw new ArgumentNullException(nameof(consumeObserverConnector));

            consumeObserverConnector.ConnectConsumeObserver(new MassTransitConsumerActivityObserver());
            return consumeObserverConnector;
        }
    }
}