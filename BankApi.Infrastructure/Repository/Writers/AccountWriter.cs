using BankApi.Domain.Entities;
using BankApi.Domain.Interfaces;

namespace BankApi.Infrastructure.Repository.Writers
{
    public class AccountWriter : IAccountWriter
    {
        public AccountListFakeDB AccountList { get; }
        public AccountWriter(AccountListFakeDB accountList)
        {
            AccountList = accountList ?? throw new ArgumentNullException(nameof(accountList));
        }

        public async Task ResetAccountsAsync()
        {
           await Task.Run(() => AccountList.Reset());
        }

        public async Task CreateAccountAsyn(Account account)
        {
            await Task.Run(() => AccountList.Accounts.Add(account));
        }

        public async Task UpdateBalance(Account account)
        {
            await Task.Run(() => AccountList.Accounts.First(x => x.Id == account.Id).Balance = account.Balance);
        }
    }
}
