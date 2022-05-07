using BankApi.Domain.Entities;
using BankApi.Domain.Interfaces;

namespace BankApi.Processors
{
    public class AccountCashout : IAccountCashout
    {
        public IAccountWriter AccountWriter { get; }

        public AccountCashout(IAccountWriter accountWriter)
        {
            AccountWriter = accountWriter ?? throw new ArgumentNullException(nameof(accountWriter));
        }

        public async Task Cashout(Account account, decimal amount)
        {
            account.Balance -= amount;

            await AccountWriter.UpdateBalance(account);
        }
    }
}
