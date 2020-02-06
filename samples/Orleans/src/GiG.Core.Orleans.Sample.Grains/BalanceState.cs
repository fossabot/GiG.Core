namespace GiG.Core.Orleans.Sample.Grains
{
    /// <summary>
    /// State class for the Transaction Grain.
    /// </summary>
    public class BalanceState
    {
        /// <summary>
        /// The balance.
        /// </summary>
        public decimal Amount { get; set; }
    }
}