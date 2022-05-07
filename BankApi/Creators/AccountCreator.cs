using BankApi.Domain.Entities;
using BankApi.Domain.Interfaces;
using BankApi.Domain.Models.Requests;

namespace BankApi.Creators
{
    public class AccountCreator : IAccountCreator
    {
        public IAccountWriter AccountWriter { get; }

        public AccountCreator(IAccountWriter accountWriter)
        {
            AccountWriter = accountWriter ?? throw new ArgumentNullException(nameof(accountWriter));
        }

        public async Task<Account> CreateAccount(EventRequest request)
        {
            var account = new Account
            {
                Id = request.Destination
            };

            await AccountWriter.CreateAccountAsyn(account);

            return account;
        }
    }
}
