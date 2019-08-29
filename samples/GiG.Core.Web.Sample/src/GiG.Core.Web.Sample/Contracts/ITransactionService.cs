namespace GiG.Core.Web.Sample.Contracts
{
    public interface ITransactionService
    {
        decimal GetBalance();

        decimal Deposit(decimal amount);

        decimal Withdraw(decimal amount);
    }
}
