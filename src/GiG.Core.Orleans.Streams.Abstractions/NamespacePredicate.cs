using Orleans;
using Orleans.Streams;
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
    }
    
    /// <summary>
    /// <see cref="IStreamNamespacePredicate"/> implementation allowing to filter stream namespaces with prefix.
    /// </summary>
    public class NamespacePredicate : IStreamNamespacePredicate
    {
        private readonly string _targetStreamNamespace;

        /// <summary>
        /// Creates an instance of <see cref="NamespacePredicate"/> with the specified namespace with prefix.
        /// </summary>
        /// <param name="streamNamespace">The stream namespace.</param>
        public NamespacePredicate(string streamNamespace)
        {
            _targetStreamNamespace = StreamHelper.GetNamespace(streamNamespace);
        }
        
        /// <summary>
        /// Creates an instance of <see cref="NamespacePredicate"/> with the specified namespace with prefix.
        /// </summary>
        /// <param name="domain">The domain of the stream.</param>
        /// <param name="streamType">The stream type.</param>
        /// <param name="version">The version number of the model.</param>
        public NamespacePredicate(string domain, string streamType, uint version = 1)
        {
            _targetStreamNamespace = StreamHelper.GetNamespace(domain, streamType, version);
        }

        /// <inheritdoc />
        public bool IsMatch(string streamNamespace)
        {
            return string.Equals(_targetStreamNamespace, streamNamespace?.Trim());
        }
    }
}