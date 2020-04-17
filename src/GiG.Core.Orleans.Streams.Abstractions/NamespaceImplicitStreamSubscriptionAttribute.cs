using Orleans;
using System;

namespace GiG.Core.Orleans.Streams.Abstractions
{
    /// <summary>
    /// The <see cref="NamespaceImplicitStreamSubscriptionAttribute"/> attribute is used to mark grains as implicit stream
    /// subscriptions by filtering stream namespaces to subscribe using a stream namespaces with prefix.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class NamespaceImplicitStreamSubscriptionAttribute : ImplicitStreamSubscriptionAttribute
    {
        /// <summary>
        /// The stream namespace to subscribe to.
        /// </summary>
        /// <param name="streamNamespace">The stream namespace.</param>
        public NamespaceImplicitStreamSubscriptionAttribute(string streamNamespace)
            : base(new NamespacePredicate(streamNamespace))
        {
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="domain">The domain of the stream.</param>
        /// <param name="streamType">The stream type.</param>
        /// <param name="version">The version number of the model.</param>
        public NamespaceImplicitStreamSubscriptionAttribute(string domain, string streamType, uint version)
            : base(new NamespacePredicate(domain, streamType, version))
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="domain">The domain of the stream.</param>
        /// <param name="streamType">The stream type.</param>
        public NamespaceImplicitStreamSubscriptionAttribute(string domain, string streamType)
            : base(new NamespacePredicate(domain, streamType))
        {
        }
    }
}