using GreenPipes.Specifications;
using JetBrains.Annotations;
using MassTransit;
using System;

namespace GiG.Core.Messaging.MassTransit.Extensions
{
    /// <summary>
    /// Publisher Pipeline Configuration Extensions.
    /// </summary>
    public static class PublishPipelineConfiguratorExtensions
    {
        /// <summary>
        /// Sets the Fault Address for a Publish Endpoint.
        /// </summary>
        /// <param name="publishPipelineConfigurator">The <see cref="IPublishPipelineConfigurator"/> on which to set the Fault address. </param>
        /// <param name="faultAddress"> The <see cref="Uri" /> to be used for Faulted messages. </param>
        /// <returns>The <see cref="IPublishPipelineConfigurator"/>. </returns>
        public static IPublishPipelineConfigurator UseFaultAddress<T>([NotNull] this IPublishPipelineConfigurator publishPipelineConfigurator, [NotNull ] Uri faultAddress)
            where T : class
        {
            if (publishPipelineConfigurator == null) throw new ArgumentNullException(nameof(publishPipelineConfigurator));
            if (faultAddress == null) throw new ArgumentNullException(nameof(faultAddress));

            publishPipelineConfigurator.ConfigurePublish(x =>
                {
                    x.AddPipeSpecification(new DelegatePipeSpecification<PublishContext<T>>(p =>
                    {
                        p.FaultAddress = faultAddress;
                    }));
                });

            return publishPipelineConfigurator;
        }
    }
}