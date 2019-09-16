namespace GiG.Core.Web.Sample.Contracts
{
    public interface ITransactionService
    {
        decimal Balance { get; }

        decimal Deposit(decimal amount);

        decimal Withdraw(decimal amount);
    }
}
