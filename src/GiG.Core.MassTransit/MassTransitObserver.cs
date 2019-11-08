using MassTransit;
using System;
using System.Threading.Tasks;

namespace GiG.Core.MassTransit
{
    public class MassTransitObserver : IConsumeObserver
    {
        private readonly IMassTransitContextFactory _massTransitContextFactory;

        public MassTransitObserver(IMassTransitContextFactory massTransitContextFactory)
        {
            _massTransitContextFactory = massTransitContextFactory;
        }

        public Task ConsumeFault<T>(ConsumeContext<T> context, Exception exception) where T : class
        {
            return Task.CompletedTask;
        }

        public Task PostConsume<T>(ConsumeContext<T> context) where T : class
        {
            return Task.CompletedTask;
        }

        public Task PreConsume<T>(ConsumeContext<T> context) where T : class
        {
            _massTransitContextFactory.Create(context.CorrelationId.GetValueOrDefault());
            return Task.CompletedTask;
        }
    }
}
