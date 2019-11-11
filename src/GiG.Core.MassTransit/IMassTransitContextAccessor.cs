﻿namespace GiG.Core.MassTransit
{
    /// <summary>
    /// The MassTransit Context Accessor.
    /// </summary>
    public interface IMassTransitContextAccessor
    {
        /// <summary>
        /// The <see cref="MassTransitContext"/> for the current message.
        /// </summary>
        MassTransitContext MassTransitContext { get; set; }
    }
}