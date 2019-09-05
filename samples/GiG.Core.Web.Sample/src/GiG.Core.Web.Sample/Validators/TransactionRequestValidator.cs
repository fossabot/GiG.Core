using FluentValidation;
using GiG.Core.Web.Sample.Contracts;

namespace GiG.Core.Web.Sample.Validators
{
    public class TransactionRequestValidator : AbstractValidator<TransactionRequest>
    {
        public TransactionRequestValidator()
        {
            RuleFor(x => x.Amount).GreaterThan(0);
        }
    }
}
