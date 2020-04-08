using FluentValidation;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GiG.Core.Validation.FluentValidation.MediatR
{
    /// <summary>
    /// Validation Pipeline Behaviour.
    /// </summary>
    /// <typeparam name="TRequest">The Request.</typeparam>
    /// <typeparam name="TResponse">The Response.</typeparam>
    public class ValidationPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="validators"></param>
        public ValidationPipelineBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        /// <summary>
        /// Handles a Request, checks for Errors and throws a ValidationException if any Errors are found.
        /// </summary>
        /// <param name="request">The Request.</param>
        /// <param name="cancellationToken">The Cancellation token.</param>
        /// <param name="next">The Request Handler delegate.</param>
        /// <returns></returns>
        public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var context = new ValidationContext(request);
            var failures = _validators
                .Select(v => v.Validate(context))
                .SelectMany(result => result.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Any())
            {
                throw new ValidationException(failures);
            }

            return next();
        }
    }
}