using BankApi.Domain.Entities;

namespace BankApi.Infrastructure
{
    public class AccountListFakeDB
    {
        public List<Account> Accounts { get; set; }

        public AccountListFakeDB()
        {
            Accounts = CreateAccountsDefault();

        }
        public void Reset()
        {
            Accounts = CreateAccountsDefault();
        }

        public static List<Account> CreateAccountsDefault()
        {
            return new List<Account>() { new Account { Id = "101" }, new Account { Id = "102" } };
        }
    }
}
