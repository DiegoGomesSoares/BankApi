using BankApi.Domain.Entities;
using BankApi.Domain.Interfaces;

namespace BankApi.Processors
{
    public class AccountCashin : IAccountCashin
    {
        public IAccountWriter AccountWriter { get; }

        public AccountCashin(IAccountWriter accountWriter)
        {
            AccountWriter = accountWriter ?? throw new ArgumentNullException(nameof(accountWriter));
        }

        public async Task Cashin(Account account, decimal amount)
        {
            account.Balance += amount;

            await AccountWriter.UpdateBalance(account);
        }
    }
}
