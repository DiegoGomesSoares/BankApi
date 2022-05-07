using BankApi.Domain.Entities;
using BankApi.Domain.Interfaces;

namespace BankApi.Infrastructure.Repository.Readers
{
    public class AccountReader : IAccountReader
    {
        public AccountListFakeDB AccountList { get; }

        public AccountReader(AccountListFakeDB accountList)
        {
            AccountList = accountList ?? throw new ArgumentNullException(nameof(accountList));
        }
       
        public async Task<Account> GetByIdAsyn(string accountId)
        {
            return await Task.FromResult(AccountList.Accounts.FirstOrDefault(x => x.Id.Equals(accountId)));
        }
    }
}
