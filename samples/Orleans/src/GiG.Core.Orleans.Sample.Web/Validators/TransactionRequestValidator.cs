using FluentValidation;
using GiG.Core.Orleans.Sample.Web.Contracts;

namespace GiG.Core.Orleans.Sample.Web.Validators
{
    public class TransactionRequestValidator : AbstractValidator<TransactionRequest>
    {
        private const decimal MinimumAmount = 10;
        public TransactionRequestValidator()
        {
            RuleSet("Deposit", () => {
                RuleFor(x => x.Amount)
                .GreaterThanOrEqualTo(MinimumAmount)
                .WithMessage($"Deposit Amount must be greater than {MinimumAmount}.");
            });

            RuleSet("Withdraw", () => {
                RuleFor(x => x.Amount)
                .GreaterThan(0)
                .WithMessage("Withdraw Amount must be smaller or equal to the Balance, and greater than 0.");
            });
        }
    }
}
